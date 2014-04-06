using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace subscriber.version.control
{
    public class ReportDefinitionSourceProvider : SourceProvider<ReportDefinition>
    {
        public override Task ProcessItem(ReportDefinition item)
        {
            var wc = ConfigurationManager.WorkflowSourceDirectory;
            var type = item.GetType();
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var file = Path.Combine(folder, item.Title + ".json");
            File.WriteAllText(file, item.ToJsonString(Formatting.Indented));

            return Task.FromResult(0);
        }
    }
}