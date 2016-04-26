using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class QueryEndpointProvider : EntityDefinitionItemsProviders<QueryEndpoint>
    {
        protected override string Icon => "fa fa-cloud-download";
        protected override string GetUrl(QueryEndpoint item) => $"query.endpoint.designer/{item.Id}";
    }
}