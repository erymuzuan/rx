using System;
using System.Data;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class SprocResultMember : Member
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

        public string TypeName { get; set; }
        public bool IsNullable { get; set; }
        [JsonIgnore]
        public Type Type
        {
            get
            {
                return Strings.GetType(this.TypeName);
            }
            set
            {
                this.TypeName = value.GetShortAssemblyQualifiedName();
            }
        }
    }
}