using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Newtonsoft.Json;

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
        [JsonIgnore]
        public string ConnectionString
        {
            get
            {
                if(this.TrustedConnection)
                return
                    $@"Data Source={this.Server};Initial Catalog={this.Database
                        };Integrated Security=True;MultipleActiveResultSets=True";
                return $"Server={this.Server};Database={this.Database};User Id={this.UserId};Password={this.Password};;MultipleActiveResultSets=True";
            }
        }

    }
}