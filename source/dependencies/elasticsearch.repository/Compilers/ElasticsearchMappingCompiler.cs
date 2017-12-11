using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Codes;
using Bespoke.Sph.Domain.Compilers;

namespace Bespoke.Sph.ElasticsearchRepository.Compilers
{
    [Export(typeof(IProjectBuilder))]
    public class ElasticsearchMappingCompiler : IProjectBuilder
    {
        public string Name => "Elasticsearch.1.7.5";
        public string Description => @"Compile EntityDefintion to Elasticsearch 1.7.5 mapping";
        public Task<IEnumerable<AttachedProperty>> GetDefaultAttachedPropertiesAsync(IProjectDefinition project)
        {
            return AttachedProperty.EmtptyListTask;
        }

        public Task<IEnumerable<AttachedProperty>> GetDefaultAttachedPropertiesAsync(Member member)
        {
            return AttachedProperty.EmtptyListTask;
        }


        public Task<IEnumerable<Class>> GenerateCodeAsync(IProjectDefinition project)
        {
            return Task.FromResult(Array.Empty<Class>().AsEnumerable());
        }

        public Task<RxCompilerResult> BuildAsync(IProjectDefinition project, Func<IProjectDefinition, CompilerOptions2> getOptions)
        {
            return RxCompilerResult.TaskEmpty;
        }

        public bool IsAvailableInDesignMode => true;
        public bool IsAvailableInBuildMode => true;
        public bool IsAvailableInDeployMode => false;
    }
}
