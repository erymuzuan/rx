using System;
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
            get { return new[] { "workflow.*" }; }
        }

        protected async override Task ProcessMessage(Workflow item, MessageHeaders header)
        {
            var context = new SphDataContext();
            var wd =
                await context.LoadOneAsync<WorkflowDefinition>(w => w.WorkflowDefinitionId == item.WorkflowDefinitionId);
            // get current activity
            var initiator = wd.ActivityCollection.Single(a => a.IsInitiator);
            var activityId = header.Operation;
            
            if (initiator.WebId == activityId)
            {
                Console.WriteLine("started");
            }

            var next = wd.GetNextActivity(activityId);
            if (null == next) return;

            var result = await next.ExecuteAsync();
            Console.WriteLine(result);

           

        }

    }
}
