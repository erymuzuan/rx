using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticSearch
{
    public class WorkflowIndexer
    {
        protected string GetTypeName(Workflow item)
        {
            return string.Format("{0}_{1}_{2}", "Workflow", item.WorkflowDefinitionId, item.Version);
        }
    }
}