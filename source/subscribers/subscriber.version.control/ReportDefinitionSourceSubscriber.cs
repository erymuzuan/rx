using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json;

namespace subscriber.version.control
{
    public class ReportDefinitionSourceSubscriber : Subscriber<ReportDefinition>
    {
        public override string QueueName
        {
            get { return "source_rdl_queue"; }
        }

        public override string[] RoutingKeys
        {
            get
            {
                return new[]
                {
                    typeof(ReportDefinition).Name + ".#.#"
                };
            }
        }

        protected override Task ProcessMessage(ReportDefinition item, MessageHeaders header)
        {
            var wc = ConfigurationManager.WorkflowSourceDirectory;
            var type = item.GetType();
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);

            var file = Path.Combine(folder, item.Title + ".json");
            File.WriteAllText(file, item.ToJsonString(Formatting.Indented));

            return Task.FromResult(0);


        }
    }
}