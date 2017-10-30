using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;

namespace subscriber.version.control
{
    public class SourceSubscriber : Subscriber<Entity>
    {
        public override string QueueName => "source_queue";

        public override string[] RoutingKeys => new[]
        {
            "#.added.#",
            "#.changed.#",
            "#.deleted.#",
            "persistence.#"
        };

        protected override async Task ProcessMessage(Entity item, MessageHeaders header)
        {
            if (null == item) return;
            var option = item.GetPersistenceOption();
            if (!option.IsSource) return;

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


            var provider = new EntitySourceProvider();
            if (header.GetRawHeaders().ContainsKey("crud") && 
                header.Crud == CrudOperation.Deleted)
                await provider.RemoveItem(item);
            else
                await provider.ProcessItem(item);

        }
    }
}