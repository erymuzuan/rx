using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace scheduler.delayactivity
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var actWebId = args[0];
            var instanceId = args[1];

            var program = new Program();
            program.ExecuteStepAsync(actWebId, instanceId)
                .Wait();
            var ts = ObjectBuilder.GetObject<ITaskScheduler>();
            ts.DeleteAsync(new ScheduledActivityExecution {  ActivityId = actWebId, InstanceId = instanceId })
                .Wait();

        }


        public async Task<ActivityExecutionResult> ExecuteStepAsync(string webId, string instanceId)
        {
            var context = new SphDataContext();
            var wf = await context.LoadOneAsync<Workflow>(w => w.Id == instanceId);
            await wf.LoadWorkflowDefinitionAsync();

            var result = await wf.ExecuteAsync(webId);
            return result;
        }
    }
}
