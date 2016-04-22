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
    public class TrackerIndexer : IDisposable
    {
        private readonly HttpClient m_client;

        public TrackerIndexer(HttpClient client)
        {
            m_client = client;
        }
        public async Task ProcessMessage(Tracker item, MessageHeaders headers)
        {
            var tasks = from ea in item.ExecutedActivityCollection
                        let id = $"{item.WorkflowDefinitionId}_{item.WorkflowId}_{ea.ActivityWebId}"
                        let wfid = item.WorkflowId
                        select AddExecutedActivityToIndexAsync(id, ea, headers, wfid);
            await Task.WhenAll(tasks);
            await AddPendingTaskAsync(item);
        }

        private async Task AddPendingTaskAsync(Tracker tracker)
        {
            var context = new SphDataContext();
            tracker.Workflow = await context.LoadOneAsync<Workflow>(w => w.Id == tracker.WorkflowId);
            await tracker.Workflow.LoadWorkflowDefinitionAsync();
            tracker.WorkflowDefinition = tracker.Workflow.WorkflowDefinition;

            var pendings = (from w in tracker.WaitingAsyncList.Keys
                            let act = tracker.WorkflowDefinition.GetActivity<Activity>(w)
                            let screen = act as ReceiveActivity
                            // NOTE : only consider the one with correlation
                            where tracker.WaitingAsyncList[w].Count > 0
                            select new PendingTask(tracker.WorkflowId, tracker.WorkflowDefinitionId)
                            {
                                ActivityName = act.Name,
                                Type = act.GetType().Name,
                                ActivityWebId = act.WebId,
                                Correlations = tracker.WaitingAsyncList[w].ToArray()
                            }).ToList();

     
            //delete previous pending tasks
            var url1 = $"{ConfigurationManager.ElasticSearchIndex}/pendingtask/{"_query?q=WorkflowId:" + tracker.WorkflowId}";
            var response1 = await m_client.DeleteAsync(url1);

            Debug.WriteLine(response1);
            var tasks = from t in pendings
                        let id = $"{tracker.WorkflowDefinitionId}_{tracker.WorkflowId}_{t.ActivityWebId}"
                        select this.AddPendingTaskToIndexAsync(id, t);
            await Task.WhenAll(tasks);

        }

        private async Task AddPendingTaskToIndexAsync(string id, PendingTask ea)
        {
            var setting = new JsonSerializerSettings();
            var json = JsonConvert.SerializeObject(ea, setting);
            var content = new StringContent(json);

            var url = $"{ConfigurationManager.ElasticSearchHost}/{ConfigurationManager.ElasticSearchIndex}/pendingtask/{id}";
            var response = await m_client.PutAsync(url, content);

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

            var url = $"{ConfigurationManager.ElasticSearchIndex}/activity/{id}";


            HttpResponseMessage response = null;
            switch (headers.Crud)
            {
                case CrudOperation.Added:
                    response = await m_client.PutAsync(url, content);
                    break;
                case CrudOperation.Changed:
                    response = await m_client.PostAsync(url, content);
                    break;
                case CrudOperation.Deleted:
                    response = await m_client.DeleteAsync(url);
                    break;

            }

            if (null != response)
            {
                Debug.Write(".");
            }

        }

        public void Dispose()
        {
            m_client?.Dispose();
        }
    }
}