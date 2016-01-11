using System;
using System.Text;
using Bespoke.Sph.Domain;
using FileHelpers;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class FlatFileMember : Member
    {
        public FieldConverterAttribute FieldConverter { get; set; }
        public Type Type { get; set; }
        public bool IsNullable { get; set; }

        protected string GetCsharpType()
        {
            return this.Type.ToCSharp();
        }
        protected string GetNullable()
        {
            if (!this.IsNullable) return string.Empty;
            if (typeof(string) == this.Type) return string.Empty;
            if (typeof(object) == this.Type) return string.Empty;
            if (typeof(Array) == this.Type) return string.Empty;
            return "?";
        }
        public override string GeneratedCode(string padding = "      ")
        {
            if (null == this.Type)
                throw new InvalidOperationException(this + " doesn't have a type");
            var code = new StringBuilder();
            if (typeof(object) == this.Type || typeof(Array) == this.Type)
            {
                return base.GeneratedCode(padding);
            }

            if (!string.IsNullOrWhiteSpace(PropertyAttribute))
                code.AppendLine(padding + PropertyAttribute);

            code.AppendLinf(padding + $"public {this.GetCsharpType()}{this.GetNullable()} {this.Name};");
            return code.ToString();
        }
    }
}
