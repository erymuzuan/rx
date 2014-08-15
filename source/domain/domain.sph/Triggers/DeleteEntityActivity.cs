using System;
using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Delete a record", TypeName = "DeleteEntity", Description = "Delete a record")]
    public partial class DeleteEntityActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            if (string.IsNullOrWhiteSpace(this.EntityIdPath))
            {
                result.Errors.Add(new BuildError(this.WebId,string.Format("[DeleteEntityActivity] -\"{0}\" EntityIdPath is missing", this.Name) ));
            }
            return result;
        }
        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            if (string.IsNullOrWhiteSpace(this.NextActivityWebId))
                throw new InvalidOperationException("NextActivityWebId is null or empty for " + this.Name);

            var code = new StringBuilder();
            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       var context = new Bespoke.Sph.Domain.SphDataContext();");
            code.AppendLinf("       var item = await context.LoadOneAsync<{0}>(e => e.{0}Id == {1});", this.EntityType, this.EntityIdPath);
            code.AppendLinf("       var self = this.WorkflowDefinition.ActivityCollection.OfType<CreateEntityActivity>().Single(a => a.WebId == \"{0}\");", this.WebId);
            
            code.AppendLine("      using (var session = context.OpenSession())");
            code.AppendLine("      {");
            code.AppendLine("          session.Delete(item);");
            code.AppendLine("          await session.SubmitChanges();");

            code.AppendLine("      }");
            // set the next activity
            code.AppendLine("       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            code.AppendLinf("       result.NextActivities = new[]{{\"{0}\"}};", this.NextActivityWebId);/* webid*/
            code.AppendLine("       return result;");
            code.AppendLine("   }");

            return code.ToString();
        }
    }
}