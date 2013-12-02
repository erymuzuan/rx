using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace Bespoke.Sph.WorkflowsExecution
{
    public class WorkflowSubscriber : Subscriber<Workflow>
    {
        public override string QueueName
        {
            get { return "workflow_execution"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { "Workflow.*" }; }
        }

        protected override void OnStart()
        {
            this.WriteMessage("Registering Workflow types for XmlSerializer");
            var files = Directory.GetFiles(@".\", "workflows.*.dll");
            foreach (var s in files)
            {
                var types = Assembly.LoadFrom(s).GetTypes().Where(t => t.BaseType == typeof(Workflow)).ToList();
                types.ForEach(t => XmlSerializerService.RegisterKnownTypes(typeof(Workflow), t));

                this.WriteMessage(string.Join(",", types.Select(t => t.FullName).ToArray()));
            }
            base.OnStart();
        }

        protected async override Task ProcessMessage(Workflow item, MessageHeaders header)
        {
            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var doc = await store.GetContentAsync(string.Format("wd.{0}.{1}", item.WorkflowDefinitionId, item.Version));

            if (item.State == "Completed") return;

            WorkflowDefinition wd;
            using (var stream = new MemoryStream(doc.Content))
            {
                wd = stream.DeserializeFromXml<WorkflowDefinition>();
            }
            item.WorkflowDefinition = wd;

            // get current activity
            dynamic headers = header;
            var initiator = wd.ActivityCollection.Single(a => a.IsInitiator);
            string activityId = headers.ActivityWebId;
            

            if (initiator.WebId == activityId)
            {
                this.WriteMessage("started");
            }
            this.WriteMessage("Running {0}", item.GetCurrentActivity());
            var result = await item.ExecuteAsync();
            this.WriteMessage(result);

        }

    }
}
