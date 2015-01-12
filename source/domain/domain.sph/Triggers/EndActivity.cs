using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Stop the workflow", TypeName = "End", Description = "Stop the workflow")]
    public partial class EndActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            return new BuildValidationResult { Result = true };
        }


        public override string GenerateExecMethodBody(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            code.AppendLine("       var result = new ActivityExecutionResult{  Status = ActivityExecutionStatus.Success };");
            code.AppendLine("       result.NextActivities = new string[]{};");
            code.AppendLinf("       this.State = \"Completed\";");

            return code.ToString();
        }
    }
}