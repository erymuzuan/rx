using System;
using System.Data;
using System.Text;
using System.Xml;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class SprocResultMember : SimpleMember
    {
        private SqlDbType m_sqlDbType;
        public SqlDbType SqlDbType
        {
            get
            {
                return (int)m_sqlDbType != 0 ? m_sqlDbType : SqlDbType.VarChar;
            }
            set { m_sqlDbType = value; }
        }

        public short? MaxLength { get; set; }
        public string FieldName { get; set; }
        public string DisplayName { get; set; }
        public string DbType { get; set; }
        public byte Length { get; set; }
        public byte Order { get; set; }

        public override string GeneratedCode(string padding = "      ")
        {
            if (null == this.Type)
                throw new InvalidOperationException(this + " doesn't have a type");
            var code = new StringBuilder();

            code.AppendLine(padding + $"//{this.GetType().Name} :{this.DbType}({this.Length}) {(IsNullable ? "" : "NOT ")}NULL");

            if (!string.IsNullOrWhiteSpace(PropertyAttribute))
                code.AppendLine(padding + PropertyAttribute);
            var displayName = string.IsNullOrWhiteSpace(this.DisplayName) ? this.Name : this.DisplayName;
            code.AppendLine($@"[JsonProperty(""{displayName}"", Order = {this.Order})]");

            code.AppendLine(padding + $"public {this.GetCsharpType()}{this.GetNullable()} {Name} {{ get; set; }}");
            return code.ToString();
        }

        public string GenerateReaderCode(string responseName = "item", string readerName = "reader")
        {
            if (Type == typeof(byte[]) && this.IsNullable)
                return $"{responseName}.{Name} = {readerName}[\"{FieldName}\"].ReadNullableByteArray();";

            if (Type == typeof(XmlDocument) && this.IsNullable)
                return $@"{responseName}.{Name} = {readerName}[""{FieldName}""].ReadNullableXmlDocument();";
            if (Type == typeof(XmlDocument) && !this.IsNullable)
                return $@"                         
                    var xml{Name} = new System.Xml.XmlDocument();
                    xml{Name}.LoadXml((string){readerName}[""{FieldName}""]);
                    {responseName}.{Name} = xml{Name};";

            if (Type == typeof(string) || !this.IsNullable)
                return $@"{responseName}.{Name} = ({Type.ToCSharp()}){readerName}[""{FieldName}""];";

            return $@"{responseName}.{Name} = {readerName}[""{FieldName}""].ReadNullable<{Type.ToCSharp()}>();";
        }
    }
}