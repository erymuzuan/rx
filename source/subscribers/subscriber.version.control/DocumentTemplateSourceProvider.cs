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

    }
}