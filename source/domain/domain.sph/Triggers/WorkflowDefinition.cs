using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class WorkflowDefinition : Entity
    {
        public Task<Workflow> InitiateAsync(IEnumerable<Variable> values = null, ScreenActivity screen = null)
        {
            var wf = new Workflow
            {
                Name = this.Name,
                WorkflowDefinitionId = this.WorkflowDefinitionId,
                State = "Active"

            };
            if (null != screen)
            {
                wf.VariableCollection.ClearAndAddRange(values);
            }
            return Task.FromResult(wf);
        }

        public ScreenActivity GetInititorScreen()
        {
            return this.ActivityCollection.Single(p => p.IsInitiator) as ScreenActivity;
        }
    }
}