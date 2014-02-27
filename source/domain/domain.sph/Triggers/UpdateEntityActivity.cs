using System;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class UpdateEntityActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            if (string.IsNullOrWhiteSpace(this.EntityIdPath))
            {
                result.Errors.Add(new BuildError(this.WebId, string.Format("[UpdateEntityActivity] -\"{0}\" EntityIdPath is missing", this.Name)));
            }

            var fullTypeName = this.EntityType;
            var type = Type.GetType(fullTypeName);
            if (null == type) result.Errors.Add(new BuildError(this.WebId, string.Format("[UpdateEntityActivity] -\"{0}\" Cannot load {1}", this.Name, this.EntityType)));

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
            code.AppendLinf("       var item = await context.LoadOneAsync<{0}>(e => e.{1}Id == {2});", type.FullName, type.Name, this.EntityIdPath);
            code.AppendLinf("       var self = this.WorkflowDefinition.ActivityCollection.OfType<CreateEntityActivity>().Single(a => a.WebId == \"{0}\");", this.WebId);

            var count = 1;
            foreach (var mapping in this.PropertyMappingCollection)
            {
                code.AppendLinf("       var functoid{1} =  self.PropertyMappingCollection.SingleOrDefault(m => m.WebId == \"{0}\") as FunctoidMapping;", mapping.WebId, count);
                code.AppendLinf("       if(null != functoid{0})", count);
                code.AppendLinf("           item.{0} = functoid{2}.Functoid.Convert<string,string>(this.{1});", mapping.Destination, mapping.Source, count);
                code.AppendLine("       else");
                code.AppendLinf("           item.{0} = this.{1};", mapping.Destination, mapping.Source);

                count++;
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