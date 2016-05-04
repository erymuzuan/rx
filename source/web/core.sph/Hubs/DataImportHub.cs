using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Web.ViewModels;
using Humanizer;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Web.Hubs
{
    [Authorize(Roles = "developers,administrators")]
    public class DataImportHub : Hub
    {

        private static bool m_isCancelRequested;
        private readonly HttpClient m_client = new HttpClient { BaseAddress = new Uri($"{ConfigurationManager.RabbitMqManagementScheme}://{ConfigurationManager.RabbitMqHost}:{ConfigurationManager.RabbitMqManagementPort}") };

        public DataImportHub()
        {
            var byteArray = Encoding.ASCII.GetBytes($"{ConfigurationManager.RabbitMqUserName}:{ConfigurationManager.RabbitMqPassword}");
            m_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public async Task RequestCancel()
        {
            m_isCancelRequested = true;
            await Task.Delay(5.Seconds());
            // now purge the queues
            await m_client.DeleteAsync($"/api/queues/{ConfigurationManager.ApplicationName}/persistence/contents");
            await m_client.DeleteAsync($"/api/queues/{ConfigurationManager.ApplicationName}/es.data-import/contents");
        }

        public async Task TruncateData(string name, ImportDataViewModel model)
        {
            // purge the queues
            await m_client.DeleteAsync($"/api/queues/{ConfigurationManager.ApplicationName}/persistence/contents");
            await m_client.DeleteAsync($"/api/queues/{ConfigurationManager.ApplicationName}/es.data-import/contents");

            // delete the elasticsearch
            using (var client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) })
            {
                var message = new HttpRequestMessage(HttpMethod.Delete,
                    $"{ConfigurationManager.ElasticSearchIndex}/{model.Entity.ToLowerInvariant()}/_query")
                {
                    Content = new StringContent(
@"{
   ""query"": {
      ""match_all"": {}
   }
}")
                };
                await client.SendAsync(message);
            }

            // truncate SQL Table
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var truncateCommand = new SqlCommand($"TRUNCATE TABLE [{ConfigurationManager.ApplicationName}].[{model.Entity}]", conn))
            using (var dbccCommand = new SqlCommand($"DBCC SHRINKDATABASE ({ConfigurationManager.ApplicationName})", conn))
            {
                await conn.OpenAsync();

                await truncateCommand.ExecuteNonQueryAsync();
                await dbccCommand.ExecuteNonQueryAsync();
            }

        }

        public class Queue
        {
            public string Name { get; set; }
            public int MessagesCount { get; set; }
            public double Rate { get; set; }
            public int Unacked { get; set; }

            public Queue(string name, int count, double rate)
            {
                this.Name = name;
                this.MessagesCount = count;
                this.Rate = rate;
            }

            public Queue(string name, int count, double rate, int unacked)
            {
                this.Name = name;
                this.MessagesCount = count;
                this.Rate = rate;
                this.Unacked = unacked;
            }
        }

        public class ProgressData
        {
            public int Rows { get; set; }
            public Queue ElasticsearchQueue { get; set; } = new Queue("es.data-import", 0, 0);
            public Queue SqlServerQueue { get; set; } = new Queue("persistence", 0, 0);

            public ProgressData(int rows)
            {
                Rows = rows;
            }

            public static ProgressData Parse(string es, string sql)
            {
                var pd = new ProgressData(-1)
                {
                    SqlServerQueue = ParseQueue(sql),
                    ElasticsearchQueue = ParseQueue(es)
                };


                return pd;
            }
            private static Queue ParseQueue(string json)
            {
                var jo = JObject.Parse(json);
                var name = jo.SelectToken("$.name").Value<string>();
                var messages = jo.SelectToken("$.messages").Value<int>();
                var deliveries = jo.SelectToken("$.message_stats.deliver_details.rate").Value<double>();
                var unacked = jo.SelectToken("$.messages_unacknowledged").Value<int>();

                return new Queue(name, messages, deliveries, unacked);

            }
        }

        public async Task<object> Execute(string name, ImportDataViewModel model, IProgress<ProgressData> progress)
        {
            m_isCancelRequested = false;
            var context = new SphDataContext();
            var adapter = await context.LoadOneAsync<Adapter>(x => x.Id == model.Adapter);
            var mapping = await context.LoadOneAsync<TransformDefinition>(x => x.Id == model.Map);

            dynamic tableAdapter = GetTableAdapterInstance(model, adapter);
            if (null == tableAdapter) return new
            {
                statusCode = 404,
                message = $"{adapter.AssemblyName}.dll does not exist, you may have to build your adapter before it could be used"
            };

            dynamic map = GetMapInstance(mapping);
            if (null == map) return new
            {
                statusCode = 404,
                message = $"{mapping.AssemblyName}.dll in web\\bin does not exist, you may have to build your TransformDefinition before it could be used"
            };

            var rows = 0;
            var headers = new Dictionary<string, object>
            {
                {"data-import", true}
            };

            var rabbitTask = GetQueueStatusAsync(progress);
            var lo = await tableAdapter.LoadAsync(model.Sql, 1, model.BatchSize, false);
            while (lo.ItemCollection.Count > 0)
            {
                using (var session = context.OpenSession())
                {
                    foreach (var source in lo.ItemCollection)
                    {
                        rows++;
                        var item = await map.TransformAsync(source);
                        Console.WriteLine("ENT_ID:" + item.Id);
                        session.Attach(item);
                    }

                    await session.SubmitChanges("Import", headers);
                    if (model.DelayThrottle.HasValue)
                        await Task.Delay(model.DelayThrottle.Value);

                    progress.Report(new ProgressData(rows));
                }
                if (m_isCancelRequested)
                    return new { statusCode = 206, rows, message = $"Stop requested after {rows} rows imported" };

                lo = await tableAdapter.LoadAsync(model.Sql, lo.CurrentPage + 1, model.BatchSize, false);
            }
            await rabbitTask;
            return new
            {
                statusCode = 200,
                success = true,
                rows,
                message = $"successfully imported {rows}",
                status = "OK"
            };
        }

        private async Task GetQueueStatusAsync(IProgress<ProgressData> progress)
        {
            var sqlMessages = 1;
            var esMessages = 1;
            var count = 10;
            while ((sqlMessages > 0 || esMessages > 0) || count > 0)
            {
                await Task.Delay(2.Seconds());

                var sql = await m_client.GetStringAsync($"api/queues/{ConfigurationManager.ApplicationName}/persistence");
                var es = await m_client.GetStringAsync($"api/queues/{ConfigurationManager.ApplicationName}/es.data-import");
                var pd = ProgressData.Parse(es, sql);
                progress.Report(pd);

                sqlMessages = pd.SqlServerQueue.MessagesCount;
                esMessages = pd.ElasticsearchQueue.MessagesCount;

                count--;
            }

        }
        public async Task<object> Preview(ImportDataViewModel model)
        {
            var context = new SphDataContext();
            var adapter = await context.LoadOneAsync<Adapter>(x => x.Id == model.Adapter);

            dynamic tableAdapter = GetTableAdapterInstance(model, adapter);
            if (null == tableAdapter) return new
            {
                statusCode = 404,
                message = $"{adapter.AssemblyName}.dll does not exist, you may have to build your adapter before it could be used"
            };

            var lo = await tableAdapter.LoadAsync(model.Sql);
            return lo;

        }

        private object GetTableAdapterInstance(ImportDataViewModel model, Adapter adapter)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var dll = assemblies.SingleOrDefault(x => x.GetName().Name == adapter.AssemblyName);
            if (null == dll) return null;
            var adapterTypeName = $"{adapter.CodeNamespace}.{model.Table}Adapter";
            var adapterType = dll.GetType(adapterTypeName);
            return Activator.CreateInstance(adapterType);
        }

        private static object GetMapInstance(TransformDefinition mapping)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var dll = assemblies.SingleOrDefault(x => x.GetName().Name == mapping.AssemblyName);
            if (null == dll) return null;

            var type = dll.GetType(mapping.FullTypeName);
            return Activator.CreateInstance(type);
        }
    }
}