using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.WathersSubscribers
{
    public class WatcherSubscriber : Subscriber<Watcher>
    {
        public override string QueueName
        {
            get { return "watcher_queue"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { "Watcher.*" }; }
        }

        protected override Task ProcessMessage(Watcher item, MessageHeaders header)
        {
            if (header.Crud == CrudOperation.Added)
            {
                m_watchers.Add(item);
            }
            if (header.Crud == CrudOperation.Deleted)
            {
                m_watchers.RemoveAll(w => w.WatcherId == item.WatcherId);
            }
            if (header.Crud == CrudOperation.Changed)
            {
                m_watchers.RemoveAll(w => w.WatcherId == item.WatcherId);
                m_watchers.Add(item);
            }
            return Task.FromResult(0);
        }

        private readonly ObjectCollection<Watcher> m_watchers = new ObjectCollection<Watcher>();

        protected async override void OnStart()
        {
            var context = new SphDataContext();
            var query = context.Watchers.Where(w => w.IsActive == true);
            var lo = await context.LoadAsync(query, includeTotalRows: true);
            m_watchers.ClearAndAddRange(lo.ItemCollection);
            while (lo.HasNextPage)
            {
                lo = await context.LoadAsync(query, lo.CurrentPage + 1, includeTotalRows: true);
                m_watchers.AddRange(lo.ItemCollection);
            }

            var edQuery = context.EntityDefinitions;
            var edLo = await context.LoadAsync(edQuery, includeTotalRows: true);
            var definitions = new ObjectCollection<EntityDefinition>(edLo.ItemCollection);

            while (edLo.HasNextPage)
            {
                edLo = await context.LoadAsync(edQuery, edLo.CurrentPage + 1, includeTotalRows: true);
                definitions.AddRange(edLo.ItemCollection);
            }

            // get the listeners
            m_listener = ObjectBuilder.GetObject<IEntityChangedListener<AuditTrail>>();
            m_listener.Changed += EntityChanged;
            m_listener.Run();



        }

        IEntityChangedListener<AuditTrail> m_listener;
        private async void EntityChanged(object sender, EntityChangedEventArgs<AuditTrail> e)
        {
            this.WriteMessage("Changed to " + e);
            var entityName = e.Item.Type;
            var id = e.Item.EntityId;
            var watchers = m_watchers
                .Where(w => w.EntityId == id && w.EntityName == entityName)
                .ToList();
            this.WriteMessage("There {0} watchers", watchers.Count);
            foreach (var w in watchers)
            {
                await this.SendMessage(w, e.Item);
            }
        }

        private async Task SendMessage(Watcher watcher, AuditTrail log)
        {
            var context = new SphDataContext();
            var message = new Message
            {
                Subject = "There are changes in your watched item: " + log.Type,
                UserName = watcher.User,
                Body = log.ToString()
            };


            using (var session = context.OpenSession())
            {
                session.Attach(message);
                await session.SubmitChanges("Add new message");
            }
        }

        protected override void OnStop()
        {
            m_listener.Stop();
            base.OnStop();
        }
    }
}
