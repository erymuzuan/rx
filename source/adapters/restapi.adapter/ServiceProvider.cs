using System.ComponentModel.Composition;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class ServiceProvider
    {
        [ImportMany(typeof(IEndpointsBuilder))]
        public IEndpointsBuilder[] Builders;
    }
}