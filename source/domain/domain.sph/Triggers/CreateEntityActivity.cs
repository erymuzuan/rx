using System;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class CreateEntityActivity : Activity
    {
        public override string GeneratedExecutionMethodCode(WorkflowDefinition wd)
        {
            if (string.IsNullOrWhiteSpace(this.NextActivityWebId))
                throw new InvalidOperationException("NextActivityWebId is null or empty for " + this.Name);

            var code = new StringBuilder();
            code.AppendLinf("   public async Task<ActivityExecutionResult> {0}()", this.MethodName);
            code.AppendLine("   {");
            code.AppendLinf("        var item = new {0}();",this.EntityType);
            code.AppendLinf("        var self = this.WorkflowDefinition.ActivityCollection.OfType<CreateEntityActivity>().Single(a => a.WebId == \"{0}\");", this.WebId);

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
            code.AppendLine("var context = new Bespoke.Sph.Domain.SphDataContext();");
            code.AppendLine("using (var session = context.OpenSession())");
            code.AppendLine("{");
            code.AppendLine("    session.Attach(item);");
            code.AppendLine("    await session.SubmitChanges();");
            code.AppendLine("}");
            // set the next activity
            code.AppendLinf("       this.CurrentActivityWebId = \"{0}\";", this.NextActivityWebId);/* webid*/
            code.AppendLinf("       await this.SaveAsync(\"{0}\");", this.WebId);
            code.AppendLine("       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            //code.AppendLine("   result.NextActivity = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            code.AppendLine("       return result;");
            code.AppendLine("   }");

            return code.ToString();
        }
    }
}