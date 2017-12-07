using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace Bespoke.Sph.ReadOnlyRepositoriesWorkers
{
    public class TrackerIndexer
    {
        public async Task ProcessMessage(Tracker item, MessageHeaders headers)
        {
            var ws = ObjectBuilder.GetObject<IWorkflowService>();
            var tasks = from ea in item.ExecutedActivityCollection
                        let id = $"{item.WorkflowDefinitionId}_{item.WorkflowId}_{ea.ActivityWebId}"
                        let wfid = item.WorkflowId
                        select ws.AddExecutedActivityAsync(id, ea, headers.Crud.ToString(), wfid);
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


            var ws = ObjectBuilder.GetObject<IWorkflowService>();
            await ws.DeletePendingTasksAsync(tracker.WorkflowId);


            var tasks = from t in pendings
                        let id = $"{tracker.WorkflowDefinitionId}_{tracker.WorkflowId}_{t.ActivityWebId}"
                        select ws.AddPendingTaskAsync(id, t);
            await Task.WhenAll(tasks);

        }

    }
}