using System.Text;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class NotificationActivity : Activity
    {
        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {

            var code = new StringBuilder();
            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       var result = new ActivityExecutionResult();");
            code.AppendLinf("       this.CurrentActivityWebId = \"{0}\";", this.NextActivityWebId);
            code.AppendLinf("       System.Console.WriteLine(\"Sending email : {0}\");", this.Name);
            code.AppendLinf("       await this.SaveAsync(\"{0}\");", this.WebId);
            code.AppendLine("       return result;");
            code.AppendLine("   }");

            return code.ToString();
        }

        public override Task<ActivityExecutionResult> ExecuteAsync()
        {
            return null;
        }
    }
}
