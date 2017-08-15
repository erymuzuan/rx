using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

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
            var operationEndpointFolder = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(OperationEndpoint)}";
            if (Directory.Exists(operationEndpointFolder))
            {
                foreach (var src in Directory.GetFiles(operationEndpointFolder, "*.json"))
                {
                    var oe = src.DeserializeFromJsonFile<OperationEndpoint>();
                    if (oe.Entity != ed.Name) continue;
                    var builder = new OperationEndpointBuilder();
                    await builder.RestoreAsync(oe);
                }

            }

            var queryEndpointFolder = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(QueryEndpoint)}";
            if (Directory.Exists(queryEndpointFolder))
            {
                foreach (var src in Directory.GetFiles(queryEndpointFolder, "*.json"))
                {
                    Console.WriteLine("QueryEndpoint " + Path.GetFileName(src));
                    var qe = src.DeserializeFromJsonFile<QueryEndpoint>();
                    if (qe.Entity != ed.Name) continue;
                    var builder = new QueryEndpointBuilder();
                    await builder.RestoreAsync(qe);
                }
            }

            var receivePortFolder = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(ReceivePort)}";
            if (Directory.Exists(receivePortFolder))
            {
                foreach (var src in Directory.GetFiles(receivePortFolder, "*.json"))
                {
                    var port = src.DeserializeFromJsonFile<ReceivePort>();
                    if (port.Entity != ed.Name) continue;
                    var builder = new ReceivePortBuilder();
                    await builder.RestoreAsync(port);

                    var receiveLocationFolder = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(ReceiveLocation)}";
                    if (!Directory.Exists(receiveLocationFolder)) continue;
                    foreach (var rsrc in Directory.GetFiles(receiveLocationFolder, "*.json"))
                    {
                        var loc = rsrc.DeserializeFromJsonFile<ReceiveLocation>();
                        if (loc.ReceivePort != port.Id) continue;
                        var locBuilder = new ReceiveLocationBuilder();
                        await locBuilder.RestoreAsync(loc);
                    }
                }

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
            options.AddReference(Path.GetFullPath(webDir + @"\bin\System.Web.Mvc.dll"));
            options.AddReference(Path.GetFullPath(webDir + @"\bin\core.sph.dll"));
            options.AddReference(Path.GetFullPath(webDir + @"\bin\Newtonsoft.Json.dll"));

            var codes = ed.GenerateCode();
            var sources = ed.SaveSources(codes);
            var result = ed.Compile(options, sources);
            this.ReportBuildStatus(result);
            
        }



    }
}