using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class StartWorkflowAction : CustomAction
    {
        public async override Task ExecuteAsync(RuleContext ruleContext)
        {
            var context = new SphDataContext();
            var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.WorkflowDefinitionId == this.WorkflowDefinitionId);
            var variables = from map in this.WorkflowTriggerMapCollection
                            select new VariableValue
                            {
                                Name = map.VariablePath,
                                Value = map.Field.GetValue(ruleContext)
                            };

            var wf = await wd.InitiateAsync(variables.ToArray());
            await wf.StartAsync();
        }

        public override bool UseAsync
        {
            get { return true; }
        }
    }
}