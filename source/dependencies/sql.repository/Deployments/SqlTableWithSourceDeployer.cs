using System;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.SqlRepository.Deployments
{
    [Export(typeof(IProjectDeployer))]
    public class SqlTableWithSourceDeployer : SqlTableTool, IProjectDeployer
    {
        public SqlTableWithSourceDeployer() : base(null)
        {
        }

        public SqlTableWithSourceDeployer(Action<string> writeMessage, Action<string> writeWarning = null,
            Action<Exception> writeError = null) : base(writeMessage, writeWarning, writeError)
        {
        }

        public async Task<bool> CheckForAsync(IProjectDefinition project)
        {
            await Task.Delay(100);
            if (!(project is EntityDefinition ed)) return false;
            return !ed.Transient && ed.TreatDataAsSource;
        }

        public Task<RxCompilerResult> DeployAsync(IProjectDefinition project, Action<JObject, dynamic> migration, int batchSize = 50)
        {
            return RxCompilerResult.TaskEmpty;
        }

        public Task<RxCompilerResult> TestDeployAsync(IProjectDefinition project, Action<JObject, dynamic> migration, int batchSize = 50)
        {
            return RxCompilerResult.TaskEmpty;
        }
    }
}