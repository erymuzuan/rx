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
        public void RequestCancel()
        {
            m_isCancelRequested = true;
        }

        public async Task TruncateData(string name, ImportDataViewModel model)
        {
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
            protected double Rate { get; set; }

            public Queue(string name, int count, double rate)
            {
                this.Name = name;
                this.MessagesCount = count;
                this.Rate = rate;

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

        private readonly HttpClient m_client = new HttpClient { BaseAddress = new Uri($"{ConfigurationManager.RabbitMqManagementScheme}://{ConfigurationManager.RabbitMqHost}:{ConfigurationManager.RabbitMqManagementPort}") };
        private async Task GetQueueStatusAsync(IProgress<ProgressData> progress)
        {
            await Task.Delay(5.Seconds());
            var byteArray = Encoding.ASCII.GetBytes($"{ConfigurationManager.RabbitMqUserName}:{ConfigurationManager.RabbitMqPassword}");
            m_client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
            var sql = 1;
            var es = 1;
            while (sql > 0 || es > 0)
            {
                await Task.Delay(2.Seconds());

                var json = await m_client.GetStringAsync($"api/queues/{ConfigurationManager.ApplicationName}/persistence");
                var jo = JObject.Parse(json);
                sql = jo.SelectToken("$.messages").Value<int>();
                var sqlRate = jo.SelectToken("$.messages_details.rate").Value<double>();

                var esjson = await m_client.GetStringAsync($"api/queues/{ConfigurationManager.ApplicationName}/es.data-import");
                var ejo = JObject.Parse(esjson);
                es = ejo.SelectToken("$.messages").Value<int>();
                var esRate = ejo.SelectToken("$.messages_details.rate").Value<double>();
                progress.Report(new ProgressData(-1)
                {
                    SqlServerQueue = new Queue("persistence", sql, sqlRate),
                    ElasticsearchQueue = new Queue("es.data-import", es, esRate)
                });
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