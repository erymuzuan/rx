using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Csharp.CompilersServices.Extensions;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Csharp.CompilersServices
{
    [Export(typeof(IProjectBuilder))]
    public class OperationEndpointCompiler : IProjectBuilder
    {
        public async Task<Dictionary<string, string>> GenerateCodeAsync(IProjectDefinition project)
        {
            var sources = new Dictionary<string, string>();
            if (!(project is OperationEndpoint endpoint)) return sources;
            var ed = new SphDataContext().LoadOneFromSources<EntityDefinition>(x => x.Name == endpoint.Entity);

            return await endpoint.GenerateSourceAsync(ed);

        }
        
        public async Task<RxCompilerResult> BuildAsync(IProjectDefinition project, string[] sources)
        {
            if (project is EntityDefinition schema)
            {
                var results = new List<RxCompilerResult>();
                var endpoints = new SphDataContext().LoadFromSources<OperationEndpoint>(x => x.Entity == schema.Name);
                foreach (var ep in endpoints)
                {
                    var src = await ep.GenerateSourceAsync(schema);
                    var files = src.Keys.Select(x => new Class(src[x]){FileName = x})
                        .Select(x => x.WriteSource(ep))
                        .ToArray();
                    var endpointCompilationResult = await ep.CompileAsync(schema, files[0], files[1]);
                    results.Add(endpointCompilationResult);
                }

                var rx = new RxCompilerResult { Result = results.TrueForAll(x => x.Result) };
                rx.Errors.AddRange(results.SelectMany(x => x.Errors));
                return rx;
            }

            if (project is OperationEndpoint endpoint)
            {
                var ed = new SphDataContext().LoadOneFromSources<EntityDefinition>(x => x.Name == endpoint.Entity);
                return await endpoint.CompileAsync(ed, sources[0], sources[1]);
            }
            return new RxCompilerResult();
        }

        public bool IsAvailableInDesignMode => false;
        public bool IsAvailableInBuildMode => true;
        public bool IsAvailableInDeployMode => false;
    }
}
