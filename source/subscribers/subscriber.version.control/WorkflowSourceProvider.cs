using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace subscriber.version.control
{
    public class WorkflowSourceProvider : SourceProvider<WorkflowDefinition>
    {
        public override async Task ProcessItem(WorkflowDefinition item)
        {
            base.SaveJsonSource(item);
            var file = $"wd.{item.Id}.{item.Version}";
            await PersistDocumentAsync(file);
            await PersistDocumentAsync(item.SchemaStoreId);

        }

        public override async Task RemoveItem(WorkflowDefinition item)
        {
            this.RemoveJsonSource(item);
            var file = $"wd.{item.Id}.{item.Version}";
            await PersistDocumentAsync(file);
            await this.RemoveDocumentAsync(item.SchemaStoreId);
        }

    }
}