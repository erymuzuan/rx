using System;
using System.Data;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class SprocResultMember : Member
    {
        private SqlDbType m_sqlDbType;

        public SqlDbType SqlDbType
        {
            get
            {

                if ((int)m_sqlDbType != 0)
                    return m_sqlDbType;


                return SqlDbType.VarChar;
            }
            set { m_sqlDbType = value; }
        }

        public Type Type { get; set; }
    }
}