using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class CorrelationSet: DomainObject
    {

        public async Task<Workflow> GetWorkflowInstanceAsync(WorkflowDefinition wd)
        {
            var context = new SphDataContext();
            var wf = await context.LoadOneAsync<Workflow>(x => x.Id == "");

            return wf;

        }
    }
}