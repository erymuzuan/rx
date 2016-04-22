using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class Tracker : Entity
    {
        public Tracker()
        {
            // default for serializer
        }

        public Tracker(WorkflowDefinition wd, Workflow wf)
        {
            this.WorkflowDefinition = wd;
            this.Workflow = wf;
            this.Id = Strings.GenerateId();
            this.WorkflowId = wf.Id;
            this.WorkflowDefinitionId = wf.WorkflowDefinitionId;
        }

        [JsonIgnore]
        public Workflow Workflow { get; set; }
        [JsonIgnore]
        public WorkflowDefinition WorkflowDefinition { get; set; }
        public ConcurrentDictionary<string, List<string>> WaitingAsyncList = new ConcurrentDictionary<string, List<string>>();
        public ConcurrentDictionary<string, List<string>> WaitingJoinList = new ConcurrentDictionary<string, List<string>>();
        public ConcurrentDictionary<string, List<string>> FiredJoinList = new ConcurrentDictionary<string, List<string>>();


        public void AddJoinWaitingList(string join, string predecessor)
        {
            var list = this.WaitingJoinList.GetOrAdd(join, new List<string>());
            list.Add(predecessor);
            this.WaitingJoinList.AddOrUpdate(join, list, (k, l) => list);

            // TODO: also initiate the async activity waiting list, still missing correlation thou, should get one from parallel up top
            this.WaitingAsyncList.TryAdd(join, new List<string>());

        }

        public void AddFiredJoin(string join, string predecessor)
        {
            var list = this.FiredJoinList.GetOrAdd(join, new List<string>());
            list.Add(predecessor);
            this.FiredJoinList.AddOrUpdate(join, list, (k, l) => list);
        }

        public bool AllJoinFired(string join)
        {
            if (!this.WaitingJoinList.ContainsKey(join)) return false;
            if (!this.FiredJoinList.ContainsKey(join)) return false;
            var waiting = this.WaitingJoinList[join];
            var fired = this.FiredJoinList[join];
            return waiting.Count == fired.Count && waiting.All(fired.Contains);
        }

        public async Task SaveAsync()
        {
            this.ExecutedActivityCollection.Sort(new ExecutedAcitivityComparer());
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(this);
                await session.SubmitChanges("SaveTracker");
            }
        }



        public void Init(Workflow wf, WorkflowDefinition wd)
        {
            this.WorkflowDefinition = wd;
            this.Workflow = wf;

        }

        public string GetState(string activityId)
        {
            return null;
        }

        public void AddInitiateActivity(Activity act, InitiateActivityResult result, DateTime time = new DateTime())
        {
            if (!this.ForbiddenActivities.Contains(act.WebId))
                this.ForbiddenActivities.Add(act.WebId);
            var directory = ObjectBuilder.GetObject<IDirectoryService>();
            var ea = new ExecutedActivity
            {
                WorkflowDefinitionId = this.WorkflowDefinitionId,
                ActivityWebId = act.WebId,
                InstanceId = this.WorkflowId,
                User = directory.CurrentUserName,
                Name = act.Name,
                Type = act.GetType().Name,
                Initiated = time == DateTime.MinValue ? DateTime.Now : time
            };
            if (WaitingAsyncList.ContainsKey(act.WebId))
            {
                var waiting = this.WaitingAsyncList[act.WebId];
                waiting.Add(result.Correlation);
            }
            else
            {
                this.WaitingAsyncList.TryAdd(act.WebId, new List<string> { result.Correlation });
            }


            this.ExecutedActivityCollection.Add(ea);
        }

        public void AddExecutedActivity(Activity act, string correlation = null)
        {
            if (!this.ForbiddenActivities.Contains(act.WebId))
                this.ForbiddenActivities.Add(act.WebId);
            var directory = ObjectBuilder.GetObject<IDirectoryService>();


            var ea = this.ExecutedActivityCollection.LastOrDefault(e => e.ActivityWebId == act.WebId);
            if (null != ea)
            {
                ea.Run = DateTime.Now;
                ea.User = directory.CurrentUserName;
                // remove the waiting list
                if (act.IsAsync && !act.IsInitiator)
                {
                    var waiting = this.WaitingAsyncList[act.WebId];
                    waiting.Remove(correlation);
                }
            }
            else
            {
                ea = new ExecutedActivity
                {
                    WorkflowDefinitionId = this.WorkflowDefinitionId,
                    ActivityWebId = act.WebId,
                    InstanceId = this.WorkflowId,
                    User = directory.CurrentUserName,
                    Name = act.Name,
                    Type = act.GetType().Name
                };
                if (this.Workflow.State == "WaitingAsync" && act.IsAsync)
                    ea.Initiated = DateTime.Now;
                else
                    ea.Run = DateTime.Now;

                this.ExecutedActivityCollection.Add(ea);
            }

        }

        public bool CanExecute(string webid, string correlation)
        {
            if (this.Workflow.State == "Completed") return false;
            if (this.Workflow.State == "Terminated") return false;
            // TODO : determine all the legal states
            var act = this.WorkflowDefinition.GetActivity<Activity>(webid);
            if (act.IsInitiator) return true;

            if (act.IsAsync)
            {
                if (!this.WaitingAsyncList.ContainsKey(webid)) return false;
                var waiting = this.WaitingAsyncList[webid];
                return waiting.Contains(correlation);
            }
            return true;
        }

        public async Task<IEnumerable<PendingTask>> GetPendingTasksAsync()
        {
            if (null == this.Workflow) throw new InvalidOperationException("Workflow is null");
            if (null == this.WorkflowDefinition) await this.Workflow.LoadWorkflowDefinitionAsync();
            this.WorkflowDefinition = this.Workflow.WorkflowDefinition;

            var pendings = (from w in this.WaitingAsyncList.Keys
                            let act = this.WorkflowDefinition.GetActivity<Activity>(w)
                            let screen = act as ReceiveActivity
                            select new PendingTask(this.WorkflowId, this.WorkflowDefinitionId)
                            {
                                ActivityName = act.Name,
                                Type = act.GetType().Name,
                                ActivityWebId = act.WebId,
                                Correlations = this.GetCorrellationSetValues()
                            }).ToList();
            return pendings;
        }

        private string[] GetCorrellationSetValues()
        {
            var list = new List<string>();
            if (null != this.Workflow && null != this.WorkflowDefinition)
            {
                list.Add("");
            }
            return list.ToArray();
        }

        /// <summary>
        /// Remove the activity from waiting list, so it cannot be invoke anymore
        /// </summary>
        /// <param name="activityWebId"></param>
        /// <param name="correlation"></param>
        /// <param name="cancelStatus">Set the ExecutedActivity cancel status</param>
        public void CancelAsyncList(string activityWebId, string correlation = null, bool cancelStatus = true)
        {
            if (this.WaitingAsyncList.ContainsKey(activityWebId))
            {
                // TODO : tracker should remove only one correlation
                if (string.IsNullOrWhiteSpace(correlation))
                    this.WaitingAsyncList[activityWebId].Clear();
                else
                    this.WaitingAsyncList[activityWebId].Remove(correlation);

                var ea = this.ExecutedActivityCollection.SingleOrDefault(x => x.ActivityWebId == activityWebId);
                if (ea != null) ea.IsCancelled = true;
            }
        }
    }
}