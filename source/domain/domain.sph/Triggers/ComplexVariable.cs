using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class ComplexVariable : Variable
    {
        public override string GeneratedCode(WorkflowDefinition wd)
        {
            var code = new StringBuilder();
            code.AppendFormat("public {0} {1} {{get;set;}}", this.TypeName, this.Name);
            return code.ToString();
        }

        public override BuildValidationResult ValidateBuild()
        {
            var result = new BuildValidationResult();
            if (this.Name.Contains(" "))
            {
                result.Result = false;
                result.Errors.Add(new BuildError { Message = string.Format("Variable {0} cannot contains space ", this.Name) });
            }

            return result;
        }

        
    }
}