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

            var ea = new ExecutedActivity
            {
                WorkflowDefinitionId = this.WorkflowDefinitionId,
                ActivityWebId = act.WebId,
                InstanceId = this.WorkflowId,
                User = "TODO: get user from activity",
                
                
            };
            this.ExecutedActivityCollection.Add(ea);
        }

        public bool CanExecute(string webid)
        {
            // TODO : determine all the legal states
            return true;
        }
    }
}