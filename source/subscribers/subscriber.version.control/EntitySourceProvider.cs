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
            var wc = ConfigurationManager.WorkflowSourceDirectory;
            var type = item.GetType();
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            dynamic wi = item;
            var file = Path.Combine(folder, wi.Name + ".json");
            File.WriteAllText(file, item.ToJsonString(Formatting.Indented));
            return Task.FromResult(0);
        }
    }
}