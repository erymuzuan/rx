using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace subscriber.version.control
{
    public class DocumentTemplateSourceProvider: SourceProvider<DocumentTemplate>
    {
        public override async Task ProcessItem(DocumentTemplate item)
        {
            this.SaveJsonSource(item);
            await PersistDocumentAsync(item.WordTemplateStoreId);

        }


        public override async Task RemoveItem(DocumentTemplate item)
        {
            this.RemoveJsonSource(item);
            await this.RemoveDocumentAsync(item.WordTemplateStoreId);
        }

    }
}