using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bespoke.Sph.Integrations.Adapters
{
    public class EndpointsBuilderFactory
    {
        public async Task<IEnumerable<IEndpointsBuilder>> CreateAsync(ServiceProvider serviceProvider, string storeId)
        {
            var builders = new List<IEndpointsBuilder>();
            foreach (var b in serviceProvider.Builders)
            {
                b.StoreId = storeId;
                var list = await b.GetBuildersAsync();
                builders.AddRange(list);
            }

            return builders.OrderBy(x => x.Order);
        }
    }
}