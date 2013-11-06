using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class DecisionActivity : Activity
    {

        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            code.AppendLinf("   public Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       var result = new ActivityExecutionResult();");
            code.AppendLine("       return Task.FromResult(result);");
            code.AppendLine("   }");

            return code.ToString();
        }

        public override Task<ActivityExecutionResult> ExecuteAsync()
        {
            return null;
        }
    }
}