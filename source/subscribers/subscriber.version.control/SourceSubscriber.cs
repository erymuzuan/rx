using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json;

namespace subscriber.version.control
{
    public class SourceSubscriber : Subscriber<Entity>
    {
        public override string QueueName
        {
            get { return "source_queue"; }
        }

        public override string[] RoutingKeys
        {
            get
            {
                return new[]
                {
                    typeof(EntityView).Name + ".#.#",
                    typeof(EntityForm).Name + ".#.#",
                    typeof(EntityDefinition).Name + ".#.#",
                    typeof(WorkflowDefinition).Name + ".#.#"
                };
            }
        }

        protected override Task ProcessMessage(Entity item, MessageHeaders header)
        {
            var wc = ConfigurationManager.WorkflowSourceDirectory;
            var type = item.GetType();
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            dynamic wi = item;
            var file = Path.Combine(folder, wi.Name + ".json");
            File.WriteAllText(file, item.ToJsonString(Formatting.Indented));
            return Task.FromResult(0);
        }
    }
}