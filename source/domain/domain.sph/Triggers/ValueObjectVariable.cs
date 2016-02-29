using System;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class ValueObjectVariable : Variable
    {
        public override string GeneratedCode(WorkflowDefinition workflowDefinition)
        {
            var context = new SphDataContext();
            var vod = context.LoadOneFromSources<ValueObjectDefinition>(x => x.Name == this.ValueObjectDefinition);

            var code = new StringBuilder();
            code.AppendLine($"   private {vod.Name} m_{Name} = new {this.Name}();");
            code.AppendLine($"   public {vod.Name} {Name}");
            code.AppendLine("   {");
            code.AppendLine($"       get{{ return m_{Name};}}");
            code.AppendLine($"       set{{ m_{Name} = value;}}");
            code.AppendLine("   }");
            return code.ToString();
        }

        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            if (this.Name.Contains(" "))
            {
                result.Result = false;
                result.Errors.Add(new BuildError(this.WebId)
                {
                    Message = $"[Variable] \"{this.Name}\" cannot contains space "
                });
            }

            return result;
        }
    }


}