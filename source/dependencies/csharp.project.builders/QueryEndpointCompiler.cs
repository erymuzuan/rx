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
    public class QueryEndpointCompiler : IProjectBuilder
    {
        public async Task<Dictionary<string, string>> GenerateCodeAsync(IProjectDefinition project)
        {
            var sources = new Dictionary<string, string>();
            if (!(project is QueryEndpoint endpoint)) return sources;
            var ed = new SphDataContext().LoadOneFromSources<EntityDefinition>(x => x.Name == endpoint.Entity);

            var wrapper= new QueryEndpointCsharp(endpoint,ed);

            var controller = wrapper.GenerateCode();
            var info = await wrapper.GetAssemblyInfoCodeAsync();

            return new Dictionary<string, string>
            {
                {controller.FileName, controller.GetCode() },
                {info.FileName, info.Code }
            };

        }

        public async Task<RxCompilerResult> BuildAsync(IProjectDefinition project, string[] sources)
        {
            if (project is EntityDefinition schema)
            {
                var results = new List<RxCompilerResult>();
                var endpoints = new SphDataContext().LoadFromSources<QueryEndpoint>(x => x.Entity == schema.Name);
                foreach (var wrapper in endpoints.Select(x => new QueryEndpointCsharp(x, schema)))
                {
                    var srcs = await this.GenerateCodeAsync(wrapper.Endpoint);
                    var files = srcs.Keys.Select(x => new Class(srcs[x]) { FileName = x })
                        .Select(x => x.WriteSource(wrapper.Endpoint))
                        .ToArray();
                    var endpointCompilationResult = await wrapper.CompileAsync(files);
                    results.Add(endpointCompilationResult);
                }

                var rx = new RxCompilerResult { Result = results.TrueForAll(x => x.Result) };
                rx.Errors.AddRange(results.SelectMany(x => x.Errors));
                return rx;
            }

            if (project is QueryEndpoint endpoint)
            {
                var ed = new SphDataContext().LoadOneFromSources<EntityDefinition>(x => x.Name == endpoint.Entity);
                var wrapper = new QueryEndpointCsharp(endpoint,ed);
                return await wrapper.CompileAsync(sources);
            }
            return new RxCompilerResult();
        }

        public bool IsAvailableInDesignMode => false;
        public bool IsAvailableInBuildMode => true;
        public bool IsAvailableInDeployMode => false;
    }
}
