﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Extensions;

namespace Bespoke.Sph.SourceBuilders
{
    public class EntityDefinitionBuilder : Builder<EntityDefinition>
    {
        protected override async Task<RxCompilerResult> CompileAssetAsync(EntityDefinition item)
        {
            return await CompileAsync(item);
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
            var cvs = ObjectBuilder.GetObject<ISourceRepository>();

            // TODO : Csharp compiler for ReceivePort
            var ports = await cvs.LoadAsync<ReceivePort>(x => x.Entity == ed.Name);
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
            var builder = new ReceivePortBuilder();
            var cvs = ObjectBuilder.GetObject<ISourceRepository>();

            var portResult = await builder.RestoreAsync(port);
            results.Add(portResult);

            var locations = await cvs.LoadAsync<ReceiveLocation>(x => x.ReceivePort == port.Id);
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