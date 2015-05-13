using System;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
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
            typeof(Adapter).Name + ".#.#",
            typeof(EmailTemplate).Name + ".#.#",
            typeof(DocumentTemplate).Name + ".#.#",
            typeof(ReportDelivery).Name + ".#.#",
            typeof(Page).Name + ".#.#",
            typeof(Organization).Name + ".#.#",
            typeof(Setting).Name + ".#.#",
            typeof(Designation).Name + ".#.#",
            typeof(EntityChart).Name + ".#.#",
            typeof(EntityView).Name + ".#.#",
            typeof(EntityForm).Name + ".#.#",
            typeof(EntityDefinition).Name + ".#.#",
            typeof(ReportDefinition).Name + ".#.#",
            typeof(WorkflowDefinition).Name + ".#.#",
            typeof(TransformDefinition).Name + ".#.#",
            typeof(Trigger).Name + ".#.#"
        };

        private void RemoveExistingSource(Entity item)
        {
            var wc = ConfigurationManager.SphSourceDirectory;
            var type = item.GetType();
            var folder = Path.Combine(wc, type.Name);
            if(!Directory.Exists(folder))
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
                        this.WriteMessage("[Id] field cannot be found in in {0}",f);
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
            RemoveExistingSource(item);
            var type = item.GetType();

            if (type == typeof(ReportDefinition))
            {
                var rd = new ReportDefinitionSourceProvider();
                await rd.ProcessItem(item as ReportDefinition);
                return;
            }

            if (type == typeof(DocumentTemplate))
            {
                var dt = new DocumentTemplateSourceProvider();
                await dt.ProcessItem(item as DocumentTemplate);
                return;
            }

            if (type == typeof(WorkflowDefinition))
            {
                var wd = new WorkflowSourceProvider();
                await wd.ProcessItem(item as WorkflowDefinition);
                return;
            }

            if (type == typeof(EntityDefinition))
            {
                var ed = new EntityDefinitionSourceProvider();
                await ed.ProcessItem(item as EntityDefinition);
                return;
            }

            if (type == typeof(EntityView))
            {
                var ev = new EntityViewSourceProvider();
                await ev.ProcessItem(item as EntityView);
                return;
            }

            if (type == typeof(EntityForm))
            {
                var ef = new EntityFormSourceProvider();
                await ef.ProcessItem(item as EntityForm);
                return;
            }


            var provider = new EntitySourceProvider();
            await provider.ProcessItem(item);

        }
    }
}