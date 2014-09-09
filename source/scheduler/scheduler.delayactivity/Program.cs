using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace scheduler.delayactivity
{
    class Program
    {
        static void Main(string[] args)
        {
            var webId = args[0];
            var instanceId = args[1];

            var program = new Program();
            program.ExecuteStepAsync(webId, instanceId)
                .Wait();
            var ts = ObjectBuilder.GetObject<ITaskScheduler>();
            ts.DeleteAsync(new ScheduledActivityExecution {  ActivityId = webId, InstanceId = instanceId })
                .Wait();

        }


        public async Task<ActivityExecutionResult> ExecuteStepAsync(string webId, string instanceId)
        {
            var context = new SphDataContext();
            var wf = await context.LoadOneAsync<Workflow>(w => w.Id == instanceId);

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var doc = await store.GetContentAsync(string.Format("wd.{0}.{1}", wf.WorkflowDefinitionId, wf.Version));
            using (var stream = new MemoryStream(doc.Content))
            {
                wf.WorkflowDefinition = stream.DeserializeFromXml<WorkflowDefinition>();
            }

            var result = await wf.ExecuteAsync(webId);
            return result;
        }
    }
}
