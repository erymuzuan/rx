using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Solutions
{
    [Export(typeof(IItemsProvider))]
    public class OperationEndpointProvider : EntityDefinitionItemsProviders<OperationEndpoint>
    {
        protected override string Icon => "fa fa-pencil";
        protected override string GetUrl(OperationEndpoint item) => $"operation.endpoint.designer/{item.Id}";
    }
}