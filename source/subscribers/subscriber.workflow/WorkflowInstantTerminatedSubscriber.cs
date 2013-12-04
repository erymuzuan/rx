using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace Bespoke.Sph.WorkflowsExecution
{
    public class WorkflowInstantTerminatedSubscriber : Subscriber<Workflow>
    {
        public override string QueueName
        {
            get { return "workflow_terminated"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { "Workflow.changed" }; }
        }

        protected async override Task ProcessMessage(Workflow item, MessageHeaders header)
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var doc = await store.GetContentAsync(string.Format("wd.{0}.{1}", item.WorkflowDefinitionId, item.Version));

            if (item.State == "Completed") return;

            WorkflowDefinition wd;
            using (var stream = new MemoryStream(doc.Content))
            {
                wd = stream.DeserializeFromXml<WorkflowDefinition>();
            }
            item.WorkflowDefinition = wd;


            wd.ActivityCollection.ForEach(async a => await a.TerminateAsync(item));

        }
    }
}