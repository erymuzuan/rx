using System;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class Workflow : Entity
    {
        [XmlAttribute]
        public string CurrentActivityWebId { get; set; }

        /// <summary>
        /// once a workflow definition is published the copy of the definition is stored for reference with id and version no,
        /// the WorkflowDefinitionId may not be of the correct version anymore
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public WorkflowDefinition WorkflowDefinition { get; set; }

        /// <summary>
        /// Once a workflow definition is published the copy of the definition is stored for reference with id and version no
        /// </summary>
        [XmlAttribute]
        public string SerializedDefinitionStoreId { get; set; }

        public virtual Task<ActivityExecutionResult> StartAsync()
        {
            // run the first one and save
            return Task.FromResult(new ActivityExecutionResult { Status = ActivityExecutionStatus.None });
        }

        public virtual Task<ActivityExecutionResult> ExecuteAsync(string activityId)
        {
            return Task.FromResult(new ActivityExecutionResult { Status = ActivityExecutionStatus.None });
        }

        public virtual Task<ActivityExecutionResult> ExecuteAsync()
        {
            return Task.FromResult(new ActivityExecutionResult { Status = ActivityExecutionStatus.None });
        }

        public Activity GetCurrentActivity()
        {
            if (null == this.WorkflowDefinition)
                throw new InvalidOperationException("You should have set the WorkflowDefinition " + this.WorkflowDefinitionId + " version " + this.Version + " via SerializedDefinitionStoreId property");
            return this.WorkflowDefinition.ActivityCollection.SingleOrDefault(d => d.WebId == this.CurrentActivityWebId);
        }

        public T GetActivity<T>(string webId) where T : Activity
        {
            if (null == this.WorkflowDefinition) throw new InvalidOperationException("You have to load the WorkflowDefinition before calling GetActivity");
            return this.WorkflowDefinition.ActivityCollection.OfType<T>().Single(w => w.WebId == webId);
        }


        public Activity GetNexActivity()
        {
            if (null == this.WorkflowDefinition)
                throw new InvalidOperationException("You should have set the WorkflowDefinition " + this.WorkflowDefinitionId + " version " + this.Version + " via SerializedDefinitionStoreId property");
            var current = string.IsNullOrWhiteSpace(this.CurrentActivityWebId) ? this.WorkflowDefinition.ActivityCollection.Single(d => d.IsInitiator) : null;

            if (null == current) return null;
            if (string.IsNullOrWhiteSpace(current.NextActivityWebId)) return null;
            return this.WorkflowDefinition.ActivityCollection.SingleOrDefault(f => f.WebId == current.NextActivityWebId);
        }

        public async virtual Task SaveAsync(string activityId)
        {
            Debug.WriteLine("Saving....." + activityId);
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(this);
                await session.SubmitChanges(activityId);
                Debug.WriteLine("Saved...");
            }
        }

        public DynamicObject ExecutionBag { get; set; }
        private readonly ObjectCollection<string> m_validExecutableStepsCollection = new ObjectCollection<string>();
        private readonly ObjectCollection<string> m_forbiddenStepsCollection = new ObjectCollection<string>();
        public ObjectCollection<string> ValidExecutableSteps
        {
            get { return m_validExecutableStepsCollection; }
        }

        public ObjectCollection<string> ForbiddenSteps
        {
            get { return m_forbiddenStepsCollection; }
        }
    }
}