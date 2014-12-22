using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Code Expression", TypeName = "Expression", Description = "Custom code expression")]
    public partial class ExpressionActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            if (string.IsNullOrWhiteSpace(this.Expression))
            {
                result.Errors.Add(new BuildError(this.WebId, string.Format("[ExpressionActivity] -\"{0}\" Expression no code", this.Name)));
            }
            // TODO : validate it's a valid C# expression
            return result;
        }


        public override string GenerateExecMethodBody(WorkflowDefinition wd)
        {

            var code = new StringBuilder();
            
            code.AppendLine(this.ExecutingCode);
            code.AppendLine("       var result = new ActivityExecutionResult{ Status = ActivityExecutionStatus.Success};");
            code.AppendLine("       var item = this;");


            code.AppendLine(this.Expression);

            code.AppendLinf("       result.NextActivities = new[]{{\"{0}\"}};", this.NextActivityWebId);
            code.AppendLine(this.ExecutedCode);
            




            return code.ToString();
        }


        public override string GetEditorView()
        {
            return Properties.ActivityHtmlResources.activity_expression;
        }

        public override string GetEditorViewModel()
        {
            return Properties.ActivityJsResources.activity_expression;
        }


    }
}