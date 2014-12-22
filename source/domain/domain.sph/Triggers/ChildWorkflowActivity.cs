using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Child Workflow", TypeName = "ChildWorkflow", Description = "Starts a child workflow")]
    public partial class ChildWorkflowActivity : Activity
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

        public override string GetEditorView()
        {
            return Properties.ActivityHtmlResources.activity_childworkflow;
        }

        public override string GetEditorViewModel()
        {
            return Properties.ActivityJsResources.activity_childworkflow;
        }
    }
}