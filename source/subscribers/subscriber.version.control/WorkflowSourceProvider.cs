using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace subscriber.version.control
{
    public class WorkflowSourceProvider : SourceProvider<WorkflowDefinition>
    {
        public override async Task ProcessItem(WorkflowDefinition item)
        {
            base.SaveJsonSource(item);
            await PersistDocumentAsync(item.SchemaStoreId);

        }
    }
}