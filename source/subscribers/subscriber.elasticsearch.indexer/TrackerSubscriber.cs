using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json;

namespace Bespoke.Sph.ElasticSearch
{
    // break down the ExecutionActivity
    public class TrackerSubscriber : EsEntityIndexer<Tracker>
    {
        protected async override Task ProcessMessage(Tracker item, MessageHeaders headers)
        {
            var tasks = from ea in item.ExecutedActivityCollection
                        let id = string.Format("{0}_{1}_{2}", item.WorkflowDefinitionId, item.WorkflowId, ea.ActivityWebId)
                        let wfid = item.WorkflowId
                        select AddToIndexAsync(id, ea, headers,wfid);
            await Task.WhenAll(tasks);
        }

        private async Task AddToIndexAsync(string id, ExecutedActivity ea, MessageHeaders headers, int wfid)
        {
            ea.InstanceId = wfid;
            var setting = new JsonSerializerSettings();

            var json = JsonConvert.SerializeObject(ea, setting);
            var content = new StringContent(json);
            var esServer = ConfigurationManager.AppSettings["es.server"];
            const string index = "sph";


            var url = string.Format("{0}/{1}/{2}/{3}", esServer, index, "activity", id);

            var client = new HttpClient();
            HttpResponseMessage response = null;
            switch (headers.Crud)
            {
                case CrudOperation.Added:
                    response = await client.PutAsync(url, content);
                    break;
                case CrudOperation.Changed:
                    response = await client.PostAsync(url, content);
                    break;
                case CrudOperation.Deleted:
                    response = await client.DeleteAsync(url);
                    break;

            }

            if (null != response)
            {
                Debug.Write(".");
            }
        }
    }
}