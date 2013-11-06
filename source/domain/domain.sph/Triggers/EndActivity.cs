using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class EndActivity : Activity
    {
        public override string GeneratedCustomTypeCode(WorkflowDefinition workflowDefinition)
        {
            return string.Empty;
        }

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
    }
}