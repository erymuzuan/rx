using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WorkflowTriggerSchedulers
{
    class Program
    {
        static void Main(string[] args)
        {
            Start();
            var webId = args[0];
            var instanceId = int.Parse(args[1]);

            var program = new Program();
            program.InitiateWorkflowAsync(webId, instanceId).Wait();

        }

        public static void Start()
        {
            var files = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "workflows.*.dll");
            foreach (var s in files)
            {
                Console.WriteLine(s);
                var types = Assembly.LoadFrom(s).GetTypes().Where(t => t.BaseType == typeof(Workflow)).ToList();
                types.ForEach(t => XmlSerializerService.RegisterKnownTypes(typeof(Workflow), t));

            }
        }

        public async Task InitiateWorkflowAsync(string activityid, int id)
        {
            var context = new SphDataContext();
            var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.WorkflowDefinitionId == id);
            var wf = await wd.InitiateAsync();
            await wf.StartAsync();
        }

    }
}
