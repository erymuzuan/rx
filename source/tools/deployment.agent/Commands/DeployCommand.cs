using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Console = Colorful.Console;

namespace Bespoke.Sph.Mangements.Commands
{
    [Export(typeof(Command))]
    public class DeployCommand : EntityDefinitionCommand
    {
        public override CommandParameter[] GetArgumentList()
        {
            return base.GetArgumentList().Concat(new[]
            {
                new CommandParameter("deploy",true),
                new CommandParameter("nes",false, "NoElasticsearch"),
                new CommandParameter("truncate",false, "t"),
                new CommandParameter("batch-size", false, "size", "batch"),
                new CommandParameter("plan", true, "migration-plan"),
            }).ToArray();
        }

        public override bool UseAsync => true;
        public override async Task ExecuteAsync()
        {
            var ed = this.GetEntityDefinition();
            if (null == ed)
            {
                Console.WriteLine("Cannot find EntityDefinition");
                return;
            }
            var migrationPlan = this.GetCommandValue<string>("plan");
            var nes = this.GetCommandValue<bool>("nes");
            var truncate = this.GetCommandValue<bool>("truncate");

            await DeploymentMetadata.InitializeAsync();
            var deployment = new DeploymentMetadata(ed);

            var batchSize = this.GetCommandValue<int?>("batch-size") ?? 1000;
            await deployment.BuildAsync(truncate, nes, batchSize, migrationPlan);

            var output = $"{ConfigurationManager.ApplicationName}.{ed.Name}";
            var web = $@"{ConfigurationManager.WebPath}\bin";
            var subscribers = ConfigurationManager.SubscriberPath;
            try
            {
                File.Copy($@"{ConfigurationManager.CompilerOutputPath}\{output}.dll", $@"{web}\{output}.dll", true);
                File.Copy($@"{ConfigurationManager.CompilerOutputPath}\{output}.pdb", $@"{web}\{output}.pdb", true);

                File.Copy($@"{ConfigurationManager.CompilerOutputPath}\{output}.dll", $@"{subscribers}\{output}.dll", true);
                File.Copy($@"{ConfigurationManager.CompilerOutputPath}\{output}.pdb", $@"{subscribers}\{output}.pdb", true);


            }
            catch (IOException ioe)
            {
                WriteError("Fail to copy dll and pdb to web/bin and subscribers");
                WriteError(ioe.Message);
            }
        }


    }
}