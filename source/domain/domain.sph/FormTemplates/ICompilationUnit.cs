using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Codes;
using Microsoft.CodeAnalysis;

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
        Task<IEnumerable<Class>> GenerateCodeAsync();
        Task<RxCompilerResult> BuildAsync(IProjectDefinition project);
        bool IsAvailableInDesignMode { get; }
        bool IsAvailableInBuildMode { get; }
        bool IsAvailableInDeployMode { get; }

    }

    public interface IProjectDeployer
    {
        Task<RxCompilerResult> DeployAsync(IProjectDefinition project);
    }
}