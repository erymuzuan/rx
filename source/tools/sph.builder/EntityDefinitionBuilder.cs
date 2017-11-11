using System;
using System.Collections.Generic;
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

        protected override async Task<WorkflowCompilerResult> CompileAssetAsync(EntityDefinition item)
        {
            var cr = CompileEntityDefinition(item);
            var cr1 = (await CompileDependenciesAsync(item)).ToList();
            var result = new WorkflowCompilerResult { Result = cr.Result && cr1.All(x => x.Result) };
            result.Errors.AddRange(cr.Errors);
            result.Errors.AddRange(cr1.SelectMany(x => x.Errors));
            result.Output = cr.Output + "\r\n" + cr1.ToString("\r\n", x => x.Output);

            return result;
        }

        public override async Task RestoreAllAsync()
        {
            var folder = ConfigurationManager.SphSourceDirectory + @"\EntityDefinition";
            var repos = ObjectBuilder.GetObject<IReadOnlyRepository>();
            await repos.CleanAsync();
            this.Initialize();
            this.Clean();
            Console.WriteLine("Reading from " + folder);

            var tasks = from f in Directory.GetFiles(folder, "*.json")
                        let ed = f.DeserializeFromJsonFile<EntityDefinition>()
                        select this.RestoreAsync(ed);

            await Task.WhenAll(tasks);

            WriteMessage("Done Custom Entities");

        }

        private static async Task<IEnumerable<WorkflowCompilerResult>> CompileDependenciesAsync(EntityDefinition ed)
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