using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace subscriber.version.control
{
    public class DocumentTemplateSourceProvider: SourceProvider<DocumentTemplate>
    {
        public override async Task ProcessItem(DocumentTemplate item)
        {
            var wc = ConfigurationManager.SphSourceDirectory;
            var type = item.GetType();
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            dynamic wi = item;
            var file = Path.Combine(folder, wi.Name + ".json");
            File.WriteAllText(file, item.ToJsonString(Formatting.Indented));

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var word = await store.GetContentAsync(item.WordTemplateStoreId);
            var xsd = Path.Combine(folder, wi.Name + ".docx");
            File.WriteAllBytes(xsd, word.Content);

        }

    }
}