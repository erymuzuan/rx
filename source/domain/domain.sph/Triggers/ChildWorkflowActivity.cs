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

        public override string GenerateInitAsyncMethod(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            code.AppendLine($"   public async Task<InitiateActivityResult> InitiateAsync{MethodName}()");
            code.AppendLine("   {");
            code.AppendLine($"       var self = this.GetActivity<ChildWorkflowActivity>(\"{WebId}\");");


            code.Append($@"            
            var context = new SphDataContext();
            var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.Id == ""{WorkflowDefinitionId}"");
            var variables = new List<VariableValue>();");
            foreach (var map in this.PropertyMappingCollection)
            {
                code.AppendLine($"variables.Add(new VariableValue{{ Name = {map.Destination}, Value = this.{map.Source} }});");
            }
            code.AppendLine($@"
            var wf = await wd.InitiateAsync(variables.ToArray());
            await wf.StartAsync(this.Id, ""{WebId}"").ConfigureAwait(false);");

            code.AppendLine("       var result = new InitiateActivityResult{ Correlation = Guid.NewGuid().ToString() };");
            code.AppendLine("       return result;");
            code.AppendLine("   }");

            return code.ToString();
        }


        public override string GenerateExecMethodBody(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            if (this.IsAsync)
            {

                code.AppendLine(this.ExecutingCode);
                code.AppendLine(@"       this.State = ""Ready"";");
                code.AppendLine("       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
                code.AppendLine($@"       result.NextActivities = new[]{{""{NextActivityWebId}""}};");
                code.AppendLine(this.ExecutedCode);

                return code.ToString();

            }
            code.AppendLine($@"            
            var context = new SphDataContext();
            var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.Id == ""{WorkflowDefinitionId}"");
            var variables = new System.Collections.Generic.List<VariableValue>();");
            foreach (var map in this.PropertyMappingCollection)
            {
                code.AppendLine($@"variables.Add(new VariableValue{{ Name = ""{map.Destination}"", Value = this.{map.Source} }});");
            }
            code.AppendLine(@"
            var wf = await wd.InitiateAsync(variables.ToArray());
            await wf.StartAsync().ConfigureAwait(false);");

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