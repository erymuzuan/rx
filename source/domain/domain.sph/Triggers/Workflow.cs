using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
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


        public virtual async Task SaveAsync(string activityId, ActivityExecutionResult result)
        {
            if (this.IsNewItem)
                this.Id = Guid.NewGuid().ToString();

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
            var cors = this.WorkflowDefinition.CorrelationSetCollection.Single(x => x.Name == name);
            var cort = this.WorkflowDefinition.CorrelationTypeCollection.Single(x => x.Name == cors.Type);
            
            var id = Guid.NewGuid().ToString();
            var json = JsonConvert.SerializeObject(new
            {
                wid = this.Id,
                wdid = this.WorkflowDefinitionId,
                tracker,
                id,
                name = cort.Name,
                value
            });
            var url = $"{ConfigurationManager.ElasticSearchIndex}/{"correlationset"}/{id}";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.PutAsync(url, new StringContent(json)).ConfigureAwait(false);
                if (null != response)
                {
                    Debug.Write(".");
                }
            }


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
    }
}