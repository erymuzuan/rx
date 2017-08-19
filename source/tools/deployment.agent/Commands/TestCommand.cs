using System.ComponentModel.Composition;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Mangements.Commands
{
    [Export(typeof(Command))]
    public class TestCommand : Command
    {
        public override CommandParameter[] GetArgumentList()
        {
            return new[]
            {
                new CommandParameter("test", true, "whatif", "test"),
                new CommandParameter("output", true),
                new CommandParameter("plan", true),
            };
        }

        public override bool UseAsync => true;

        public override async Task ExecuteAsync(EntityDefinition ed)
        {
            var migrationPlan = this.GetCommandValue<string>("output");

            await DeploymentMetadata.InitializeAsync();
            var deployment = new DeploymentMetadata(ed);

            //clean
            var migrationAssemblies = Directory.GetFiles(ConfigurationManager.CompilerOutputPath, "migration.*");
            migrationAssemblies.ForEach(File.Delete);
            
            var outputFolder = this.GetCommandValue<string>("output");
            await deployment.TestMigrationAsync(migrationPlan, outputFolder);

        }
    }
}