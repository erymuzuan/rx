using System.Linq;
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

        public override BuildValidationResult ValidateBuild(WorkflowDefinition wd)
        {
            var result = base.ValidateBuild(wd);
            if (string.IsNullOrWhiteSpace(this.TypeName))
            {
                result.Result = false;
                result.Errors.Add(new BuildError(this.WebId, string.Format("[Variable] \"{0}\" does not have a valid type", this.Name)));
            }

            return result;
        }

        public override string GetJsonIntance(WorkflowDefinition wd)
        {

            var schema = wd.GetCustomSchema();

            var xsd = new XsdMetadata(schema);
            var members = xsd.GetMembersPath(this.TypeName);

            var json = new StringBuilder();
            json.AppendFormat("\"{0}\":", this.Name);
            json.AppendLine("{");

            var childs = members.Select(x => string.Format("\"{0}\":null", x));
            json.AppendLine(string.Join(",\r\n", childs));

            // TODO : split the child aggregate into it's own object

            json.AppendLine("}");

            return json.ToString();
        }
    }
}