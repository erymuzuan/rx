using System;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class EndActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            return new BuildValidationResult { Result = true };
        }

        public override string GeneratedCustomTypeCode(WorkflowDefinition workflowDefinition)
        {
            return string.Empty;
        }

        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       var result = new ActivityExecutionResult();");
            code.AppendLinf("       this.CurrentActivityWebId = null;");
            code.AppendLinf("       this.State = \"Completed\";");
            code.AppendLinf("       await this.SaveAsync(\"{0}\");", this.WebId);
            code.AppendLine("       return result;");
            code.AppendLine("   }");

            return code.ToString();
        }
    }
}