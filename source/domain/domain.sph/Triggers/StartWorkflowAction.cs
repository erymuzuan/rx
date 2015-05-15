using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    [Export(typeof(CustomAction))]
    [DesignerMetadata(Name = "Workflow", TypeName = "Bespoke.Sph.Domain.StartWorkflowAction, domain.sph", Description = "Starts a new workflow when this action is executed", FontAwesomeIcon = "code-fork")]
    public partial class StartWorkflowAction : CustomAction
    {
        public override string GetEditorView()
        {
            return Properties.Resources.StartworkflowActionHtml;
        }

        public override string GetEditorViewModel()
        {
            return Properties.Resources.StartworkflowActionJs;
        }

        public override Bitmap GetPngIcon()
        {
            return Properties.Resources.Flow_Cart;
        }

        public async override Task ExecuteAsync(RuleContext ruleContext)
        {
            var context = new SphDataContext();
            var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.Id == this.WorkflowDefinitionId);
            var variables = from map in this.WorkflowTriggerMapCollection
                            select new VariableValue
                            {
                                Name = map.VariablePath,
                                Value = map.Field.GetValue(ruleContext)
                            };

            var wf = await wd.InitiateAsync(variables.ToArray());
            await wf.StartAsync().ConfigureAwait(false);
        }

        public override bool UseAsync => true;
    }
}