using System;
using Bespoke.Sph.Domain;
using System.Data;
using System.Text;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class SqlColumn
    {
        public string Name { get; set; }
        public virtual bool CanWrite => true;
        public virtual SqlDbType SqlType { get; set; }
        public virtual Type ClrType => this.SqlType.ToString().GetClrType();
        public virtual bool IsNullable { get; set; }
        public short Length { get; set; }
        public bool IsIdentity { get; set; }
        public bool IsComputed { get; set; }
        public bool IsPrimaryKey { get; set; }

        public virtual string GenerateUpdateParameterValue(string commandName = "cmd")
        {
            var nullable = this.IsNullable ? ".ToDbNull()" : "";
            return $"{commandName}.Parameters.AddWithValue(\"@{Name}\", item.{Name}{nullable});";
            
        }
        public virtual string GenerateReadCode()
        {
            return $"item.{Name} = ({ClrType.ToCSharp()})reader[\"{Name}\"];";
        }

        public virtual Member GetMember()
        {
            return new ColumnMember(this)
            {
                Type = this.ClrType
            };
        }

        public virtual SqlColumn Initialize(ColumnMetadata mt)
        {
            var col = this.JsonClone();
            col.Name = mt.Name;
            col.SqlType = (SqlDbType)Enum.Parse(typeof(SqlDbType), mt.SqlType, true);
            col.IsNullable = mt.IsNullable;
            col.IsIdentity = mt.IsIdentity;
            col.IsComputed = mt.IsComputed;
            col.Length = mt.Length;
            col.IsPrimaryKey = mt.IsPrimaryKey;

            return col;
        }

        public override string ToString()
        {
            return new ColumnMetadata(this).ToString();
        }
    }

    public class ColumnMember : SimpleMember
    {
        private readonly SqlColumn m_column;

        public ColumnMember(){}

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
            code.AppendLine(padding + $"public {this.GetCsharpType()}{this.GetNullable()} {Name} {{ get; set; }}");
            return code.ToString();
        }
    }
    public class SqlTable
    {
        public string Name { get; set; }
        public ObjectCollection<SqlColumn> ColumnCollection { get; } = new ObjectCollection<SqlColumn>();
    }
}