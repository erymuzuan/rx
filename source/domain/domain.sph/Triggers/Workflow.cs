using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class Workflow : Entity
    {
        public override int GetId()
        {
            return this.WorkflowId;
        }

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
            var act = this.GetActivity<Activity>(activityId);
            var headers = new Dictionary<string, object>
                                {
                                    {"Name", act.Name},
                                    {"ActivityWebId", activityId},
                                    {"IsActivityExecuted", true},
                                    {"NextActivityWebId", act.NextActivityWebId}
                                };

            var tracker = await this.GetTrackerAsync();
            tracker.AddExecutedActivity(act);

            var context = new SphDataContext();
            if (this.WorkflowId > 0)
            {
                using (var session = context.OpenSession())
                {
                    session.Attach(this, tracker);
                    await session.SubmitChanges("Execute", headers);
                }
                return;
            }
            using (var session = context.OpenSession())
            {
                session.Attach(this);
                await session.SubmitChanges("Execute", headers);
            }

            tracker.WorkflowId = this.WorkflowId;
            tracker.WorkflowDefinitionId = this.WorkflowDefinitionId;
            using (var session = context.OpenSession())
            {
                session.Attach(tracker);
                await session.SubmitChanges("Execute", headers);
            }
        }


        public async Task<Tracker> GetTrackerAsync()
        {
            if (this.WorkflowId == 0)
                return new Tracker
                {
                    Workflow = this,
                    WorkflowDefinition = this.WorkflowDefinition,
                    WorkflowId = this.WorkflowId,
                    WorkflowDefinitionId = this.WorkflowDefinitionId
                };

            var context = new SphDataContext();
            var tracker = await context.LoadOneAsync<Tracker>(t => t.WorkflowId == this.WorkflowId)
                          ??
                          new Tracker { WorkflowId = this.WorkflowId, WorkflowDefinitionId = this.WorkflowDefinitionId };
            tracker.Workflow = this;
            tracker.WorkflowDefinition = this.WorkflowDefinition;
            if (tracker.TrackerId == 0)
                tracker.Init(this, this.WorkflowDefinition);

            return tracker;
        }


        public async virtual Task TerminateAsync()
        {
            this.State = "Terminated";
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(this);
                await session.SubmitChanges("Terminate");
                // TODO : use the subscriber to delete any scheduled task or related resources to this instance
                // this.WorkflowDefinition.ActivityCollection.ForEach(async a => await a.TerminateAsync(this));
            }
        }
    }
}