using System;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public class DelimitedTextSimpleMember : SimpleMember
    {
        private readonly DelimitedTextFieldMapping m_fieldMapping;

        public DelimitedTextSimpleMember(DelimitedTextFieldMapping fieldMapping)
        {
            if(null == fieldMapping)
                throw new ArgumentNullException(nameof(fieldMapping));
            m_fieldMapping = fieldMapping;
            this.Name = fieldMapping.Name;
            this.TypeName = fieldMapping.TypeName;
            this.AllowMultiple = false;
            this.IsNullable = fieldMapping.IsNullable;
        }
        public override string GeneratedCode(string padding = "      ")
        {
            var type = this.Type.ToCSharp() + this.GetNullable();
            var code = new StringBuilder();
            code.AppendLine($@"{padding}[JsonIgnore]");
            code.AppendLine($@"{padding}public string {Name}Raw;");
            code.Append($@"{padding}public {type} {Name}");
            if (this.IsNullable)
            {
                code.AppendLine("{");
                code.AppendLine("    get{");
                code.AppendLine($@"        if({Name}Raw == {m_fieldMapping.NullPlaceholder.ToVerbatim()})");
                code.AppendLine($@"             return null;");
                code.AppendLine($"         {m_fieldMapping.GenerateNullableReadCode(Name + "Raw")}");

                code.AppendLine("       }");
                code.AppendLine("}");
            }
            else
            {
                var expression = m_fieldMapping.GenerateReadExpressionCode(Name + "Raw");
                code.AppendLine($@" => {expression};");
            }

            return code.ToString();



        }
    }
}