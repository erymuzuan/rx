using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace subscriber.version.control
{
    public class SourceSubscriber : Subscriber<Entity>
    {
        public override string QueueName => "source_queue";

        public override string[] RoutingKeys => new[]
        {
            "#.added.#",
            "#.changed.#",
            "#.deleted.#"
        };

        private void RemoveExistingSource(Entity item)
        {
            if (null == item) return;
            
            var wc = ConfigurationManager.SphSourceDirectory;
            var type = item.GetEntityType();
            var folder = Path.Combine(wc, type.Name);
            if (!Directory.Exists(folder))
                return;

            var files = Directory.GetFiles(folder, "*.json");
            foreach (var f in files)
            {
                var text = File.ReadAllText(f);
                if (string.IsNullOrWhiteSpace(text))
                {
                    File.Delete(f);
                    continue;
                }
                try
                {
                    var o = JObject.Parse(text);
                    var idToken = o.SelectToken("$.Id");
                    if (null == idToken)
                    {
                        this.WriteMessage("[Id] field cannot be found in in {0}", f);
                        continue;
                    }
                    var id = idToken.Value<string>();
                    if (id != item.Id) continue;
                    File.Delete(f);
                    return;
                }
                catch (JsonReaderException e)
                {
                    this.NotificicationService.Write($"There is an error in {f}");
                    this.NotificicationService.WriteError(e, $"there is an error in {f}");
                }
            }
        }

        protected override async Task ProcessMessage(Entity item, MessageHeaders header)
        {
            if (null == item) return;
            RemoveExistingSource(item);

            var type = item.GetEntityType();

            if (type == typeof(ReportDefinition))
            {
                var rd = new ReportDefinitionSourceProvider();
                if (header.Crud == CrudOperation.Deleted)
                    await rd.RemoveItem(item as ReportDefinition);
                else
                    await rd.ProcessItem(item as ReportDefinition);
                return;
            }

            if (type == typeof(DocumentTemplate))
            {
                var dt = new DocumentTemplateSourceProvider();
                if (header.Crud == CrudOperation.Deleted)
                    await dt.RemoveItem(item as DocumentTemplate);
                else
                    await dt.ProcessItem(item as DocumentTemplate);
                return;
            }

            if (type == typeof(WorkflowDefinition))
            {
                var wd = new WorkflowSourceProvider();
                if (header.Crud == CrudOperation.Deleted)
                    await wd.RemoveItem(item as WorkflowDefinition);
                else
                    await wd.ProcessItem(item as WorkflowDefinition);
                return;
            }
            

            if (type == typeof(EntityView))
            {
                var ev = new EntityViewSourceProvider();
                if (header.Crud == CrudOperation.Deleted)
                    await ev.RemoveItem(item as EntityView);
                else
                    await ev.ProcessItem(item as EntityView);
                return;
            }

            if (type == typeof(EntityForm))
            {
                var ef = new EntityFormSourceProvider();
                if (header.Crud == CrudOperation.Deleted)
                    await ef.RemoveItem(item as EntityForm);
                else
                    await ef.ProcessItem(item as EntityForm);
                return;
            }

            var sourceAttribute = StoreAsSourceAttribute.GetAttribute(type);
            if (null == sourceAttribute) return;

            var provider = new EntitySourceProvider();


            if (header.Crud == CrudOperation.Deleted)
                await provider.RemoveItem(item);
            else
                await provider.ProcessItem(item);

        }
    }
}