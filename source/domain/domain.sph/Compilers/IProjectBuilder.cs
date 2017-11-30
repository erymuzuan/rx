using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain.Compilers
{
    public interface IProjectBuilder
    {
        Task<IEnumerable<AttachProperty>> GetAttachPropertiesAsycn(IProjectDefinition project);
        Task<IEnumerable<AttachProperty>> GetAttachPropertiesAsycn(Member member);
        /*MetadataReference[] GetMetadataReferences();
        Task<IProjectDefinition> GetDefinitionAsync();*/
        Task<Dictionary<string, string>> GenerateCodeAsync(IProjectDefinition project);
        Task<RxCompilerResult> BuildAsync(IProjectDefinition project, string[] sources);
        bool IsAvailableInDesignMode { get; }
        bool IsAvailableInBuildMode { get; }
        bool IsAvailableInDeployMode { get; }

    }
}