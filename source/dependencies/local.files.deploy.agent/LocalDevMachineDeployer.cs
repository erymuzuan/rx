using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Extensions;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.Deloyments
{
    [Export(typeof(IProjectDeployer))]
    public class LocalDevMachineDeployer : IProjectDeployer
    {
        public Task<bool> CheckForAsync(IProjectDefinition project)
        {
            throw new NotImplementedException();
        }

        public Task<RxCompilerResult> DeployAsync(IProjectDefinition project, Action<JObject, dynamic> migration, int batchSize = 50)
        {
            this.CopyFiles(project);
            return RxCompilerResult.TaskEmpty;
        }

        public Task<RxCompilerResult> TestDeployAsync(IProjectDefinition project, Action<JObject, dynamic> migration, int batchSize = 50)
        {

            return RxCompilerResult.TaskEmpty;
        }


        private void CopyFiles(IProjectDefinition ed)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            var output = $"{ConfigurationManager.ApplicationName}.{ed.Name}";
            var web = $@"{ConfigurationManager.WebPath}\bin";
            var subscribers = ConfigurationManager.SubscriberPath;
            try
            {
                var pe = $@"{ConfigurationManager.CompilerOutputPath}\{output}.dll";
                var pdb = $@"{ConfigurationManager.CompilerOutputPath}\{output}.pdb";

                File.Copy(pe, $@"{web}\{output}.dll", true);
                File.Copy(pdb, $@"{web}\{output}.pdb", true);
                logger.WriteVerbose($"Succesfully copied {pe} and {pdb} to {web}");

                File.Copy(pe, $@"{subscribers}\{output}.dll", true);
                File.Copy(pdb, $@"{subscribers}\{output}.pdb", true);
                logger.WriteVerbose($"Succesfully copied {pe} and {pdb} to {subscribers}");


            }
            catch (IOException ioe)
            {
                logger.WriteError("Fail to copy dll and pdb to web/bin and subscribers");
                logger.WriteError(ioe.Message);
            }
        }
    }
}
