using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Bespoke.Sph.Domain
{
    public partial class CorrelationSet: DomainObject
    {

        public async Task<Workflow> GetWorkflowInstanceAsync(WorkflowDefinition wd)
        {
            var query = "{}" + "" + this.Name;
            var content = new StringContent(query);

            using (var client = new HttpClient{BaseAddress =new Uri(ConfigurationManager.ElasticSearchHost)})
            {
                var response = await client.PostAsync(new Uri(""), content);

            }

            var context = new SphDataContext();
            var wf = await context.LoadOneAsync<Workflow>(x => x.Id == "");

            return wf;

        }
    }
}