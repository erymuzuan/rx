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


        public T GetActivity<T>(string webId) where T : Activity
        {
            if (null == this.WorkflowDefinition) throw new InvalidOperationException("You have to load the WorkflowDefinition before calling GetActivity");
            return this.WorkflowDefinition.ActivityCollection.OfType<T>().Single(w => w.WebId == webId);
        }


        public async virtual Task SaveAsync(string activityId, ActivityExecutionResult result)
        {
            const string operation = "Execute";
            var act = this.GetActivity<Activity>(activityId);
            var headers = new Dictionary<string, object>
                                {
                                    {"Name", act.Name},
                                    {"ActivityWebId", activityId},
                                    {"Executed", result.Status == ActivityExecutionStatus.Success },
                                    {"NextActivities",string.Join(",", result.NextActivities)}
                                };

            var tracker = await this.GetTrackerAsync();
            tracker.AddExecutedActivity(act);

            var context = new SphDataContext();
            if (this.WorkflowId > 0)
            {
                using (var session = context.OpenSession())
                {
                    session.Attach(this, tracker);
                    await session.SubmitChanges(operation, headers);
                }
                return;
            }

            // for start do not fire the subscriber
            using (var session = context.OpenSession())
            {
                session.Attach(this);
                await session.SubmitChanges("Start", headers);
            }

            tracker.WorkflowId = this.WorkflowId;
            tracker.WorkflowDefinitionId = this.WorkflowDefinitionId;
            using (var session = context.OpenSession())
            {
                session.Attach(this, tracker);
                await session.SubmitChanges(operation, headers);
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
            }
        }
    }
}