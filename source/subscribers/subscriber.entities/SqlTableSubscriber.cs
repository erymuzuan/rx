using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SqlRepository;
using Bespoke.Sph.SubscribersInfrastructure;

namespace subscriber.entities
{
    public class SqlTableSubscriber : Subscriber<EntityDefinition>
    {
        public override string QueueName => "ed_sql_table";
        public override string[] RoutingKeys => new[] { typeof(EntityDefinition).Name + ".changed.Publish" };
        protected override async Task ProcessMessage(EntityDefinition item, MessageHeaders header)
        {
            var tableSchema = new SqlTableBuilder(m => this.WriteMessage(m), m => this.WriteWarning(m), m => this.WriteError(m));
            await tableSchema.BuildAsync(item);
        }

        public Task ProcessMessageAsync(EntityDefinition ed)
        {
            return this.ProcessMessage(ed, null);
        }
    }
}
