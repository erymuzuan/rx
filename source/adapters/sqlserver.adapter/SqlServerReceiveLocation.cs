using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    [EntityType(typeof(ReceiveLocation))]
    [Export("ReceiveLocationDesigner", typeof(ReceiveLocation))]
    [DesignerMetadata(FriendlyName = "Microsoft SQL Server", Route = "receive.location.sqlserver/:id",
         FontAwesomeIcon = "windows", Name = "sqlserver")]
    public partial class SqlServerReceiveLocation : ReceiveLocation
    {
    }
}
