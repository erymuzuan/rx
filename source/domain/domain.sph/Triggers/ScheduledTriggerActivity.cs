using System;
using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Scheduled Trigger", TypeName = "ScheduledTrigger", Description = "A workflow could be start on schedule")]
    public partial class ScheduledTriggerActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result  =  base.ValidateBuild(wd);
            if(string.IsNullOrWhiteSpace(this.NextActivityWebId))
                result.Errors.Add(new BuildError(this.WebId,$"[ScheduledTriggerActivity] ->{this.Name} is missing NextActivityWebId"));

            if(this.IntervalScheduleCollection.Count == 0)
                result.Errors.Add(new BuildError(this.WebId,$"[ScheduledTriggerActivity] -> {this.Name} is missing triggers"));

            return result;
        }

        public override string GenerateExecMethodBody(WorkflowDefinition wd)
        {
            if (string.IsNullOrWhiteSpace(this.NextActivityWebId))
                throw new InvalidOperationException("NextActivityWebId is null or empty for " + this.Name);

            var code = new StringBuilder();

            code.AppendLine(this.ExecutingCode);
            code.AppendLine("       this.State = \"Ready\";");
            // set the next activity
            code.AppendLine("       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success };");
            code.AppendLinf("       result.NextActivities = new[]{{\"{0}\"}};", this.NextActivityWebId);

            code.AppendLine(this.ExecutedCode);


            return code.ToString();
        }
    }
}