using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace Bespoke.Sph.WorkflowsExecution
{
    public class WorkflowInstantTerminatedSubscriber : Subscriber<Workflow>
    {
        public override string QueueName => "workflow_terminated";

        public override string[] RoutingKeys => new[] { "Workflow.changed.Terminate" };

        protected override async Task ProcessMessage(Workflow item, MessageHeaders header)
        {

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var doc = await store.GetContentAsync($"wd.{item.WorkflowDefinitionId}.{item.Version}");

            WorkflowDefinition wd;
            using (var stream = new MemoryStream(doc.Content))
            {
                wd = stream.DeserializeFromJson<WorkflowDefinition>();
            }
            item.WorkflowDefinition = wd;


            wd.ActivityCollection.ForEach(async a => await a.TerminateAsync(item));

        }
    }
}