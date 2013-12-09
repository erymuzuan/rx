using System;
using System.Collections.Generic;
using System.Linq;
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
            if (act.IsAsync)
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