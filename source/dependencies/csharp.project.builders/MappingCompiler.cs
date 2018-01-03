using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Csharp.CompilersServices.Extensions;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.Domain.Compilers;

namespace Bespoke.Sph.Csharp.CompilersServices
{
    [Export(typeof(IProjectBuilder))]
    public class MappingCompiler : IProjectBuilder
    {
        public string Name => "Roslyn mapping compiler";
        public string Description => "Use Microsoft.Net compiler services to compile Transform Definition";
        public Task<IEnumerable<AttachedProperty>> GetDefaultAttachedPropertiesAsync(IProjectDefinition project)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<AttachedProperty>> GetDefaultAttachedPropertiesAsync(Member member)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Class>> GenerateCodeAsync(IProjectDefinition project)
        {
            var sources = new List<Class>();
            if (!(project is TransformDefinition map)) return Task.FromResult(sources.AsEnumerable());
            var classes = map.GenerateCode().Select(x => new Class(x.Value){Name =  x.Key});

            sources.AddRange(classes.Distinct(new ClassComparer()));

            return Task.FromResult(sources.AsEnumerable());
        }

        public async Task<RxCompilerResult> BuildAsync(IProjectDefinition project, Func<IProjectDefinition, CompilerOptions2> getOption)
        {
            if (!(project is TransformDefinition map)) return RxCompilerResult.Empty;
            
            var result = await map.CompileAsyncWithRoslynAsync(getOption(map));
            return result;
        }

        public bool IsAvailableInDesignMode => false;
        public bool IsAvailableInBuildMode => true;
        public bool IsAvailableInDeployMode => false;
    }
}