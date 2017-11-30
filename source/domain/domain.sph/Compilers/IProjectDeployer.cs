using System;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Domain.Compilers
{
    public interface IProjectDeployer
    {
        Task<bool> CheckForAsync(IProjectDefinition project);
        Task<RxCompilerResult> DeployAsync(IProjectDefinition project, Action<JObject, dynamic> migration, int batchSize = 50);
        // TODO : for test the client should be able to supply output stream -sort of like what if, or logger to display
        // what will happen if the action is executed
        Task<RxCompilerResult> TestDeployAsync(IProjectDefinition project, Action<JObject, dynamic> migration, int batchSize = 50);
    }
}