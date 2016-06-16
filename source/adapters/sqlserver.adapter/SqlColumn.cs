using System;
using Bespoke.Sph.Domain;
using System.Data;
using System.Text;
using Bespoke.Sph.Domain.Api;
using Newtonsoft.Json;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class SqlColumn : Column
    {
        public virtual SqlDbType SqlType { get; set; }
        public override string GenerateUpdateParameterValue(string commandName = "cmd")
        {
            var nullable = this.IsNullable ? ".ToDbNull()" : "";
            return $"{commandName}.Parameters.AddWithValue(\"@{Name}\", item.{Name}{nullable});";

        }
        public override string GenerateReadCode()
        {
            return $"item.{Name} = ({ClrType.ToCSharp()})reader[\"{Name}\"];";
        }

        public override string GeneratedCode(string padding = "      ")
        {
            if (null == this.ClrType)
                throw new InvalidOperationException(this + " doesn't have a type");
            var code = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(PropertyAttribute))
                code.AppendLine(padding + PropertyAttribute);

            code.AppendLine(padding + $"//{this.GetType().Name} :{this.DbType}({this.Length}) {(IsNullable ? "" : "NOT ")}NULL");
            code.AppendLine(padding + $"public {this.GetCsharpType()}{this.GetNullable()} {Name} {{ get; set; }}");
            return code.ToString();
        }

        public override Column Initialize(ColumnMetadata mt, TableDefinition td)
        {
            var col = this.JsonClone();
            col.Name = mt.Name;
            col.DbType = mt.DbType;
            col.IsNullable = mt.IsNullable;
            col.IsIdentity = mt.IsIdentity;
            col.IsComputed = mt.IsComputed;
            col.Length = mt.Length;
            col.IsPrimaryKey = mt.IsPrimaryKey;
            col.IsVersion = td.VersionColumn == col.Name;
            col.IsModifiedDate = td.ModifiedDateColumn == col.Name;
            col.TypeName = col.ClrType.GetShortAssemblyQualifiedName();

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
        [JsonIgnore]
        public override Type ClrType
        {
            get
            {
                if(string.IsNullOrWhiteSpace(this.DbType))
                    throw new Exception($"[{Name}]Cannot get CLR type for " + this);
                return this.DbType.GetClrType();
            }
        }
    }


}