using System;
using System.Text;

namespace Bespoke.Sph.Domain
{
    public class DelimitedTextSimpleMember : SimpleMember
    {
        //#6240, let json serializer works on the public property
        public DelimitedTextFieldMapping FieldMapping { get; }

        public DelimitedTextSimpleMember()
        {

        }

        public DelimitedTextSimpleMember(DelimitedTextFieldMapping fieldMapping)
        {
            FieldMapping = fieldMapping ?? throw new ArgumentNullException(nameof(fieldMapping));
            this.Name = fieldMapping.Name;
            this.TypeName = fieldMapping.TypeName;
            this.AllowMultiple = false;
            this.IsNullable = fieldMapping.IsNullable;
        }
        public override string GeneratedCode(string padding = "      ")
        {
            if (null == this.FieldMapping) throw new InvalidOperationException("The FieldMapping has not been set, please re-create your EntityDefinition");
            var type = this.Type.ToCSharp() + this.GetNullable();
            var code = new StringBuilder();
            code.AppendLine($@"{padding}[JsonIgnore]");
            code.AppendLine($@"{padding}public string {Name}Raw;");
            code.Append($@"{padding}public {type} {Name}");
            if (this.IsNullable)
            {
                code.AppendLine("{");
                code.AppendLine("    get{");
                code.AppendLine($@"        if({Name}Raw == {FieldMapping.NullPlaceholder.ToVerbatim()})");
                code.AppendLine($@"             return null;");
                code.AppendLine($"         {FieldMapping.GenerateNullableReadCode(Name + "Raw")}");

                code.AppendLine("       }");
                code.AppendLine("}");
            }
            else
            {
                var expression = FieldMapping.GenerateReadExpressionCode(Name + "Raw");
                code.AppendLine($@" => {expression};");
            }

            return code.ToString();



        }
    }
}