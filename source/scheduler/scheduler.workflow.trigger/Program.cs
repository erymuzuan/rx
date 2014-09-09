using System.Threading.Tasks;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WorkflowTriggerSchedulers
{
    class Program
    {
        static void Main(string[] args)
        {
            var webId = args[0];
            var instanceId = args[1];

            var program = new Program();
            program.InitiateWorkflowAsync(webId, instanceId).Wait();

        }

        public async Task InitiateWorkflowAsync(string activityid, string id)
        {
            var context = new SphDataContext();
            var wd = await context.LoadOneAsync<WorkflowDefinition>(w => w.Id == id);
            var wf = await wd.InitiateAsync();
            await wf.StartAsync();
        }

    }
}
