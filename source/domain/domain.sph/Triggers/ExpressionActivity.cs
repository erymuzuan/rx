﻿using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class ExpressionActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            if (string.IsNullOrWhiteSpace(this.Expression))
            {
                result.Errors.Add(new BuildError(this.WebId,string.Format("[ExpressionActivity] -\"{0}\" Expression no code", this.Name)));
            }
            // TODO : validate it's a valid C# expression
            return result;
        }


        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {

            var code = new StringBuilder();
            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       await Task.Delay(50);");
            code.AppendLine("       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};");
            code.AppendLine("       var item = this;");


            code.AppendLine("       " + this.Expression);

            code.AppendLinf("       result.NextActivities = new[]{{\"{0}\"}};", this.NextActivityWebId);
            code.AppendLine("       return result;");
            code.AppendLine("   }");// end metod




            return code.ToString();
        }

    }
}