using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json;

namespace subscriber.version.control
{
    public class WorkflowSourceSubscriber : Subscriber<WorkflowDefinition>
    {
        public override string QueueName
        {
            get { return "source_wd_queue"; }
        }

        public override string[] RoutingKeys
        {
            get
            {
                return new[]
                {
                    typeof(WorkflowDefinition).Name + ".#.#"
                };
            }
        }

        protected override async Task ProcessMessage(WorkflowDefinition item, MessageHeaders header)
        {
            var wc = ConfigurationManager.WorkflowSourceDirectory;
            var type = item.GetType();
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            dynamic wi = item;
            var file = Path.Combine(folder, wi.Name + ".json");
            File.WriteAllText(file, item.ToJsonString(Formatting.Indented));

            var store = ObjectBuilder.GetObject<IBinaryStore>();
            var schema = await store.GetContentAsync(item.SchemaStoreId);
            var xsd = Path.Combine(folder, wi.Name + ".xsd");
            File.WriteAllBytes(xsd, schema.Content);

        }
    }
}