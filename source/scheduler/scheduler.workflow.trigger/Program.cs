using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WorkflowTriggerSchedulers
{
    class Program
    {
        static void Main(string[] args)
        {
            var webId = args[0];
            var instanceId = int.Parse(args[1]);

            var program = new Program();
            program.InitiateWorkflowAsync(webId, instanceId).Wait();

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
