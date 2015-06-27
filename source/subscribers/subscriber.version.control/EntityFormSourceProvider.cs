using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace subscriber.version.control
{
    public class EntityFormSourceProvider : SourceProvider<EntityForm>
    {
        public override async Task ProcessItem(EntityForm form)
        {
            base.SaveJsonSource(form);
            await PersistDocumentAsync(form.IconStoreId);
            

        }

    }
}