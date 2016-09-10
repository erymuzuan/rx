using System.Text;

namespace Bespoke.Sph.Domain
{
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
            code.AppendLine($@"var {varName}Raw = Strings.RegexSingleValue(this.Headers[""{field.Header}""], {field.Pattern.ToVerbatim()}, ""value"");");
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