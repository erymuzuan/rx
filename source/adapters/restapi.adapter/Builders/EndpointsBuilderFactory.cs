using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class EndpointsBuilderFactory
    {
        private class BuilderComparer : IEqualityComparer<IEndpointsBuilder>
        {
            public bool Equals(IEndpointsBuilder x, IEndpointsBuilder y)
            {
                return x.GetType() == y.GetType();
            }

            public int GetHashCode(IEndpointsBuilder obj)
            {
                return obj.GetType().FullName.GetHashCode();
            }
        }
        public async Task<IEnumerable<IEndpointsBuilder>> CreateAsync(ServiceProvider serviceProvider, string storeId)
        {
            var builders = new List<IEndpointsBuilder>();
            foreach (var b in serviceProvider.Builders)
            {
                b.StoreId = storeId;
                var list = await b.GetBuildersAsync();
                builders.AddRange(list);
            }

            return builders.Distinct(new BuilderComparer()).OrderBy(x => x.Order).ToArray();
        }
    }
}