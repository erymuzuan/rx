using System.Data;
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
        
    }
}