using System;
using Bespoke.Sph.Domain.Codes;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain.Api
{
    public partial class Column : SimpleMember
    {
        public virtual bool CanWrite => true;
        [JsonIgnore]
        public virtual Type ClrType => typeof(object);
        public short Length { get; set; }
        public bool IsSelected { get; set; }

        public virtual string GenerateUpdateParameterValue(string commandName = "cmd")
        {
            var nullable = this.IsNullable ? ".ToDbNull()" : "";
            return $"{commandName}.Parameters.AddWithValue(\"@{Name}\", item.{Name}{nullable});";

        }
    
        public virtual string GenerateValueStatementCode(string dbValue)
        {
            return null;
        }
        public virtual string GenerateValueAssignmentCode(string dbValue)
        {
            return null;
        }
        [JsonIgnore]
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

        public void Merge(Column oc, TableDefinition table)
        {
            // TODO : copy users setting property like, MIME, inline data or not from oc to col
            this.IsComplex = oc.IsComplex;
            this.MimeType = oc.MimeType;
            this.IsVersion = table.VersionColumn == this.Name;
            this.IsModifiedDate = table.ModifiedDateColumn == this.Name;
            this.LookupColumnTable = oc.LookupColumnTable;
        }

        public virtual Property GetLookupProperty(Adapter adapter, TableDefinition table)
        {
            if (!this.LookupColumnTable.IsEnabled) return null;
            var prop = new Property
            {
                Name = this.LookupColumnTable.Name,
                Type = this.LookupColumnTable.Type
            };
            prop.AttributeCollection.Add("//" + new ColumnMetadata(this));
            return prop;
        }
    }
}