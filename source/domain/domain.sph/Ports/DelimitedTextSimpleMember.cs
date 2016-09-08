using System;
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
            this.IsNullable = fieldMapping.IsNullable;
        }
        public override string GeneratedCode(string padding = "      ")
        {
            var type = this.Type.ToCSharp() + this.GetNullable();
            var rawType = this.IsNullable ? "string" : this.Type.ToCSharp();

            var code = new StringBuilder();
            code.AppendLine($@"{padding}[JsonIgnore]");
            code.AppendLine($@"{padding}public {rawType} {Name}Raw;");
            code.Append($@"{padding}public {type} {Name}");
            if (this.IsNullable)
            {
                code.AppendLine("{");
                code.AppendLine("    get{");
                code.AppendLine($@"        if({Name}Raw == {m_fieldMapping.NullPlaceholder.ToVerbatim()})");
                code.AppendLine($@"             return null;");
                if (this.Type == typeof(string))
                    code.AppendLine($@"        return {Name}Raw;");
                if (this.Type == typeof(DateTime))
                    code.AppendLine($@"        return DateTime.ParseExact({Name}Raw,{m_fieldMapping.Converter.ToVerbatim()}, System.Globalization.CultureInfo.InvariantCulture);");

                // TODO : parse according to format
                if (this.Type == typeof(int))
                    code.AppendLine($@"        
                                        var n = 0;
                                        if(int.TryParse({Name}Raw, NumberStyles.Any, CultureInfo.InvariantCulture, out n)) return n;
                                        return null;");
                //TODO : parse according to format
                if (this.Type == typeof(decimal))
                    code.AppendLine($@"        
                                        var n = 0m;
                                        if(decimal.TryParse({Name}Raw, NumberStyles.Any, CultureInfo.InvariantCulture, out n)) return n;
                                        return null;");
                // TODO : assuming Converter is the True string
                if (this.Type == typeof(bool))
                    code.AppendLine($@"  return {Name}Raw == {m_fieldMapping.Converter.ToVerbatim()};");

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