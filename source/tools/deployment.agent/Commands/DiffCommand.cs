using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Colorful;

namespace Bespoke.Sph.Mangements.Commands
{
    [Export(typeof(Command))]
    public class DiffCommand : EntityDefinitionCommand
    {
        public override CommandParameter[] GetArgumentList()
        {
            return base.GetArgumentList().Concat(new[]
            {
                new CommandParameter("diff",true, "change", "changes")
            }).ToArray();
        }

        public override bool UseAsync => true;

        public override async Task ExecuteAsync()
        {
            var ed = this.GetEntityDefinition();
            await DeploymentMetadata.InitializeAsync();
            var deployment = new DeploymentMetadata(ed);

            var plan = await deployment.GetChangesAsync();
            foreach (var change in plan.ChangeCollection.OrderBy(x => x.OldPath).Where(x => !x.IsEmpty))
            {
                Console.WriteLine("______________________________________________________");
                Console.WriteLine(change);
            }
            var migrationPlanFile = $"{ed.Name}-{plan.PreviousCommitId}-{plan.CurrentCommitId}";
            Console.WriteLine($"MigrationPlan is saved to {migrationPlanFile}", Color.Yellow);
            File.WriteAllText($@"{ConfigurationManager.SphSourceDirectory}\MigrationPlan\{migrationPlanFile}.json", plan.ToJson());
        }
    }
}