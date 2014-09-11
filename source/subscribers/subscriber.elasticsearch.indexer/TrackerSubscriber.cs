using System;
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
                        select AddExecutedActivityToIndexAsync(id, ea, headers, wfid);
            await Task.WhenAll(tasks);
            await AddPendingTaskAsync(item);
        }

        private async Task AddPendingTaskAsync(Tracker item)
        {
            var context = new SphDataContext();
            item.Workflow = await context.LoadOneAsync<Workflow>(w => w.Id == item.WorkflowId);
            await item.Workflow.LoadWorkflowDefinitionAsync();
            item.WorkflowDefinition = item.Workflow.WorkflowDefinition;

            var pendings = (from w in item.WaitingAsyncList.Keys
                            let act = item.WorkflowDefinition.GetActivity<Activity>(w)
                            let screen = act as ScreenActivity
                            // NOTE : only consider the one with correlation
                            where item.WaitingAsyncList[w].Count > 0
                            select new PendingTask(item.WorkflowId)
                            {
                                Name = act.Name,
                                Type = act.GetType().Name,
                                WebId = act.WebId,
                                Correlations = item.WaitingAsyncList[w].ToArray()
                            }).ToList();

            pendings.ForEach(async t =>
            {
                var screen = item.WorkflowDefinition.ActivityCollection
                    .OfType<ScreenActivity>()
                    .SingleOrDefault(a => a.WebId == t.WebId);
                if (null != screen)
                    t.Performers = await screen.GetUsersAsync(item.Workflow);
            });
            //delete previous pending tasks
            var url1 = string.Format("{0}/{1}/{2}/{3}", ConfigurationManager.ElasticSearchHost, ConfigurationManager.ElasticSearchIndex, "pendingtask",
                "_query?q=WorkflowId:" + item.WorkflowId);
            var client1 = new HttpClient();
            var response1 = await client1.DeleteAsync(url1);

            Debug.WriteLine(response1);
            var tasks = from t in pendings
                        let id = string.Format("{0}_{1}_{2}", item.WorkflowDefinitionId, item.WorkflowId, t.WebId)
                        select this.AddPendingTaskToIndexAsync(id, t);
            await Task.WhenAll(tasks);

        }

        private async Task AddPendingTaskToIndexAsync(string id, PendingTask ea)
        {
            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(ea, setting);
            var content = new StringContent(json);


            var url = string.Format("{0}/{1}/{2}/{3}", ConfigurationManager.ElasticSearchHost, ConfigurationManager.ElasticSearchIndex, "pendingtask", id);
            var client = new HttpClient();
            var response = await client.PutAsync(url, content);


            if (null != response)
            {
                Debug.Write(".");
            }
        }
        private async Task AddExecutedActivityToIndexAsync(string id, ExecutedActivity ea, MessageHeaders headers, string wfid)
        {
            ea.InstanceId = wfid;
            var setting = new JsonSerializerSettings();

            var json = JsonConvert.SerializeObject(ea, setting);
            var content = new StringContent(json);

            var url = string.Format("{0}/{1}/{2}", ConfigurationManager.ElasticSearchIndex, "activity", id);

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
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
}