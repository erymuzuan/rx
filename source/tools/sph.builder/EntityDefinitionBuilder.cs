using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;

namespace Bespoke.Sph.SourceBuilders
{
    public class EntityDefinitionBuilder : Builder<EntityDefinition>
    {
        protected override async Task<RxCompilerResult> CompileAssetAsync(EntityDefinition item)
        {
            var results = new List<RxCompilerResult>();
            WriteDebug($"Found {this.DeveloperService.ProjectBuilders.Length} IProjectBuilders");
            foreach (var builder in this.DeveloperService.ProjectBuilders)
            {
                WriteDebug($"Building with {builder.GetType().Name}...");
                var sources = await builder.GenerateCodeAsync(item);
                foreach (var src in sources)
                {
                    File.WriteAllText($@"{ConfigurationManager.SphSourceDirectory}\EntityDefinition\{src.Key}", src.Value);
                }
                WriteDebug($"Savings source files {sources.Keys.ToString(", ")}");
                var cr = await builder.BuildAsync(item, sources.Keys.ToArray());
                results.Add(cr);
                WriteMessage($"{builder.GetType().Name} has {(cr.Result ? "successfully building" : "failed to build")} {item.Name}");
                WriteMessage(cr.ToString());
            }

            var final = new RxCompilerResult { Result = results.All(x => x.Result) };
            final.Errors.AddRange(results.SelectMany(x => x.Errors));

            return final;
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

        private static async Task<IEnumerable<RxCompilerResult>> CompileDependenciesAsync(EntityDefinition ed)
        {
            var results = new List<RxCompilerResult>();
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


        private static async Task<IEnumerable<RxCompilerResult>> CompileReceivePortAsync(ReceivePort port)
        {
            var results = new List<RxCompilerResult>();
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


    }
}