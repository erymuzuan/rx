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

        
    }
}