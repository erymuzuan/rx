using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Integrations.Adapters
{
    [EntityType(typeof(Adapter))]
    [Export("AdapterDesigner", typeof(Adapter))]
    [DesignerMetadata(Name = "restapi", FriendlyName = "REST Api", FontAwesomeIcon = "gg",
         RouteTableProvider = typeof(RestApiAdapterRouteProvider), Route = "adapter.restapi/0")]
    public partial class RestApiAdapter : Adapter
    {
        public RestApiAdapter()
        {

        }
        public RestApiAdapter(string odataTranslator)
        {
            OdataTranslator = odataTranslator;
        }

        protected override Task<Class> GenerateOdataTranslatorSourceCodeAsync()
        {
            var tcs = new TaskCompletionSource<Class>();
            tcs.SetResult(null);
            return tcs.Task;
        }
    }
}