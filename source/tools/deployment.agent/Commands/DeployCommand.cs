using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Mangements.Commands
{
    [Export(typeof(Command))]
    public class DeployCommand : Command
    {
        public override CommandParameter[] GetArgumentList()
        {
            return new[]
            {
                new CommandParameter("e",true),
                new CommandParameter("deploy",true),
                new CommandParameter("nes",false, "NoElasticsearch"),
                new CommandParameter("truncate",false, "t"),
                new CommandParameter("batch-size", false, "size", "batch"),
                new CommandParameter("plan", true, "migration-plan"),
            };
        }

        public override bool UseAsync => true;

        public override async Task ExecuteAsync(EntityDefinition ed)
        {

            var migrationPlan = this.GetCommandValue<string>("plan");
            var nes = this.GetCommandValue<bool>("nes");
            var truncate = this.GetCommandValue<bool>("truncate");

            await DeploymentMetadata.InitializeAsync();
            var deployment = new DeploymentMetadata(ed);

            var batchSize = this.GetCommandValue<int?>("batch-size") ?? 1000;
            await deployment.BuildAsync(truncate, nes, batchSize, migrationPlan);
        }
    }
}