﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Bespoke.Sph.Domain
{
    public partial class Workflow : Entity
    {
        private Tracker m_tracker;


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

        public virtual Task<ActivityExecutionResult> StartAsync(string parentWorkflow, string parentActivity)
        {
            ParentWorkflowId = parentWorkflow;
            ParentActivity = parentActivity;
            // run the first one and save
            return Task.FromResult(new ActivityExecutionResult { Status = ActivityExecutionStatus.None });
        }

        public virtual Task<ActivityExecutionResult> ExecuteAsync(string activityId, string correlation = null)
        {
            return Task.FromResult(new ActivityExecutionResult { Status = ActivityExecutionStatus.None });
        }


        public T GetActivity<T>(string webId) where T : Activity
        {
            if (null == this.WorkflowDefinition) throw new InvalidOperationException("You have to load the WorkflowDefinition before calling GetActivity");
            return this.WorkflowDefinition.ActivityCollection.OfType<T>().Single(w => w.WebId == webId);
        }


        public virtual async Task SaveAsync(string activityId, ActivityExecutionResult result)
        {
            if (this.IsNewItem)
                this.Id = Strings.GenerateId();

            const string OPERATION = "Execute";
            var act = this.GetActivity<Activity>(activityId);
            var headers = new Dictionary<string, object>
                                {
                                    {"Name", act.Name},
                                    {"ActivityWebId", activityId},
                                    {"Executed", result.Status == ActivityExecutionStatus.Success },
                                    {"NextActivities",string.Join(",", result.NextActivities)}
                                };

            var tracker = await this.GetTrackerAsync().ConfigureAwait(false);
            tracker.AddExecutedActivity(act, result.Correlation);

            var context = new SphDataContext();
            if (!string.IsNullOrWhiteSpace(this.Id))
            {
                using (var session = context.OpenSession())
                {
                    session.Attach(this, tracker);
                    await session.SubmitChanges(OPERATION, headers).ConfigureAwait(false);
                }
                return;
            }

            // for start do not fire the subscriber
            using (var session = context.OpenSession())
            {
                session.Attach(this);
                await session.SubmitChanges("Start", headers).ConfigureAwait(false);
            }

            tracker.WorkflowId = this.Id;
            tracker.WorkflowDefinitionId = this.WorkflowDefinitionId;
            using (var session = context.OpenSession())
            {
                session.Attach(this, tracker);
                await session.SubmitChanges(OPERATION, headers).ConfigureAwait(false);
            }
        }


        public async Task<Tracker> GetTrackerAsync()
        {
            if (null != m_tracker)
                return m_tracker;

            if (string.IsNullOrWhiteSpace(this.Id))
                return m_tracker = new Tracker(this.WorkflowDefinition, this);

            var context = new SphDataContext();
            m_tracker = await context.LoadOneAsync<Tracker>(t => t.WorkflowId == this.Id).ConfigureAwait(false)
                          ?? new Tracker(this.WorkflowDefinition, this);
            m_tracker.Workflow = this;
            m_tracker.WorkflowDefinition = this.WorkflowDefinition;

            if (string.IsNullOrWhiteSpace(m_tracker.Id))
                m_tracker.Init(this, this.WorkflowDefinition);

            return m_tracker;
        }


        public async Task InitializeCorrelationSetAsync(string name, string value)
        {
            var tracker = await this.GetTrackerAsync().ConfigureAwait(false);
            var corr = new Correlation
            {
                WorfklowId = this.Id,
                WorkflowDefinitionId = this.WorkflowDefinitionId,
                Tracker = tracker,
                Name = name,
                Id = Strings.GenerateId(),
                Value = value

            };
            var repos = ObjectBuilder.GetObject<IWorkflowService>();
            await repos.SaveInstanceAsync(corr);
        }

        public async Task LoadWorkflowDefinitionAsync()
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var file = $"wd.{this.WorkflowDefinitionId}.{this.Version}";
            var doc = await store.GetContentAsync(file).ConfigureAwait(false);
            using (var stream = new MemoryStream(doc.Content))
            {
                this.WorkflowDefinition = stream.DeserializeFromJson<WorkflowDefinition>();
            }
        }


        public virtual async Task TerminateAsync()
        {
            this.State = "Terminated";
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(this);
                await session.SubmitChanges("Terminate").ConfigureAwait(false);
            }
        }

        public string ParentWorkflowId { get; set; }
        public string ParentActivity { get; set; }

        public async Task<Workflow> GetParentWorkflowAsync()
        {
            if (string.IsNullOrWhiteSpace(ParentWorkflowId)) return null;
            if (string.IsNullOrWhiteSpace(ParentActivity)) return null;
            var context = new SphDataContext();
            return await context.LoadOneAsync<Workflow>(c => c.Id == ParentWorkflowId);
        }
    }
}