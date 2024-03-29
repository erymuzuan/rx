using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace subscriber.version.control
{
    public class EntityViewSourceProvider : SourceProvider<EntityView>
    {
        public override async Task ProcessItem(EntityView view)
        {
            SaveJsonSource(view);
            await PersistDocumentAsync(view.IconStoreId);

        }


        public override async Task RemoveItem(EntityView item)
        {
            this.RemoveJsonSource(item);
            await this.RemoveDocumentAsync(item.IconStoreId);
        }


    }
}