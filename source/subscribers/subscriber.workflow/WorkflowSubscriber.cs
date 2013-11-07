using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace Bespoke.Sph.WorkflowsExecution
{
    public class WorkflowSubscriber : Subscriber<Workflow>
    {
        public override string QueueName
        {
            get { return "workflow_execution"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { "Workflow.*" }; }
        }

        protected async override Task ProcessMessage(Workflow item, MessageHeaders header)
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var doc = await store.GetContentAsync(item.SerializedDefinitionStoreId);

            WorkflowDefinition wd;
            using (var stream = new MemoryStream(doc.Content))
            {
                wd = stream.DeserializeFromXml<WorkflowDefinition>();
            }
            item.WorkflowDefinition = wd;

            // get current activity
            var initiator = wd.ActivityCollection.Single(a => a.IsInitiator);
            var activityId = header.Operation;

            if (initiator.WebId == activityId)
            {
                Console.WriteLine("started");
            }

            var result = await item.ExecuteAsync();
            Console.WriteLine(result);



        }

    }
}
