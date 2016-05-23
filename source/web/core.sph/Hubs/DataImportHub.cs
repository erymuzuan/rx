using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Humanizer;
using Microsoft.AspNet.SignalR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Web.Hubs
{
    [Authorize(Roles = "developers,administrators")]
    public class DataImportHub : Hub
    {

        private static bool m_isCancelRequested;
        private readonly HttpClient m_rabbitManagementHttpClient = new HttpClient { BaseAddress = new Uri($"{ConfigurationManager.RabbitMqManagementScheme}://{ConfigurationManager.RabbitMqHost}:{ConfigurationManager.RabbitMqManagementPort}") };
        private readonly HttpClient m_elasticsearchHttpClient = new HttpClient { BaseAddress = new Uri($"{ConfigurationManager.ElasticSearchHost}") };

        public DataImportHub()
        {
            var byteArray = Encoding.ASCII.GetBytes($"{ConfigurationManager.RabbitMqUserName}:{ConfigurationManager.RabbitMqPassword}");
            m_rabbitManagementHttpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", Convert.ToBase64String(byteArray));
        }

        public async Task RequestCancel()
        {
            m_isCancelRequested = true;
            await Task.Delay(5.Seconds());
            // now purge the queues
            await m_rabbitManagementHttpClient.DeleteAsync($"/api/queues/{ConfigurationManager.ApplicationName}/persistence/contents");
            await m_rabbitManagementHttpClient.DeleteAsync($"/api/queues/{ConfigurationManager.ApplicationName}/es.data-import/contents");
        }

        public async Task TruncateData(string name, DataTransferDefinition model)
        {
            // purge the queues
            await m_rabbitManagementHttpClient.DeleteAsync($"/api/queues/{ConfigurationManager.ApplicationName}/persistence/contents");
            await m_rabbitManagementHttpClient.DeleteAsync($"/api/queues/{ConfigurationManager.ApplicationName}/es.data-import/contents");

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
            public Exception Exception { get; set; }
            public object Data { get; set; }
            public string ErrorId { get; set; }
            public int Rows { get; set; }
            public int ElasticsearchRows { get; set; }
            public int SqlRows { get; set; }
            [JsonIgnore]
            public TimeSpan Elapsed { set; get; }
            public string TotalTime => this.Elapsed.Humanize();
            public Queue ElasticsearchQueue { get; set; } = new Queue("es.data-import", 0, 0);
            public Queue SqlServerQueue { get; set; } = new Queue("persistence", 0, 0);

            public ProgressData(int rows)
            {
                Rows = rows;
            }
            public ProgressData(Exception exception, object data, string errorId)
            {
                Exception = exception;
                Data = data;
                ErrorId = errorId;
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

        public void IgnoreRow(string errorId)
        {
            var folder = $"{ConfigurationManager.WebPath}\\App_Data\\data-imports";

            var dataFile = $"{folder}\\{errorId}.data";
            if (System.IO.File.Exists(dataFile))
                System.IO.File.Delete(dataFile);
            var errorFile = $"{folder}\\{errorId}.error";
            if (System.IO.File.Exists(errorFile))
                System.IO.File.Delete(errorFile);

        }

        public async Task<object> ImportOneRow(string errorId, DataTransferDefinition model, string json)
        {
            var folder = $"{ConfigurationManager.WebPath}\\App_Data\\data-imports";
            var context = new SphDataContext();
            var adapter = await context.LoadOneAsync<Adapter>(x => x.Id == model.InboundAdapter);
            var mapping = await context.LoadOneAsync<TransformDefinition>(x => x.Id == model.InboundMap);

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

            var headers = new Dictionary<string, object>
            {
                {"data-import", model.IgnoreMessaging},
                {"sql.retry", model.SqlRetry},
                {"sql.wait", model.SqlWait},
                {"es.retry", model.EsRetry},
                {"es.wait", model.EsWait}
            };
            using (var session = context.OpenSession())
            {
                var type = GetSourceType(model, adapter);
                dynamic source = JsonConvert.DeserializeObject(json, type);
                try
                {
                    // TODO : try catch
                    var item = await map.TransformAsync(source);
                    session.Attach(item);
                    await session.SubmitChanges("Import", headers);
                }
                catch (Exception e)
                {
                    var log = new LogEntry(e);
                    System.IO.File.WriteAllText($"{folder}\\{errorId}.error", log.ToString());
                    return new
                    {
                        statusCode = 500,
                        success = false,
                        message = e.Message,
                        stackTrace = e.StackTrace,
                        type = e.GetType().FullName,
                        status = "Not OK"
                    };

                }
            }

            var dataFile = $"{folder}\\{errorId}.data";
            if (System.IO.File.Exists(dataFile))
                System.IO.File.Delete(dataFile);
            var errorFile = $"{folder}\\{errorId}.error";
            if (System.IO.File.Exists(errorFile))
                System.IO.File.Delete(errorFile);

            return new
            {
                statusCode = 200,
                success = true,
                message = "successfully imported",
                status = "OK"
            };
        }

        public async Task<object> Resume(string id, DataImportHistory log, IProgress<ProgressData> progress)
        {
            var sw = new Stopwatch();
            sw.Start();
            m_isCancelRequested = false;

            var modelPath = $"{ConfigurationManager.WebPath}\\App_Data\\data-imports\\{log.Name.ToIdFormat()}.json";
            var json = System.IO.File.ReadAllText(modelPath);
            var model = JsonConvert.DeserializeObject<DataTransferDefinition>(json);

            var statusTask = UpdateImportStatusAsync(model, sw, progress);
            var importTask = ImportDataAsync(model, sw, progress, log.PageNumber + 1, log.RowsRead, log.Errors);
            await Task.WhenAll(statusTask, importTask);
            sw.Stop();
            dynamic result = await importTask;

            await log.FinalizeAsync(model, result, this);
            await log.SaveAsync();

            return result;
        }

        public async Task<object> Execute(string name, DataTransferDefinition model, IProgress<ProgressData> progress)
        {
            var sw = new Stopwatch();
            sw.Start();
            m_isCancelRequested = false;

            var log = new DataImportHistory(model);
            await log.InitializeAsync(model, this);

            var statusTask = UpdateImportStatusAsync(model, sw, progress);
            var importTask = ImportDataAsync(model, sw, progress);
            await Task.WhenAll(statusTask, importTask);
            sw.Stop();
            dynamic result = await importTask;

            await log.FinalizeAsync(model, result, this);
            await log.SaveAsync();

            return result;
        }

        public class DataImportHistory
        {
            public DataImportHistory(DataTransferDefinition model)
            {
                this.Name = model.Name;
                this.DateTime = DateTime.Now;
            }

            public DataImportHistory()
            {

            }
            [JsonProperty("name")]
            public string Name { get; set; }
            [JsonProperty("dateTime")]
            public DateTime DateTime { get; set; }
            [JsonProperty("rows")]
            public int RowsRead { get; set; }
            [JsonProperty("errors")]
            public int Errors { get; set; }
            [JsonProperty("page")]
            public int PageNumber { get; set; }
            [JsonProperty("size")]
            public int PageSize { get; set; }
            [JsonProperty("sqlBefore")]
            public int SqlServerRowsBefore { get; set; }
            [JsonProperty("sqlAfter")]
            public int SqlServerRowsAfter { get; set; }
            [JsonProperty("elasticsearchBefore")]
            public int ElasticsearchBefore { get; set; }
            [JsonProperty("elasticsearchAfter")]
            public int ElasticsearchAfter { get; set; }

            public Task SaveAsync()
            {
                System.IO.File.WriteAllText($"{ConfigurationManager.WebPath}\\App_Data\\data-imports\\history\\{this.Name.ToIdFormat()}-{this.DateTime:yyyyMMdd.HHmmss}.log", this.ToJson());
                return Task.FromResult(0);
            }

            public async Task InitializeAsync(DataTransferDefinition model, DataImportHub hub)
            {
                var sqlTask = hub.GetSqlServerCountAsync(model);
                var esTask = hub.GetElasticsearchCountAsync(model);
                await Task.WhenAll(sqlTask, esTask);

                this.SqlServerRowsBefore = await sqlTask;
                this.ElasticsearchBefore = await esTask;

            }
            public async Task FinalizeAsync(DataTransferDefinition model, dynamic result, DataImportHub hub)
            {
                var sqlTask = hub.GetSqlServerCountAsync(model);
                var esTask = hub.GetElasticsearchCountAsync(model);
                await Task.WhenAll(sqlTask, esTask);

                this.RowsRead = result.rows;
                this.Errors = result.errors;
                this.PageNumber = result.page;
                this.PageSize = model.BatchSize;
                this.SqlServerRowsAfter = await sqlTask;
                this.ElasticsearchAfter = await esTask;

            }
        }

        public class MapResult
        {
            public dynamic Result { get; set; }
            public bool HasError { get; set; }
            public Exception Error { get; set; }
            public string ErrorId { get; set; }
        }
        private async Task<object> ImportDataAsync(DataTransferDefinition model, Stopwatch sw, IProgress<ProgressData> progress, int page = 1, int initialRows = 0, int initialErrors = 0)
        {
            var context = new SphDataContext();
            var adapter = await context.LoadOneAsync<Adapter>(x => x.Id == model.InboundAdapter);
            var mapping = await context.LoadOneAsync<TransformDefinition>(x => x.Id == model.InboundMap);

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

            var rows = initialRows;
            var errors = initialErrors;
            var headers = new Dictionary<string, object>
            {
                {"data-import", model.IgnoreMessaging},
                {"sql.retry", model.SqlRetry},
                {"sql.wait", model.SqlWait},
                {"es.retry", model.EsRetry},
                {"es.wait", model.EsWait}
            };
            var lo = await tableAdapter.LoadAsync(model.SelectStatement, page, model.BatchSize, false);
            while (lo.ItemCollection.Count > 0)
            {
                using (var session = context.OpenSession())
                {
                    var tasks = new List<Task<MapResult>>();
                    foreach (var source in lo.ItemCollection)
                    {
                        rows++;
                        var task = this.TransformAsync(map, source, model, progress);
                        tasks.Add(task);
                    }
                    var results = await Task.WhenAll(tasks);
                    var entities = results.Where(x => !x.HasError)
                            .Select(x => x.Result)
                            .Cast<Entity>()
                            .ToArray();
                    session.Attach(entities);

                    errors += results.Count(x => x.HasError);

                    await session.SubmitChanges("Import", headers);
                    if (model.DelayThrottle.HasValue)
                        await Task.Delay(model.DelayThrottle.Value);

                    var pd = new ProgressData(rows) { Elapsed = sw.Elapsed };
                    progress.Report(pd);
                }
                if (m_isCancelRequested)
                    return new { statusCode = 206, rows, errors, page = lo.CurrentPage, message = $"Stop requested after {rows} rows imported" };

                lo = await tableAdapter.LoadAsync(model.SelectStatement, lo.CurrentPage + 1, model.BatchSize, false);
            }
            return new
            {
                statusCode = 200,
                success = true,
                rows,
                errors,
                page = lo.CurrentPage,
                message = $"successfully imported {rows}",
                status = "OK"
            };
        }

        private async Task<MapResult> TransformAsync(dynamic map, dynamic source, DataTransferDefinition model, IProgress<ProgressData> progress)
        {
            var setting = new JsonSerializerSettings { Formatting = Formatting.Indented };
            var retryCount = 0;
            while (retryCount < 5)
            {
                try
                {
                    var dest0 = await map.TransformAsync(source);
                    return new MapResult { Result = dest0 };
                }
                catch (Exception ex)
                {
                    if (retryCount >= 4)
                    {
                        var log = new LogEntry(ex);
                        var id = $"{DateTime.Now:yyyyMMdd.HHmmss.fff}.{Guid.NewGuid().ToString().Substring(0, 4)}";
                        var errorId = $"{model.Name.ToIdFormat()}/{id}";
                        progress.Report(new ProgressData(ex, source, errorId));
                        var folder = $"{ConfigurationManager.WebPath}\\App_Data\\data-imports\\";
                        if (!System.IO.Directory.Exists(folder))
                            System.IO.Directory.CreateDirectory(folder);
                        System.IO.File.WriteAllText($"{folder}\\{errorId}.data", JsonConvert.SerializeObject(source, setting));
                        System.IO.File.WriteAllText($"{folder}\\{errorId}.error", log.ToString());
                        return new MapResult
                        {
                            Error = ex,
                            HasError = true,
                            ErrorId = errorId
                        };
                    }
                }
                retryCount++;
            }
            var dest = map.TransformAsync(source);
            return new MapResult { Result = dest };
        }

        private async Task UpdateImportStatusAsync(DataTransferDefinition model, Stopwatch sw, IProgress<ProgressData> progress)
        {
            var sqlMessages = 1;
            var esMessages = 1;
            var count = 10;
            // wait for 5 consecutives 0 queues for stoping
            var consecutiveEmptyMessages = 0;
            while (consecutiveEmptyMessages < 5 || count > 0)
            {
                await Task.Delay(2.Seconds());

                var sqlTask = m_rabbitManagementHttpClient.GetStringAsync($"api/queues/{ConfigurationManager.ApplicationName}/persistence");
                var esTask = m_rabbitManagementHttpClient.GetStringAsync($"api/queues/{ConfigurationManager.ApplicationName}/es.data-import");
                var sqlCountTask = GetSqlServerCountAsync(model);
                var esCountTask = GetElasticsearchCountAsync(model);
                await Task.WhenAll(sqlTask, esTask);
                var es = await esTask;
                var sql = await sqlTask;

                var pd = ProgressData.Parse(es, sql);
                pd.Elapsed = sw.Elapsed;
                pd.ElasticsearchRows = await esCountTask;
                pd.SqlRows = await sqlCountTask;
                progress.Report(pd);

                // increment consecutiveEmptyMessages, if the previous iteration also yields empty messages in the queue
                if (sqlMessages == 0 && esMessages == 0)
                    consecutiveEmptyMessages++;
                else
                    consecutiveEmptyMessages = 0;
                if (pd.ElasticsearchQueue.Rate > 0 || pd.SqlServerQueue.Rate > 0)
                    consecutiveEmptyMessages = 0;

                sqlMessages = pd.SqlServerQueue.MessagesCount;
                esMessages = pd.ElasticsearchQueue.MessagesCount;

                count--;
            }
        }

        private async Task<int> GetElasticsearchCountAsync(DataTransferDefinition model)
        {
            var json = await
                m_elasticsearchHttpClient.GetStringAsync(
                    $"{ConfigurationManager.ElasticSearchIndex}/{model.Entity.ToLowerInvariant()}/_count");
            var jo = JObject.Parse(json);
            return jo.SelectToken("$.count").Value<int>();
        }
        private async Task<int> GetSqlServerCountAsync(DataTransferDefinition model)
        {
            var sql = $"SELECT COUNT(*) FROM [{ConfigurationManager.ApplicationName}].[{model.Entity}]";
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                await conn.OpenAsync();
                var rows = await cmd.ExecuteScalarAsync();
                if (rows == DBNull.Value) return 0;
                return (int)rows;
            }
        }
        public async Task<object> Preview(DataTransferDefinition model)
        {
            try
            {
                var context = new SphDataContext();
                var adapter = await context.LoadOneAsync<Adapter>(x => x.Id == model.InboundAdapter);

                dynamic tableAdapter = GetTableAdapterInstance(model, adapter);
                if (null == tableAdapter) return new
                {
                    statusCode = 404,
                    message = $"{adapter.AssemblyName}.dll does not exist, you may have to build your adapter before it could be used"
                };

                var lo = await tableAdapter.LoadAsync(model.SelectStatement);
                return lo;
            }
            catch (Exception e)
            {
                return new LogEntry(e);
            }

        }

        private Type GetSourceType(DataTransferDefinition model, Adapter adapter)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            var dll = assemblies.SingleOrDefault(x => x.GetName().Name == adapter.AssemblyName);
            if (null == dll) return null;
            var sourceTypeName = $"{adapter.CodeNamespace}.{model.Table}";
            var sourceType = dll.GetType(sourceTypeName);
            return sourceType;
        }
        private object GetTableAdapterInstance(DataTransferDefinition model, Adapter adapter)
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