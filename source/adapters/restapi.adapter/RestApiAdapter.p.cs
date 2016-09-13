using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public partial class RestApiAdapter
    {
        public string BaseAddress { get; set; }
        public string AuthenticationType { get; set; }
        public ObjectCollection<HttpHeader> SecurityHeaderCollection { get; } = new ObjectCollection<HttpHeader>();

        public override string OdataTranslator { get; }
    }
}