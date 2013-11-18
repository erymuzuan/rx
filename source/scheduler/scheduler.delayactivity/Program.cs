using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace scheduler.delayactivity
{
    class Program
    {
        static void Main(string[] args)
        {
            var webId = args[0];
            var instanceId = int.Parse(args[1]);

            var program = new Program();
            program.ExecuteStepAsync(webId, instanceId)
                .Wait();
            var ts = ObjectBuilder.GetObject<ITaskScheduler>();
            ts.DeleteAsync(new ScheduledActivityExecution { ActivityId = webId, InstanceId = instanceId })
                .Wait();

        }

        public async Task<ActivityExecutionResult> ExecuteStepAsync(string webId, int instanceId)
        {
            var context = new SphDataContext();
            var wf = await context.LoadOneAsync<Workflow>(w => w.WorkflowId == instanceId);
            var result = await wf.ExecuteAsync(webId);
            return result;
        }
    }
}
