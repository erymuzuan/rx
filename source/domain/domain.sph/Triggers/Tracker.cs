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
        }

        public bool CanExecute(string webid)
        {
            if(this.ForbiddenActivities.Contains(webid)) return false;
            if (string.IsNullOrWhiteSpace(this.Workflow.State)) return true;
            if (this.Workflow.State != "WaitingAsync") return false;
            return true;
        }
    }
}