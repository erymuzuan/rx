using System;
using Bespoke.Sph.Domain;
using System.Data;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class SqlColumn : Column
    {
        public virtual SqlDbType SqlType { get; private set; }
        public override string GenerateUpdateParameterValue(string commandName = "cmd")
        {
            var nullable = this.IsNullable ? ".ToDbNull()" : "";
            return $"{commandName}.Parameters.AddWithValue(\"@{Name}\", item.{Name}{nullable});";

        }
        public override string GenerateReadCode()
        {
            return $"item.{Name} = ({ClrType.ToCSharp()})reader[\"{Name}\"];";
        }

        public override Member GetMember(TableDefinition td)
        {
            return new ColumnMember(this)
            {
                Type = this.ClrType,
                IsVersion = td.VersionColumn == this.Name,
                IsModifiedDate = td.ModifiedDateColumn == this.Name
            };
        }

        public override Column Initialize(ColumnMetadata mt, TableDefinition td)
        {
            var col = this.JsonClone();
            col.Name = mt.Name;
            col.IsNullable = mt.IsNullable;
            col.IsIdentity = mt.IsIdentity;
            col.IsComputed = mt.IsComputed;
            col.Length = mt.Length;
            col.IsPrimaryKey = mt.IsPrimaryKey;
            col.IsVersion = td.VersionColumn == col.Name;
            col.IsModifiedDate = td.ModifiedDateColumn == col.Name;

            SqlDbType st;
            if (Enum.TryParse(mt.DbType, true, out st))
                col.SqlType = st;

            return col;
        }

        public override string ToString()
        {
            return new ColumnMetadata(this) + " // - " + this.GetType().FullName;
        }

        public virtual string GenerateReadAdapterCode(TableDefinition table, SqlServerAdapter adapter)
        {
            return null;
        }
    }


}