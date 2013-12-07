using System;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class ScheduledTriggerActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result  =  base.ValidateBuild(wd);
            if(string.IsNullOrWhiteSpace(this.NextActivityWebId))
                result.Errors.Add(new BuildError(this.WebId, string.Format("[ScheduledTriggerActivity] ->{0} is missing NextActivityWebId",this.Name)));

            if(this.IntervalScheduleCollection.Count == 0)
                result.Errors.Add(new BuildError(this.WebId, string.Format("[ScheduledTriggerActivity] -> {0} is missing triggers",this.Name)));

            return result;
        }

        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            if (string.IsNullOrWhiteSpace(this.NextActivityWebId))
                throw new InvalidOperationException("NextActivityWebId is null or empty for " + this.Name);

            var code = new StringBuilder();
            code.AppendLinf("   public Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine(this.BeforeExcuteCode);
            code.AppendLine("       this.State = \"Ready\";");
            // set the next activity
            code.AppendLine("       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success };");
            code.AppendLinf("       result.NextActivities = new[]{{\"{0}\"}};", this.NextActivityWebId);

            code.AppendLine(this.AfterExcuteCode);
            code.AppendLine("       return Task.FromResult(result);");
            code.AppendLine("   }");

            return code.ToString();
        }
    }
}