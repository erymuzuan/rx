using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json;

namespace subscriber.version.control
{
    public class DocumentTemplateSourceSubscriber : Subscriber<DocumentTemplate>
    {
        public override string QueueName
        {
            get { return "source_document_template_queue"; }
        }

        public override string[] RoutingKeys
        {
            get
            {
                return new[]
                {
                    typeof(DocumentTemplate).Name + ".#.#"
                };
            }
        }

        protected async override Task ProcessMessage(DocumentTemplate item, MessageHeaders header)
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
            var word = await store.GetContentAsync(item.WordTemplateStoreId);
            var xsd = Path.Combine(folder, wi.Name + ".docx");
            File.WriteAllBytes(xsd, word.Content);

        }
    }
}