using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    [EntityType(typeof(ReceiveLocation))]
    [Export("ReceiveLocationDesigner", typeof(ReceiveLocation))]
    [DesignerMetadata(FriendlyName = "Rest API", Route = "receive.location.restapi/:id", FontAwesomeIcon = "gg",
         Name = "restapi")]
    public class RestApiReceiveLocation : ReceiveLocation
    {
        public string BaseAddress { get; set; }
        public string ContentType { get; set; }
        public string InboundMapping { get; set; }
        public string InboundType { get; set; }
        public string Method { get; set; }
        public string Route { get; set; }
        public bool InProcess { get; set; }
        public ObjectCollection<HttpHeader> Headers { get; } = new ObjectCollection<HttpHeader>();
    }
}