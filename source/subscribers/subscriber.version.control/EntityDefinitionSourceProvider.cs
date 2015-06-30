using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace subscriber.version.control
{
    public class EntityDefinitionSourceProvider : SourceProvider<EntityDefinition>
    {
        public override async Task ProcessItem(EntityDefinition item)
        {
            this.SaveJsonSource(item);
            await PersistDocumentAsync(item.IconStoreId);

        }

        public override async Task RemoveItem(EntityDefinition item)
        {
            this.RemoveJsonSource(item);
            await this.RemoveDocumentAsync(item.IconStoreId);
        }

    }
}