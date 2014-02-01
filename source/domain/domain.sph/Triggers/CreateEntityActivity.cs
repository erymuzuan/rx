using System;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class CreateEntityActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            return new BuildValidationResult { Result = true };
        }
        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            if (string.IsNullOrWhiteSpace(this.NextActivityWebId))
                throw new InvalidOperationException("NextActivityWebId is null or empty for " + this.Name);
            var context = new SphDataContext();
            var ed = context.LoadOne<EntityDefinition>(d => d.Name == this.EntityType);
            var entityFullName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName,
                ed.EntityDefinitionId, ed.Name);

            var code = new StringBuilder();
            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLinf("        var item = new {0}();", entityFullName);
            code.AppendLinf("        var self = this.WorkflowDefinition.ActivityCollection.OfType<CreateEntityActivity>().Single(a => a.WebId == \"{0}\");", this.WebId);

            foreach (var mapping in this.PropertyMappingCollection)
            {
                code.AppendLinf("           item.{0} = this.{1};", mapping.Destination, mapping.Source);

            }
            code.AppendLine("      var context = new Bespoke.Sph.Domain.SphDataContext();");
            code.AppendLine("      using (var session = context.OpenSession())");
            code.AppendLine("      {");
            code.AppendLine("          session.Attach(item);");
            code.AppendLine("          await session.SubmitChanges();");
            if (!string.IsNullOrWhiteSpace(this.ReturnValuePath))
                code.AppendLinf("          this.{0} = item.{1}Id;", this.ReturnValuePath, this.EntityType);
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