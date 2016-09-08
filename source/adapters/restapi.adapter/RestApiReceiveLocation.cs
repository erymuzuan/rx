using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace restapi.adapter
{
    [EntityType(typeof(ReceiveLocation))]
    [Export("ReceiveLocationDesigner", typeof(ReceiveLocation))]
    [DesignerMetadata(FriendlyName = "Rest API", Route = "receive.location.restapi/:id", FontAwesomeIcon = "gg",
         Name = "restapi")]
    public class RestApiReceiveLocation : ReceiveLocation
    {
        
    }
}