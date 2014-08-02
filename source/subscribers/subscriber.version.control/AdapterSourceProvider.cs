using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Newtonsoft.Json;

namespace subscriber.version.control
{
    public class AdapterSourceProvider : SourceProvider<Adapter>
    {
        public override Task ProcessItem(Adapter item)
        {
            var wc = ConfigurationManager.WorkflowSourceDirectory;
            var type = item.GetType();
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            dynamic wi = item;
            var file = Path.Combine(folder, wi.Name + ".json");
            File.WriteAllText(file, item.ToJsonString(Formatting.Indented));

          // other assets, these are specific to each adapters
            item.SaveAssets();


            return Task.FromResult(0);

        }
    }
}