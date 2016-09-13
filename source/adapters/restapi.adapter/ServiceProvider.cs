using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class ServiceProvider
    {
        [ImportMany(typeof(IEndpointsBuilder))]
        public IEndpointsBuilder[] Builders;

        private static bool m_initialized;

        public static void Init()
        {
            if (m_initialized) return;
            var sp = new ServiceProvider();
            ObjectBuilder.ComposeMefCatalog(sp);
            ObjectBuilder.AddCacheList(sp);

            m_initialized = true;
        }
    }
}