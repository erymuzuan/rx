using System.Text;

namespace Bespoke.Sph.Domain
{
    public partial class HeaderFieldMapping : TextFieldMapping
    {
        protected override string GenerateReadFieldCode(string objectName, string rawName = "")
        {
            var code = new StringBuilder();
            var field = this;
            var name = field.Name.ToCamelCase();
            code.AppendLine($@"var {name}Raw = Strings.RegexSingleValue(this.Headers[""{field.Header}""], {field.Pattern.ToVerbatim()}, ""value"");");
            code.AppendLine(base.GenerateReadFieldCode("record"));
            return code.ToString();
        }


    }
}