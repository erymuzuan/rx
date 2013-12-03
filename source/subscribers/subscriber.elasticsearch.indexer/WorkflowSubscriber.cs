using Bespoke.Sph.Domain;

namespace Bespoke.Sph.ElasticSearch
{
    public class WorkflowSubscriber : EsEntityIndexer<Workflow>
    {
        protected override string GetTypeName(Workflow item)
        {
            return string.Format("{0}_{1}_{2}", base.GetTypeName(item), item.WorkflowDefinitionId, item.Version);
        }
    }
}