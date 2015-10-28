using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace subscriber.version.control
{
    public class EntitySourceProvider : SourceProvider<Entity>
    {
        public override Task ProcessItem(Entity item)
        {
            var wc = ConfigurationManager.SphSourceDirectory;
            var type = item.GetEntityType();
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var file = Path.Combine(folder, item.Id + ".json");
            File.WriteAllText(file, item.ToJsonString(Formatting.Indented));
            return Task.FromResult(0);
        }

        public override Task RemoveItem(Entity item)
        {
            this.RemoveJsonSource(item);
            return Task.FromResult(0);
        }

    }
}