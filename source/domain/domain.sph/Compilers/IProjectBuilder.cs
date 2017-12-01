using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain.Compilers
{
    public interface IProjectBuilder
    {
        string Name { get; }
        string Description { get; }
        Task<IEnumerable<AttachedProperty>> GetAttachPropertiesAsycn(IProjectDefinition project);
        Task<IEnumerable<AttachedProperty>> GetAttachPropertiesAsycn(Member member);
        /*MetadataReference[] GetMetadataReferences();
        Task<IProjectDefinition> GetDefinitionAsync();*/
        Task<IEnumerable<Class>> GenerateCodeAsync(IProjectDefinition project);
        Task<RxCompilerResult> BuildAsync(IProjectDefinition project, Func<IProjectDefinition, CompilerOptions2> getOption);
        bool IsAvailableInDesignMode { get; }
        bool IsAvailableInBuildMode { get; }
        bool IsAvailableInDeployMode { get; }

    }
}