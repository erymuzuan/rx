using System.Diagnostics;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("Path = {Name}({Pattern})/{Converter}, TypeName= {TypeName}")]
    public partial class HeaderFieldMapping : TextFieldMapping
    {
        public override Member GenerateMember()
        {
            return new HeaderFieldMember(this);
        }

        public string GenerateProcessRecordCode()
        {
            var code = new StringBuilder();
            var field = this;
            var varName = field.Name.ToCamelCase();
            var fieldName = field.Name;
            code.AppendLine("// Header: " + fieldName);
            code.AppendLine(!string.IsNullOrWhiteSpace(this.Pattern)
                ? $@"var {varName}Raw = Strings.RegexSingleValue(this.Headers[""{field.Name}""], {field.Pattern.ToVerbatim()}, ""value"");"
                : $@"var {varName}Raw = this.Headers[""{field.Name}""];");

            if (field.IsNullable)
            {
                var nullable = field.Type == typeof(string) ? "" : "?";
                code.AppendLine($"Func<string, {field.Type.ToCSharp()}{nullable}> func{fieldName} = x =>{{");
                code.AppendLine($@" if(null == x) return null;");
                code.AppendLine(field.GenerateNullableReadCode("x"));
                code.AppendLine("};");
                code.AppendLine($"record.{fieldName} = func{fieldName}({varName}Raw);");
            }
            else
            {
                var expression = field.GenerateReadExpressionCode($"{varName}Raw");
                code.AppendLine($"record.{fieldName} = {expression};");
            }

            return code.ToString();
        }
    }
}