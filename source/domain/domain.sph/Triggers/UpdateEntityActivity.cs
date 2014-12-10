using System;
using System.ComponentModel.Composition;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Update a record", TypeName = "UpdateEntity", Description = "Update a record")]
    public partial class UpdateEntityActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            if (string.IsNullOrWhiteSpace(this.EntityIdPath))
                result.Errors.Add(new BuildError(this.WebId, string.Format("[UpdateEntityActivity] -\"{0}\" EntityIdPath is missing", this.Name)));
            if (string.IsNullOrWhiteSpace(this.EntityType))
                result.Errors.Add(new BuildError(this.WebId, string.Format("[UpdateEntityActivity] -\"{0}\" EntityType is missing", this.Name)));

            if (!string.IsNullOrWhiteSpace(this.EntityType))
            {
                var fullTypeName = this.EntityType;
                var type = Type.GetType(fullTypeName);
                if (null == type) result.Errors.Add(new BuildError(this.WebId, string.Format("[UpdateEntityActivity] -\"{0}\" Cannot load {1}", this.Name, this.EntityType)));

            }

            return result;
        }
        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            if (string.IsNullOrWhiteSpace(this.NextActivityWebId))
                throw new InvalidOperationException("NextActivityWebId is null or empty for " + this.Name);

            var fullTypeName = this.EntityType;
            var type = Type.GetType(fullTypeName);
            if (null == type)
                throw new InvalidOperationException("Cannot load " + this.EntityType);

            var code = new StringBuilder();
            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLine("       var context = new Bespoke.Sph.Domain.SphDataContext();");
            if (this.IsUsingVariable)
                code.AppendLinf("       var item = this.{0};", this.UseVariable);
            else
                code.AppendLinf("       var item = await context.LoadOneAsync<{0}>(e => e.{1}Id == {2});", type.FullName, type.Name, this.EntityIdPath);

            code.AppendLinf("if(item.{0}Id == 0)throw new InvalidOperationException(\"{0}Id is 0\");", type.Name);

            code.AppendLinf("       var self = this.WorkflowDefinition.ActivityCollection.OfType<UpdateEntityActivity>().Single(a => a.WebId == \"{0}\");", this.WebId);

            foreach (var mapping in this.PropertyMappingCollection)
            {
                code.AppendLinf("           item.{0} = this.{1};", mapping.Destination, mapping.Source);
                if (null != mapping as FunctoidMapping)
                    throw new Exception("Functoid mapping is not yet supported");
            }
            code.AppendLine("      using (var session = context.OpenSession())");
            code.AppendLine("      {");
            code.AppendLine("          session.Attach(item);");
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