using System.ComponentModel.Composition;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Mangements.Commands
{
    [Export(typeof(Command))]
    public class TestCommand : EntityDefinitionCommand
    {
        public override CommandParameter[] GetArgumentList()
        {
            return base.GetArgumentList().Concat(new[]
            {
                new CommandParameter("test", true, "whatif", "test"),
                new CommandParameter("output", true, "o", "out"),
                new CommandParameter("keep", false),
                new CommandParameter("plan", ()=>
                    Directory.GetFiles($"{ConfigurationManager.SphSourceDirectory}\\MigrationPlan", "*.json")
                    .Select(Path.GetFileNameWithoutExtension)
                    .ToArray())
            }).ToArray();
        }

        public override bool UseAsync => true;

        public override async Task ExecuteAsync()
        {
            var ed = this.GetEntityDefinition();
            var planName = this.GetCommandValue<string>("plan");
            var keep = this.GetCommandValue<bool>("keep");
            var output = Path.GetFullPath(this.GetCommandValue<string>("output"));
            var migrationPlan = $"{ConfigurationManager.SphSourceDirectory}\\MigrationPlan\\{planName}.json";

            WriteInfo($"Running migration simulation on {ed.Name} using {planName}");


            if (Directory.Exists(output))
                Directory.CreateDirectory(output);
            if (!keep)
            {
                var testFiles = Directory.GetFiles(output, "*.json");
                testFiles.ForEach(File.Delete);
                var migrationAssemblies = Directory.GetFiles(ConfigurationManager.CompilerOutputPath, "migration.*");
                migrationAssemblies.ForEach(File.Delete);
            }

            await DeploymentMetadata.InitializeAsync();
            var deployment = new DeploymentMetadata(ed);

            await deployment.TestMigrationAsync(migrationPlan, output);
            Process.Start("explorer.exe", output);

        }
    }
}