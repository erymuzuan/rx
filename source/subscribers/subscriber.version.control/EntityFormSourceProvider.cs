using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace subscriber.version.control
{
    public class EntityFormSourceProvider : SourceProvider<EntityForm>
    {
        public override async Task ProcessItem(EntityForm item)
        {
            var wc = ConfigurationManager.WorkflowSourceDirectory;
            var type = item.GetType();
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var form = item;
            var file = Path.Combine(folder, form.Name + ".json");
            File.WriteAllText(file, item.ToJsonString(Formatting.Indented));

            if (string.IsNullOrWhiteSpace(form.IconStoreId)) return;
            var store = ObjectBuilder.GetObject<IBinaryStore>();

            var icon = await store.GetContentAsync(item.IconStoreId);
            if (null == icon) return;

            var png = Path.Combine(folder, item.Name + icon.Extension);
            File.WriteAllBytes(png, icon.Content);

        }

    }
}