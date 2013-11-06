using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class NotificationActivity : Activity
    {
        public override string GeneratedCustomTypeCode(WorkflowDefinition workflowDefinition)
        {
            return "";
        }

        public override Task<ActivityExecutionResult> ExecuteAsync()
        {
            return null;
        }
    }
}
