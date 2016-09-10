using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class HttpHeader : DomainObject
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}