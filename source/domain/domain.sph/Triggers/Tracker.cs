using System;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class Tracker : Entity
    {
        [XmlIgnore]
        [JsonIgnore]
        public WorkflowDefinition WorkflowDefinition { get; set; }

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

        public void AddExecutedActivity(Activity act)
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
        }

        public bool CanExecute(string webid)
        {
            // TODO : determine all the legal states
            return true;
        }

    }
}