using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

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
        Task<bool> CheckForAsync(IProjectDefinition project);
        Task<RxCompilerResult> DeployAsync(IProjectDefinition project, Action<JObject, dynamic> migration, int batchSize = 50);
        // TODO : for test the client should be able to supply output stream -sort of like what if, or logger to display
        // what will happen if the action is executed
        Task<RxCompilerResult> TestDeployAsync(IProjectDefinition project, Action<JObject, dynamic> migration, int batchSize = 50);
    }
}