using System.Text;

namespace Bespoke.Sph.Domain
{
    public class DelimitedTextSimpleMember : SimpleMember
    {
        private readonly DelimitedTextFieldMapping m_fieldMapping;

        public DelimitedTextSimpleMember(DelimitedTextFieldMapping fieldMapping)
        {
            m_fieldMapping = fieldMapping;
            this.Name = fieldMapping.Name;
            this.TypeName = fieldMapping.TypeName;
            this.AllowMultiple = false;
            this.IsNullable = this.IsNullable;
        }
        public override string GeneratedCode(string padding = "      ")
        {
            string kind;
            /*
    None,
    Date,
    Boolean,
    Byte,
    Int16,
    Int32,
    Int64,
    Decimal,
    Double,
    PercentDouble,
    Single,
    SByte,
    UInt16,
    UInt32,
    UInt64,
    DateMultiFormat,
    Char,
    Guid,*/
            switch (this.TypeName)
            {
                case "System.DateTime, mscorlib":
                    kind = "Date"; break;
                case "System.Int32, mscorlib":
                    kind = "Int"; break;
                case "System.Decimal, mscorlib":
                    kind = "Decimal"; break;
                case "System.Boolean, mscorlib":
                    kind = "Boolean"; break;
                default:
                    kind = "None";
                    break;
            }

            var type = this.Type.ToCSharp() + this.GetNullable();

            var code = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(m_fieldMapping.Converter))
                code.AppendLine($@"[FieldConverter(ConverterKind.{kind}, ""{m_fieldMapping.Converter}"")]");
            code.AppendLine($@"{padding}[JsonIgnore]");
            code.AppendLine($@"{padding}public {Type.ToCSharp()} {Name}Raw;");
            code.Append($@"{padding}public {type} {Name}");
            if (this.IsNullable)
            {
                code.AppendLine("{");
                code.AppendLine("    get{");
                code.AppendLine($@"        if({Name}Raw) == {m_fieldMapping.NullPlaceholder.ToVerbatim()})");
                code.AppendLine($@"             return null;");
                code.AppendLine($@"        return {Name}Raw;");
                code.AppendLine("       }");
                code.AppendLine("}");
            }
            else
            {
                code.AppendLine($@" => {Name}Raw;");
            }

            return code.ToString();



        }
    }
}