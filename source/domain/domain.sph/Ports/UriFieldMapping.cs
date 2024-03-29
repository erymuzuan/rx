﻿using System.Diagnostics;
using System.Text;

namespace Bespoke.Sph.Domain
{
    [DebuggerDisplay("Path = {Name}({Pattern})/{Converter}, TypeName= {TypeName}")]
    public partial class UriFieldMapping : TextFieldMapping
    {
        public override Member GenerateMember()
        {
            return new HeaderFieldMember(this);
        }

        public string GenerateProcessRecordCode()
        {
            var code = new StringBuilder();
            var varName = Name.ToCamelCase();
            code.AppendLine("// Uri: " + Name);
            code.AppendLine(!string.IsNullOrWhiteSpace(this.Pattern)
                ? $@"var {varName}Raw = Strings.RegexSingleValue(this.Uri.ToString(), {Pattern.ToVerbatim()}, ""value"");"
                : $@"var {varName}Raw = this.Uri.ToString();");
            if (IsNullable)
            {
                var nullable = Type == typeof(string) ? "" : "?";
                code.AppendLine($"Func<string, {Type.ToCSharp()}{nullable}> func{Name} = x =>{{");
                code.AppendLine(" if(null == x) return null;");
                code.AppendLine(GenerateNullableReadCode("x"));
                code.AppendLine("};");
                code.AppendLine($"record.{Name} = func{Name}({varName}Raw);");
            }
            else
            {
                var expression = GenerateReadExpressionCode($"{varName}Raw");
                code.AppendLine($"record.{Name} = {expression};");
            }
            code.AppendLine();
            return code.ToString();
        }
    }
}