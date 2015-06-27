using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace subscriber.version.control
{
    public class EntityDefinitionSourceProvider : SourceProvider<EntityDefinition>
    {
        public override async Task ProcessItem(EntityDefinition item)
        {
            var wc = ConfigurationManager.SphSourceDirectory;
            var type = item.GetType();
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var ed = item;
            var file = Path.Combine(folder, ed.Id + ".json");
            File.WriteAllText(file, item.ToJsonString(Formatting.Indented));

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            if (string.IsNullOrWhiteSpace(ed.IconStoreId)) return;

            var icon = await store.GetContentAsync(item.IconStoreId);
            if (null == icon) return;
            var png = Path.Combine(folder, item.Id + icon.Extension);
            File.WriteAllBytes(png, icon.Content);

        }

    }
}