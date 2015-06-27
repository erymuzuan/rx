using System.Threading.Tasks;
using Bespoke.Sph.Domain.Api;

namespace subscriber.version.control
{
    public class AdapterSourceProvider : SourceProvider<Adapter>
    {
        public override Task ProcessItem(Adapter item)
        {
            SaveJsonSource(item);
            // other assets, these are specific to each adapters
            item.SaveAssets();
            return Task.FromResult(0);

        }
    }
}