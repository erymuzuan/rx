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
                var host = string.Empty;
                if (!string.IsNullOrWhiteSpace(this.Server))
                    host = string.Format("(HOST={0})", this.Server);
                var port = this.Port == 0 ? 1521 : this.Port;
                var cs = String.Format("Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP){0}(PORT={1})))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME={2})));User Id={3};Password={4};",
                    host, port, this.Sid, this.UserId, this.Password);
                return cs;

            }
        }
    }
}