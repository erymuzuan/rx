using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;

namespace Bespoke.Sph.Integrations.Adapters
{
    [EntityType(typeof(Adapter))]
    [Export("AdapterDesigner", typeof(Adapter))]
    [DesignerMetadata(Name = "MS SQL database", FontAwesomeIcon = "windows", RouteTableProvider = typeof(SqlServerAdapterRouteProvider), Route = "adapter.sqlserver/0")]
    public partial class SqlServerAdapter
    {
        public string Server { get; set; }
        public bool TrustedConnection { get; set; }
        public string UserId { get; set; }
        public string Password { get; set; }
        public string Database { get; set; }
        public string ConnectionString
        {
            get
            {
                if(this.TrustedConnection)
                return string.Format(@"Data Source={0};Initial Catalog={1};Integrated Security=True;MultipleActiveResultSets=True",
                    this.Server, this.Database);
                return string.Format("Server={0};Database={1};User Id={2};Password={3};",
                    this.Server, this.Database, this.UserId, this.Password);
            }
        }

        
    }
}