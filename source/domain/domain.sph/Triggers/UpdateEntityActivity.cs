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
                result.Errors.Add(new BuildError(this.WebId,$"[UpdateEntityActivity] -\"{this.Name}\" EntityIdPath is missing"));
            if (string.IsNullOrWhiteSpace(this.Entity))
                result.Errors.Add(new BuildError(this.WebId,$"[UpdateEntityActivity] -\"{this.Name}\" EntityType is missing"));

            if (!string.IsNullOrWhiteSpace(this.Entity))
                result.Errors.Add(new BuildError(this.WebId, $"[UpdateEntityActivity] -\"{this.Name}\" Cannot load {this.Entity}"));

            return result;
        }
        public override string GenerateExecMethodBody(WorkflowDefinition wd)
        {
            if (string.IsNullOrWhiteSpace(this.NextActivityWebId))
                throw new InvalidOperationException("NextActivityWebId is null or empty for " + this.Name);
            
            var context = new SphDataContext();
            var ed = context.LoadOneFromSources<EntityDefinition>(x => x.Id == this.Entity);

            var code = new StringBuilder();
            

            code.AppendLine("       var context = new Bespoke.Sph.Domain.SphDataContext();");
            code.AppendLine(this.IsUsingVariable
                ? $"       var item = this.{UseVariable};"
                : $"       var item = await context.LoadOneAsync<{ed.FullTypeName}>(e => e.Id == this.{EntityIdPath});");


            code.AppendLine("if(string.IsNullOrWhiteSpace(item.Id))throw new InvalidOperationException(\"Id cannot be null or empty\");");

            code.AppendLine($@"       var self = this.GetActivity<UpdateEntityActivity>(""{WebId}"");");

            foreach (var mapping in this.PropertyMappingCollection)
            {
                code.AppendLine($"           item.{mapping.Destination} = this.{mapping.Source};");
                if (mapping is FunctoidMapping)
                    throw new Exception("Functoid mapping is not yet supported");
            }
            code.AppendLine("      using (var session = context.OpenSession())");
            code.AppendLine("      {");
            code.AppendLine("          session.Attach(item);");
            code.AppendLine("          await session.SubmitChanges();");

            code.AppendLine("      }");
            // set the next activity
            code.AppendLine("       var result = new ActivityExecutionResult{Status = ActivityExecutionStatus.Success};");
            code.AppendLine($@"       result.NextActivities = new[]{{""{NextActivityWebId}""}};");
            

            return code.ToString();
        }
    }
}