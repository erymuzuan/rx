using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class NotificationActivity : Activity
    {
        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {

            var code = new StringBuilder();
            code.AppendLinf("   public Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       var result = new ActivityExecutionResult();");
            code.AppendLinf("       System.Console.WriteLine(\"sending notification from {0}\");", this.From);
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
