using System;
using System.Text;
using Bespoke.Sph.Domain.Codes;
using Humanizer;
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
            return $"{commandName}.Parameters.AddWithValue(\"@{ClrName}\", item.{ClrName}{nullable});";

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

        public virtual Column Initialize(Adapter adapter, TableDefinition td, ColumnMetadata mt)
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
            col.WebId = Guid.NewGuid().ToString();
            if (string.IsNullOrWhiteSpace(col.DisplayName))
            {
                switch (adapter.ColumnDisplayNameStrategy)
                {
                    case "camel":
                        col.DisplayName = mt.Name.Camelize();
                        break;
                    case "Pascal":
                        col.DisplayName = mt.Name.Pascalize();
                        break;
                    case "-":
                        col.DisplayName = mt.Name.Dasherize();
                        break;
                    case "_":
                        col.DisplayName = mt.Name.Underscore();
                        break;
                    default:
                        col.DisplayName = null;
                        break;
                }
            }

            return col;
        }

        public string DbType { get; set; }

        public override string ToString()
        {
            return new ColumnMetadata(this) + " // - " + this.GetType().FullName;
        }
        public string ClrName => this.Name.ToPascalCase();
        [JsonIgnore]
        public string LookupClrName => $"{this.LookupColumnTable.Name}{ClrName}Lookup".ToPascalCase();

        public override string GeneratedCode(string padding = "      ")
        {
            if (null == this.Type)
                throw new InvalidOperationException(this + " doesn't have a type");
            var code = new StringBuilder();

            code.AppendLine(padding + $"//{this.GetType().Name} :{this.DbType}({this.Length}) {(IsNullable ? "" : "NOT ")}NULL");

            if (!string.IsNullOrWhiteSpace(PropertyAttribute))
                code.AppendLine(padding + PropertyAttribute);
            var displayName = string.IsNullOrWhiteSpace(this.DisplayName) ? this.Name : this.DisplayName;
            var canIgnore = this.IsComplex || this.Ignore || this.IsVersion || this.IsModifiedDate;
            code.AppendLine(canIgnore ? "[JsonIgnore]" : $@"[JsonProperty(""{displayName}"", Order = {this.Order})]");

            code.AppendLine(padding + $"public {this.GetCsharpType()}{this.GetNullable()} {ClrName} {{ get; set; }}");
            return code.ToString();
        }
        public virtual string GenerateReadAdapterCode(TableDefinition table, Adapter adapter)
        {
            return null;
        }

        public virtual void Merge(Column oc, TableDefinition table)
        {
            // TODO : copy users setting property like, MIME, inline data or not from oc to col
            this.DefaultValue = oc.DefaultValue;
            this.DisplayName = oc.DisplayName;
            this.Ignore = oc.Ignore;
            this.IsComplex = oc.IsComplex;
            this.IsModifiedDate = table.ModifiedDateColumn == this.Name;
            this.IsVersion = table.VersionColumn == this.Name;
            this.LookupColumnTable = oc.LookupColumnTable;
            this.MimeType = oc.MimeType;
            this.Order = oc.Order;
            this.WebId = oc.WebId ?? Guid.NewGuid().ToString();
        }

        public virtual Property GetLookupProperty(Adapter adapter, TableDefinition table)
        {
            if (!this.LookupColumnTable.IsEnabled) return null;
            var prop = new Property
            {
                Name = LookupClrName,
                Type = this.LookupColumnTable.Type
            };
            prop.AttributeCollection.Add($"//{this.GetType().Name}:{new ColumnMetadata(this)}");
            prop.AttributeCollection.Add($@"[JsonProperty(""{LookupColumnTable.Name}"", Order={Order})]");
            return prop;
        }


        public virtual string GetDefaultValueCode(TableDefinition table, string itemIdentifier = "item", string adapterIdentifier = "adapterDefinition")
        {
            if (null == this.DefaultValue) return null;
            if (this.IsNullable) return null;
            if (!(Ignore || IsComplex)) return null;
            
            var code = new StringBuilder();
            code.AppendLine($@"           var field{ClrName} = {adapterIdentifier}.TableDefinitionCollection.Single(x => x.Name == ""{table.Name}"" && x.Schema == ""{table.Schema}"" )
                                                                .ColumnCollection.Single(x => x.Name == ""{this.Name}"").DefaultValue;");
            code.AppendLine($"           var val{ClrName} = field{ClrName}.GetValue(rc);");
            code.AppendLine($"           {itemIdentifier}.{Name} = ({Type.FullName})val{ClrName};");

            return code.ToString();

        }
    }
}