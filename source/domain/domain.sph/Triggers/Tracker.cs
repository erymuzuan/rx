using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class Tracker : Entity
    {
        private Dictionary<string, List<string>> m_waitingAsyncList = new Dictionary<string, List<string>>();

        [XmlIgnore]
        [JsonIgnore]
        public WorkflowDefinition WorkflowDefinition { get; set; }

        private Dictionary<string, List<string>> m_waitingJoinList = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> WaitingJoinList
        {
            get { return m_waitingJoinList; }
            set { m_waitingJoinList = value; }
        }
        private Dictionary<string, List<string>> m_firedJoinList = new Dictionary<string, List<string>>();
        public Dictionary<string, List<string>> FiredJoinList
        {
            get { return m_firedJoinList; }
            set { m_firedJoinList = value; }
        }

        public void AddJoinWaitingList(string join, string predecessor)
        {
            if (this.WaitingJoinList.ContainsKey(join))
            {
                this.WaitingJoinList[join].Add(predecessor);
                // TODO: also initiate the async activity waiting list, still missing correlation thou, should get one from parallel up top
                this.WaitingAsyncList.Add(join, new List<string>());
            }
            else
            {
                this.WaitingJoinList.Add(join, new List<string> { predecessor });
            }

        }

        public void AddFiredJoin(string join, string predecessor)
        {

            if (this.FiredJoinList.ContainsKey(join))
            {
                this.FiredJoinList[join].Add(predecessor);
            }
            else
            {
                this.FiredJoinList.Add(join, new List<string> { predecessor });
            }
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
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(this);
                await session.SubmitChanges("SaveTracker");
            }
        }


        public Dictionary<string, List<string>> WaitingAsyncList
        {
            get { return m_waitingAsyncList; }
            set { m_waitingAsyncList = value; }
        }

        [XmlIgnore]
        [JsonIgnore]
        public Workflow Workflow { get; set; }


        public void Init(Workflow wf, WorkflowDefinition wd)
        {
            this.WorkflowDefinition = wd;
            this.Workflow = wf;

        }

        public override int GetId()
        {
            return this.TrackerId;
        }

        public string GetState(string activityId)
        {
            return null;
        }

        public void AddInitiateActivity(Activity act, InitiateActivityResult result)
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
                Initiated = DateTime.UtcNow
            };
            if (WaitingAsyncList.ContainsKey(act.WebId))
            {
                var waiting = this.WaitingAsyncList[act.WebId];
                waiting.Add(result.Correlation);
            }
            else
            {
                this.WaitingAsyncList.Add(act.WebId, new List<string> { result.Correlation });
            }


            this.ExecutedActivityCollection.Add(ea);
        }

        public void AddExecutedActivity(Activity act, string correlation = null)
        {
            if (!this.ForbiddenActivities.Contains(act.WebId))
                this.ForbiddenActivities.Add(act.WebId);
            var directory = ObjectBuilder.GetObject<IDirectoryService>();


            var ea = this.ExecutedActivityCollection.SingleOrDefault(e => e.ActivityWebId == act.WebId);
            if (null != ea)
            {
                ea.Run = DateTime.UtcNow;
                return;
            }
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
                ea.Initiated = DateTime.UtcNow;
            else
                ea.Run = DateTime.UtcNow;

            this.ExecutedActivityCollection.Add(ea);

            // remove the waiting list
            if (act.IsAsync && !act.IsInitiator)
            {
                var waiting = this.WaitingAsyncList[act.WebId];
                waiting.Remove(correlation);
            }
        }

        public bool CanExecute(string webid, string correlation)
        {
            if (this.Workflow.State == "Completed") return false;
            if (this.Workflow.State == "Terminated") return false;
            // TODO : determine all the legal states
            var act = this.WorkflowDefinition.GetActivity<Activity>(webid);
            if (act.IsAsync)
            {
                if (!this.WaitingAsyncList.ContainsKey(webid)) return false;
                var waiting = this.WaitingAsyncList[webid];
                return waiting.Contains(correlation);
            }
            return true;
        }

    }
}