using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Domain
{

    public interface IProjectDefinitionWithMembers : IProjectDefinition
    {
        ObjectCollection<Member> MemberCollection { get; }
    }

    public interface IProjectDefinition
    {
        string CodeNamespace { get; }
        string Name { get; }
        string Id { get; set; }
    }

    public interface IProjectBuilder
    {
        /*MetadataReference[] GetMetadataReferences();
        Task<IProjectDefinition> GetDefinitionAsync();*/
        Task<Dictionary<string, string>> GenerateCodeAsync(IProjectDefinition project);
        Task<RxCompilerResult> BuildAsync(IProjectDefinition project, string[] sources);
        bool IsAvailableInDesignMode { get; }
        bool IsAvailableInBuildMode { get; }
        bool IsAvailableInDeployMode { get; }

    }

    public interface IProjectDeployer
    {
        Task<RxCompilerResult> DeployAsync(IProjectDefinition project);
    }
}