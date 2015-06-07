using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticSearch
{
    public class WorkflowIndexer
    {
        protected string GetTypeName(Workflow item)
        {
            return $"{"Workflow"}_{item.WorkflowDefinitionId}_{item.Version}";
        }
    }
}