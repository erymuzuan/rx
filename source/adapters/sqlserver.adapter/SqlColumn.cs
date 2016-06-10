using System;
using Bespoke.Sph.Domain;
using System.Data;
using Bespoke.Sph.Domain.Api;

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
        public bool IsVersion { get; set; }
        public bool IsModifiedDate { get; set; }

        public virtual string GenerateUpdateParameterValue(string commandName = "cmd")
        {
            var nullable = this.IsNullable ? ".ToDbNull()" : "";
            return $"{commandName}.Parameters.AddWithValue(\"@{Name}\", item.{Name}{nullable});";
            
        }
        public virtual string GenerateReadCode()
        {
            return $"item.{Name} = ({ClrType.ToCSharp()})reader[\"{Name}\"];";
        }

        public virtual Member GetMember(TableDefinition td)
        {
            return new ColumnMember(this)
            {
                Type = this.ClrType,
                IsVersion = td.VersionColumn == this.Name,
                IsModifiedDate = td.ModifiedDateColumn == this.Name
            };
        }

        public virtual SqlColumn Initialize(ColumnMetadata mt, TableDefinition td)
        {
            var col = this.JsonClone();
            col.Name = mt.Name;
            col.SqlType = (SqlDbType)Enum.Parse(typeof(SqlDbType), mt.SqlType, true);
            col.IsNullable = mt.IsNullable;
            col.IsIdentity = mt.IsIdentity;
            col.IsComputed = mt.IsComputed;
            col.Length = mt.Length;
            col.IsPrimaryKey = mt.IsPrimaryKey;
            col.IsVersion = td.VersionColumn == col.Name;
            col.IsModifiedDate = td.ModifiedDateColumn == col.Name;

            return col;
        }

        public override string ToString()
        {
            return new ColumnMetadata(this) + " // - " + this.GetType().FullName;
        }
    }

    public class SqlTable
    {
        public string Name { get; set; }
        public ObjectCollection<SqlColumn> ColumnCollection { get; } = new ObjectCollection<SqlColumn>();
    }
}