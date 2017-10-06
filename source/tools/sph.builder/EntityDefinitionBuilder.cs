using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;

namespace Bespoke.Sph.SourceBuilders
{
    public class EntityDefinitionBuilder : Builder<EntityDefinition>
    {

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

        private async Task CompileDependenciesAsync(EntityDefinition ed)
        {
            await ed.ServiceContract.CompileAsync(ed);
            var context = new SphDataContext();

            // NOTE : it may be tempting to use Task.WhenAll, but we should compile them sequentially
            var operationEndpoints = context.LoadFromSources<OperationEndpoint>().Where(x => x.Entity == ed.Name);
            foreach (var oe in operationEndpoints)
            {
                var builder = new OperationEndpointBuilder();
                await builder.RestoreAsync(oe);
            }

            var queryEndpoints = context.LoadFromSources<QueryEndpoint>().Where(x => x.Entity == ed.Name);
            foreach (var qe in queryEndpoints)
            {
                var builder = new QueryEndpointBuilder();
                await builder.RestoreAsync(qe);
            }

            var ports = context.LoadFromSources<ReceivePort>().Where(x => x.Entity == ed.Name);
            foreach (var p in ports)
            {
                await CompileReceivePortAsync(p);
            }

        }

        private static async Task CompileReceivePortAsync(ReceivePort port)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            var context = new SphDataContext();
            var builder = new ReceivePortBuilder();
            await builder.RestoreAsync(port);

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
                await locBuilder.RestoreAsync(loc);
            }

        }


        public override async Task RestoreAsync(EntityDefinition ed)
        {
            CompileEntityDefinition(ed);
            await CompileDependenciesAsync(ed);
        }


        private void CompileEntityDefinition(EntityDefinition ed)
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
            var result = ed.Compile(options, sources);
            this.ReportBuildStatus(result);

        }



    }
}