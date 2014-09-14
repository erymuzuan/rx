using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace subscriber.version.control
{
    public class EntityViewSourceProvider: SourceProvider<EntityView>
    {
        public override async Task ProcessItem(EntityView item)
        {
            var wc = ConfigurationManager.SphSourceDirectory;
            var type = item.GetType();
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var vw = item;
            var file = Path.Combine(folder, vw.Name + ".json");
            File.WriteAllText(file, item.ToJsonString(Formatting.Indented));

            if (string.IsNullOrWhiteSpace(vw.IconStoreId)) return;
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var icon = await store.GetContentAsync(item.IconStoreId);
            if (null == icon) return;
            var png = Path.Combine(folder, item.Name + icon.Extension);
            File.WriteAllBytes(png, icon.Content);

        }

    }
}