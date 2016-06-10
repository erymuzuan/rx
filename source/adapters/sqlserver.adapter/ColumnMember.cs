using System.Text;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class ColumnMember : SimpleMember
    {
        private readonly SqlColumn m_column;
        public bool IsVersion { get; set; }
        public bool IsModifiedDate { get; set; }

        public ColumnMember() { }

        public ColumnMember(SqlColumn column)
        {
            m_column = column;
            Name = column.Name;
            IsNullable = column.IsNullable;
            IsFilterable = true;
        }

        public override string GeneratedCode(string padding = "      ")
        {
            var code = new StringBuilder();
            if (null != m_column)
                code.AppendLine(padding + $"//{m_column}");
            if (this.IsVersion || this.IsModifiedDate)
                code.AppendLine(padding + "[Newtonsoft.Json.JsonIgnore]");
            code.AppendLine(padding + $"public {this.GetCsharpType()}{this.GetNullable()} {Name} {{ get; set; }}");
            return code.ToString();
        }
    }
}