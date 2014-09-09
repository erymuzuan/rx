﻿using System;
using System.Collections.Generic;
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

        public virtual Task<ActivityExecutionResult> ExecuteAsync(string activityId, string correlation = null)
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
            tracker.AddExecutedActivity(act, result.Correlation);

            var context = new SphDataContext();
            if (!string.IsNullOrWhiteSpace(this.Id))
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

            tracker.WorkflowId = this.Id;
            tracker.WorkflowDefinitionId = this.WorkflowDefinitionId;
            using (var session = context.OpenSession())
            {
                session.Attach(this, tracker);
                await session.SubmitChanges(operation, headers);
            }
        }


        public async Task<Tracker> GetTrackerAsync()
        {
            if (null != m_tracker)
                return m_tracker;

            if ( string.IsNullOrWhiteSpace(this.Id))
                return m_tracker = new Tracker
                {
                    Workflow = this,
                    WorkflowDefinition = this.WorkflowDefinition,
                    WorkflowId = this.Id,
                    WorkflowDefinitionId = this.WorkflowDefinitionId
                };

            var context = new SphDataContext();
            m_tracker = await context.LoadOneAsync<Tracker>(t => t.WorkflowId == this.Id)
                          ??
                          new Tracker { WorkflowId = this.Id, WorkflowDefinitionId = this.WorkflowDefinitionId };
            m_tracker.Workflow = this;
            m_tracker.WorkflowDefinition = this.WorkflowDefinition;
            if ( string.IsNullOrWhiteSpace(m_tracker.Id))
                m_tracker.Init(this, this.WorkflowDefinition);

            return m_tracker;
        }

        public async Task LoadWorkflowDefinitionAsync()
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var doc = await store.GetContentAsync(string.Format("wd.{0}.{1}", this.WorkflowDefinitionId, this.Version));
            using (var stream = new System.IO.MemoryStream(doc.Content))
            {
                this.WorkflowDefinition = stream.DeserializeFromXml<WorkflowDefinition>();
            }
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