using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;
using Bespoke.Sph.SqlRepository;
using Bespokse.Sph.ElasticsearchRepository;
using Newtonsoft.Json;
using Polly;

namespace Bespoke.Sph.SourceBuilders
{
    public class EntityDefinitionBuilder : Builder<EntityDefinition>
    {
        private readonly HttpClient m_elasticsearchHttpClient;

        public EntityDefinitionBuilder()
        {
            m_elasticsearchHttpClient = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) };
        }

        protected override async Task<WorkflowCompilerResult> CompileAssetAsync(EntityDefinition item)
        {
            var cr = CompileEntityDefinition(item);
            var cr1 = (await CompileDependenciesAsync(item)).ToList();
            var result = new WorkflowCompilerResult { Result = cr.Result && cr1.All(x => x.Result) };
            result.Errors.AddRange(cr.Errors);
            result.Errors.AddRange(cr1.SelectMany(x => x.Errors));
            result.Output = cr.Output + "\r\n" + cr1.ToString("\r\n", x => x.Output);

            var options = item.GetPersistenceOption();
            if (options.IsSource)
            {
                await RebuildDataFromSourceAsync(item);
            }

            return result;
        }

        public override async Task RestoreAllAsync()
        {
            var folder = ConfigurationManager.SphSourceDirectory + @"\EntityDefinition";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.DeleteAsync(ConfigurationManager.ApplicationName);
                Console.WriteLine("DELETE {1} index : {0}", response.StatusCode, ConfigurationManager.ApplicationName);
                await client.PutAsync(ConfigurationManager.ApplicationName, new StringContent(""));

            }
            this.Initialize();
            this.Clean();
            Console.WriteLine("Reading from " + folder);

            var tasks = from f in Directory.GetFiles(folder, "*.json")
                        let ed = f.DeserializeFromJsonFile<EntityDefinition>()
                        select this.RestoreAsync(ed);

            await Task.WhenAll(tasks);

            Console.WriteLine("Done Custom Entities");

        }

        private async Task<IEnumerable<WorkflowCompilerResult>> CompileDependenciesAsync(EntityDefinition ed)
        {
            var results = new List<WorkflowCompilerResult>();
            await ed.ServiceContract.CompileAsync(ed);
            var context = new SphDataContext();

            // NOTE : it may be tempting to use Task.WhenAll, but we should compile them sequentially
            var operationEndpoints = context.LoadFromSources<OperationEndpoint>().Where(x => x.Entity == ed.Name);
            foreach (var oe in operationEndpoints)
            {
                var builder = new OperationEndpointBuilder();
                var cr = await builder.RestoreAsync(oe);
                results.Add(cr);
            }

            var queryEndpoints = context.LoadFromSources<QueryEndpoint>().Where(x => x.Entity == ed.Name);
            foreach (var qe in queryEndpoints)
            {
                var builder = new QueryEndpointBuilder();
                var cr = await builder.RestoreAsync(qe);
                results.Add(cr);
            }

            var ports = context.LoadFromSources<ReceivePort>().Where(x => x.Entity == ed.Name);
            foreach (var p in ports)
            {
                var portResults = await CompileReceivePortAsync(p);
                results.AddRange(portResults);
            }

            return results;

        }


        private async Task RebuildDataFromSourceAsync(EntityDefinition ed)
        {
            if (ed.Transient) return;
            if (!ed.GetPersistenceOption().IsSource) return;

            await RebuildSqlTable(ed);
            await RebuildElasticsearchMapping(ed);


            var sqlDataBuilder = new Builder { EntityDefinition = ed, Name = ed.Name };
            sqlDataBuilder.Initialize();

            var taskBuckets = new List<Task>();
            var files = Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\{ed.Name}", "*.json");
            foreach (var file in files)
            {
                var json = File.ReadAllText(file);
                var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
                dynamic ent = JsonConvert.DeserializeObject(json, setting);
                ent.Id = Path.GetFileNameWithoutExtension(file);

                var esTask = IndexItemToElasticSearchAsync(ent);
                var sqlTask = sqlDataBuilder.InsertAsync(ent);
                taskBuckets.Add(esTask);
                taskBuckets.Add(sqlTask);

                if (taskBuckets.Count > 10)
                {
                    await Task.WhenAll(taskBuckets);
                    taskBuckets.Clear();
                }
            }
            if (taskBuckets.Any())
                await Task.WhenAll(taskBuckets);

        }

        private async Task RebuildSqlTable(EntityDefinition ed)
        {
            WriteMessage($"Droping SQL table [{ConfigurationManager.ApplicationName}].[{ed.Name}]...");
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            using (var dropTableCommand = new SqlCommand($"DROP TABLE [{ConfigurationManager.ApplicationName}].[{ed.Name}]", conn) { CommandType = CommandType.Text })
            {
                await conn.OpenAsync();
                await dropTableCommand.ExecuteNonQueryAsync();
                WriteMessage($"Droping SQL table [{ConfigurationManager.ApplicationName}].[{ed.Name}]...");
            }
            using (var conn = new SqlConnection(ConfigurationManager.SqlConnectionString))
            {
                var createTableSql = File.ReadAllText($@"{ConfigurationManager.SphSourceDirectory}\{nameof(EntityDefinition)}\{ed.Name}.sql");
                using (var createTableCommand = new SqlCommand(createTableSql, conn) { CommandType = CommandType.Text })
                {
                    await conn.OpenAsync();
                    WriteMessage($"Executing  create SQL table [{ConfigurationManager.ApplicationName}].[{ed.Name}]...");
                    await createTableCommand.ExecuteNonQueryAsync();
                }
            }
        }

        private async Task RebuildElasticsearchMapping(EntityDefinition item)
        {
            WriteMessage($"Rebuilding {item.Name} elasticsearch mapping...");
            var builder = new MappingBuilder();
            await builder.DeleteMappingAsync(item);
            await builder.PutMappingAsync(item);
            WriteMessage($"Rebuilt {item.Name} elasticsearch mapping from source");

        }

        private async Task IndexItemToElasticSearchAsync(Entity item)
        {
            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(item, setting);

            var content = new StringContent(json);
            var id = item.Id;
            if (item.GetType().Namespace == typeof(Entity).Namespace) return;// just custom entity


            var name = item.GetType().Name.ToLowerInvariant();
            var index = ConfigurationManager.ApplicationName.ToLowerInvariant();
            var url = $"{index}/{name}/{id}";

            var c = m_elasticsearchHttpClient;
            var pr = await Policy.Handle<HttpRequestException>()
                .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(Math.Pow(2, x) * 500))
                .ExecuteAndCaptureAsync(async () => await c.PostAsync(url, content));
            if (null != pr.FinalException)
                pr.Result.EnsureSuccessStatusCode();

            WriteMessage($"Indexed {name}({id}) ..");


        }


        private static async Task<IEnumerable<WorkflowCompilerResult>> CompileReceivePortAsync(ReceivePort port)
        {
            var results = new List<WorkflowCompilerResult>();
            var logger = ObjectBuilder.GetObject<ILogger>();
            var context = new SphDataContext();
            var builder = new ReceivePortBuilder();

            var portResult = await builder.RestoreAsync(port);
            results.Add(portResult);

            var locations = context.LoadFromSources<ReceiveLocation>().Where(x => x.ReceivePort == port.Id);
            foreach (var loc in locations)
            {
                var vr = await loc.ValidateBuildAsync();
                if (!vr.Result)
                {
                    logger.WriteWarning($"==== [ReceiveLocation] Unable to compile {loc.Id} ===== \r\n{vr}");
                    continue;
                }

                var locBuilder = new ReceiveLocationBuilder();
                var cr = await locBuilder.RestoreAsync(loc);
                results.Add(cr);
            }
            return results;

        }



        private WorkflowCompilerResult CompileEntityDefinition(EntityDefinition ed)
        {
            var options = new CompilerOptions
            {
                IsVerbose = false,
                IsDebug = true
            };
            var webDir = ConfigurationManager.WebPath;
            options.AddReference(Path.GetFullPath($@"{webDir}\bin\System.Web.Mvc.dll"));
            options.AddReference(Path.GetFullPath($@"{webDir}\bin\core.sph.dll"));
            options.AddReference(Path.GetFullPath($@"{webDir}\bin\Newtonsoft.Json.dll"));

            var codes = ed.GenerateCode();
            var sources = ed.SaveSources(codes);
            return ed.Compile(options, sources);

        }



    }
}