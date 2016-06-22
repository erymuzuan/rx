using System.Data;
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

        public string GenerateReaderCode(string responseName = "item", string readerName = "reader")
        {
            if(Type == typeof(byte[]) && this.IsNullable)
                return $"{responseName}.{Name} = {readerName}[\"{Name}\"].ReadNullableByteArray();";

            if (Type == typeof(XmlDocument) && this.IsNullable)
                return $@"{responseName}.{Name} = {readerName}[""{Name}""].ReadNullableXmlDocument();";
            if (Type == typeof(XmlDocument) && !this.IsNullable)
                return $@"                         
                    var xml{Name} = new System.Xml.XmlDocument();
                    xml{Name}.LoadXml((string){readerName}[""{Name}""]);
                    {responseName}.{Name} = xml{Name};";

            if (Type == typeof(string) || !this.IsNullable)
                return $@"{responseName}.{Name} = ({Type.ToCSharp()}){readerName}[""{Name}""];";

            return $@"{responseName}.{Name} = {readerName}[""{Name}""].ReadNullable<{Type.ToCSharp()}>();";
        }
    }
}