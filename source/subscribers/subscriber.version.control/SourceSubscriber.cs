using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

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
                    typeof(Trigger).Name + ".#.#"
                };
            }
        }

        protected override async Task ProcessMessage(Entity item, MessageHeaders header)
        {
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