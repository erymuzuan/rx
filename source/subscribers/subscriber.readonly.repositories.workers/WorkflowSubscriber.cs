using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ReadOnlyRepositoriesWorkers
{
    public class WorkflowIndexer
    {
        protected string GetTypeName(Workflow item)
        {
            return $"Workflow_{item.WorkflowDefinitionId}_{item.Version}";
        }
    }
}