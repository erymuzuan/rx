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
            SaveJsonSource(item);
            return Task.FromResult(0);
        }

        public override Task RemoveItem(ReportDefinition item)
        {
            this.RemoveJsonSource(item);
            return Task.FromResult(0);
        }

    }
}