using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Sph.Csharp.CompilersServices.Extensions;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.Domain.Compilers;

namespace Bespoke.Sph.Csharp.CompilersServices
{
    [Export(typeof(IProjectBuilder))]
    public class OperationEndpointCompiler : IProjectBuilder
    {
        public string Name => "OperationEndpoint";
        public string Description => @"Compile EntityDefintion and all its OperationEndpoint or just one OperationEndpoint";
        public Task<IEnumerable<AttachedProperty>> GetDefaultAttachedPropertiesAsync(IProjectDefinition project)
        {
            return AttachedProperty.EmtptyListTask;
        }

        public Task<IEnumerable<AttachedProperty>> GetDefaultAttachedPropertiesAsync(Member member)
        {
            return AttachedProperty.EmtptyListTask;
        }


        public async Task<IEnumerable<Class>> GenerateCodeAsync(IProjectDefinition project)
        {
            var repos = ObjectBuilder.GetObject<ISourceRepository>();
            if (!(project is OperationEndpoint endpoint)) return Array.Empty<Class>();
            var ed = await repos.LoadOneAsync<EntityDefinition>(x => x.Name == endpoint.Entity);

            return await endpoint.GenerateSourceAsync(ed);

        }

        public async Task<RxCompilerResult> BuildAsync(IProjectDefinition project, Func<IProjectDefinition, CompilerOptions2> getOption)
        {
            switch (project)
            {
                //case EntityDefinition schema:
                //    var results = new List<RxCompilerResult>();
                //    var endpoints = new SphDataContext().LoadFromSources<OperationEndpoint>(x => x.Entity == schema.Name);
                //    foreach (var ep in endpoints)
                //    {
                //        var eo = getOption(ep);
                //        var endpointCompilationResult = await ep.CompileAsyncWithRoslynAsync(eo);
                //        results.Add(endpointCompilationResult);
                //    }

                //    var rx = new RxCompilerResult { Result = results.TrueForAll(x => x.Result) };
                //    rx.Errors.AddRange(results.SelectMany(x => x.Errors));
                //    return rx;
                case OperationEndpoint endpoint:
                    return await endpoint.CompileAsyncWithRoslynAsync(getOption(endpoint));
            }

            return RxCompilerResult.Empty;
        }

        public bool IsAvailableInDesignMode => false;
        public bool IsAvailableInBuildMode => true;
        public bool IsAvailableInDeployMode => false;
    }
}
