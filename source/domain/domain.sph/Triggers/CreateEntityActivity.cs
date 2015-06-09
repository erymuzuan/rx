using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [Export("ActivityDesigner", typeof(Activity))]
    [DesignerMetadata(Name = "Create a record", TypeName = "CreateEntity", Description = "Create a new record for your entity")]
    public partial class CreateEntityActivity : Activity
    {
        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = new BuildValidationResult { Result = true };
            try
            {
                var assembly = Assembly.Load(ConfigurationManager.ApplicationName + "." + this.EntityType);
                Console.WriteLine(assembly);
            }
            catch (FileNotFoundException)
            {
                result.Errors.Add(new BuildError(this.WebId, $"[CreateEntityActivity] : Cannot find custom entity assembly \"{ConfigurationManager.ApplicationName}.{this.EntityType}\" for {this.Name}"));

            }
            result.Result = result.Errors.Count == 0;
            return result;
        }


        public override string GenerateExecMethodBody(WorkflowDefinition wd)
        {
            if (string.IsNullOrWhiteSpace(this.NextActivityWebId))
                throw new InvalidOperationException("NextActivityWebId is null or empty for " + this.Name);
            var context = new SphDataContext();
            var ed = context.LoadOne<EntityDefinition>(d => d.Name == this.EntityType);
            var entityFullName = $"Bespoke.{ConfigurationManager.ApplicationName}_{ed.Id}.Domain.{ed.Name}";

            var code = new StringBuilder();

            var itemMapping = this.PropertyMappingCollection.FirstOrDefault(x => x.Destination == ".");
            code.AppendLine(null == itemMapping
                ? $"        var item = new {entityFullName}();"
                : $"        var item = this.{itemMapping.Source};");


            code.AppendLine("           item.Id = Guid.NewGuid().ToString();");
            foreach (var mapping in this.PropertyMappingCollection.Where(x => x.Destination != "."))
            {
                code.AppendLinf("           item.{0} = this.{1};", mapping.Destination, mapping.Source);
            }

            code.AppendLine("      var context = new Bespoke.Sph.Domain.SphDataContext();");
            code.AppendLine("      using (var session = context.OpenSession())");
            code.AppendLine("      {");
            code.AppendLine("          session.Attach(item);");
            code.AppendLine("          await session.SubmitChanges();");
            if (!string.IsNullOrWhiteSpace(this.ReturnValuePath))
                code.AppendLinf("          this.{0} = item.Id;", this.ReturnValuePath);
            code.AppendLine("      }");
            // set the next activity
            code.AppendLine("       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            code.AppendLinf("       result.NextActivities = new[]{{\"{0}\"}};", this.NextActivityWebId);/* webid*/


            return code.ToString();
        }


    }
}