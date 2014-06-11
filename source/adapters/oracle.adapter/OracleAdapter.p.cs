using System;
using System.Security;

namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class OracleAdapter
    {
        public string UserId { get; set; }
        public string Sid { get; set; }
        public string Password { get; set; }
        public SecureString SecurePassword { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }

        public string ConnectionString
        {
            get
            {
                var cs = String.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST={0})(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={2})));User Id={3};Password={4};",
                                        this.Server, this.Port, this.Sid, this.UserId, this.Password);
                return cs;

            }
        }
    }
}