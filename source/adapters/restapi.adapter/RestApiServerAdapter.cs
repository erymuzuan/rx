using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Domain.Codes;

namespace Bespoke.Sph.Integrations.Adapters
{
    [EntityType(typeof(Adapter))]
    [Export("AdapterDesigner", typeof(Adapter))]
    [DesignerMetadata(Name = "REST Api", FontAwesomeIcon = "gg",
         RouteTableProvider = typeof(RestApiAdapterRouteProvider), Route = "adapter.restapi/0")]
    public partial class RestApiServerAdapter : Adapter
    {
        public RestApiServerAdapter()
        {
            
        }
        public RestApiServerAdapter(string odataTranslator)
        {
            OdataTranslator = odataTranslator;
        }

        public override string OdataTranslator { get; }
        protected override Task<IEnumerable<Class>> GenerateSourceCodeAsync(CompilerOptions options, params string[] namespaces)
        {
            throw new System.NotImplementedException();
        }

        protected override Task<Class> GenerateOdataTranslatorSourceCodeAsync()
        {
            throw new System.NotImplementedException();
        }

        protected override Task<Class> GeneratePagingSourceCodeAsync()
        {
            throw new System.NotImplementedException();
        }

        protected override Task<TableDefinition> GetSchemaDefinitionAsync(string table)
        {
            throw new System.NotImplementedException();
        }
    }
}