using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace subscriber.version.control
{
    public class EntityViewSourceProvider: SourceProvider<EntityView>
    {
        public override async Task ProcessItem(EntityView view)
        {
            var wc = ConfigurationManager.SphSourceDirectory;
            var type = view.GetType();
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var vw = view;
            var file = Path.Combine(folder, vw.Id + ".json");
            File.WriteAllText(file, view.ToJsonString(Formatting.Indented));

            if (string.IsNullOrWhiteSpace(vw.IconStoreId)) return;
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var icon = await store.GetContentAsync(view.IconStoreId);
            if (null == icon) return;
            var png = Path.Combine(folder, view.Id + icon.Extension);
            File.WriteAllBytes(png, icon.Content);

        }

    }
}