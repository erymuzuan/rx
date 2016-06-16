using System;

namespace Bespoke.Sph.Domain.Api
{
    public partial class Column : SimpleMember
    {
        public virtual bool CanWrite => true;
        public virtual Type ClrType => typeof(object);
        public short Length { get; set; }
        public bool IsSelected { get; set; }

        public virtual string GenerateUpdateParameterValue(string commandName = "cmd")
        {
            var nullable = this.IsNullable ? ".ToDbNull()" : "";
            return $"{commandName}.Parameters.AddWithValue(\"@{Name}\", item.{Name}{nullable});";

        }
        public virtual string GenerateReadCode()
        {
            return $"item.{Name} = ({ClrType.ToCSharp()})reader[\"{Name}\"];";
        }

        public override Type Type
        {
            get { return this.ClrType; }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }


        public virtual Column Initialize(ColumnMetadata mt, TableDefinition td)
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

            return col;
        }

        public string DbType { get; set; }

        public override string ToString()
        {
            return new ColumnMetadata(this) + " // - " + this.GetType().FullName;
        }

        public virtual string GenerateReadAdapterCode(TableDefinition table, Adapter adapter)
        {
            return null;
        }
        
    }
}