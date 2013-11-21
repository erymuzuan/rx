using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace scheduler.delayactivity
{
    class Program
    {
        static void Main(string[] args)
        {
            Start();
            var webId = args[0];
            var instanceId = int.Parse(args[1]);

            var program = new Program();
            program.ExecuteStepAsync(webId, instanceId)
                .Wait();
            var ts = ObjectBuilder.GetObject<ITaskScheduler>();
            ts.DeleteAsync(new ScheduledActivityExecution { ActivityId = webId, InstanceId = instanceId })
                .Wait();

        }


        public static void Start()
        {
            var files = Directory.GetFiles(@".\", "workflows.*.dll");
            foreach (var s in files)
            {
                Console.WriteLine(s);
                var types = Assembly.LoadFrom(s).GetTypes().Where(t => t.BaseType == typeof(Workflow)).ToList();
                types.ForEach(t => XmlSerializerService.RegisterKnownTypes(typeof(Workflow), t));

            }
        }

        public async Task<ActivityExecutionResult> ExecuteStepAsync(string webId, int instanceId)
        {
            var context = new SphDataContext();
            var wf = await context.LoadOneAsync<Workflow>(w => w.WorkflowId == instanceId);

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
