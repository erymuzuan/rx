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

        private ObjectCollection<Watcher> m_watchers;
        private IEntityChangedListener<Building> m_buildingListener;
        private IEntityChangedListener<RentalApplication> m_applicationListener;
        private IEntityChangedListener<Space> m_spaceListener;
        private IEntityChangedListener<Complaint> m_complaintListener;
        private IEntityChangedListener<Maintenance> m_maintenanceListener;
        private IEntityChangedListener<Contract> m_contractListener;

        protected async override void OnStart()
        {
            var context = new SphDataContext();
            var query = context.Watchers.Where(w => w.IsActive == true);
            var lo = await context.LoadAsync(query, includeTotalRows: true);
            m_watchers = new ObjectCollection<Watcher>(lo.ItemCollection);
            while (lo.HasNextPage)
            {
                lo = await context.LoadAsync(query, lo.CurrentPage + 1, includeTotalRows: true);
                m_watchers.AddRange(lo.ItemCollection);
            }

            // get the listeners
            m_buildingListener = ObjectBuilder.GetObject<IEntityChangedListener<Building>>();
            m_buildingListener.Changed += EntityChanged;
            m_buildingListener.Run();

            m_applicationListener = ObjectBuilder.GetObject<IEntityChangedListener<RentalApplication>>();
            m_applicationListener.Changed += EntityChanged;
            m_applicationListener.Run();

            m_complaintListener = ObjectBuilder.GetObject<IEntityChangedListener<Complaint>>();
            m_complaintListener.Changed += EntityChanged;
            m_complaintListener.Run();

            m_maintenanceListener = ObjectBuilder.GetObject<IEntityChangedListener<Maintenance>>();
            m_maintenanceListener.Changed += EntityChanged;
            m_maintenanceListener.Run();

            m_spaceListener = ObjectBuilder.GetObject<IEntityChangedListener<Space>>();
            m_spaceListener.Changed += EntityChanged;
            m_spaceListener.Run();

            m_contractListener = ObjectBuilder.GetObject<IEntityChangedListener<Contract>>();
            m_contractListener.Changed += EntityChanged;
            m_contractListener.Run();

        }

        async void EntityChanged<T>(object sender, EntityChangedEventArgs<T> e) where T : Entity
        {
            this.WriteMessage("Changed to " + e);
            var entityName = typeof(T).Name;
            var id = (int)typeof(T).GetProperty(entityName + "Id")
                .GetValue(e.Item);
            var watchers = m_watchers
                .Where(w => w.EntityId == id
                && w.EntityName == entityName)
                .ToList();
            this.WriteMessage("There {0} watchers", watchers.Count);
            foreach (var w in watchers)
            {
                await this.SendMessage(w, e);
            }
        }

        private async Task SendMessage<T>(Watcher watcher, EntityChangedEventArgs<T> arg) where T:Entity
        {
            var context = new SphDataContext();
            var message = new Message
            {
                Subject = "Pertukaran di item yang anda pantau : " + arg.Item,
                UserName = watcher.User,
                Body = arg.AuditTrail.ToString()
            };


            using (var session = context.OpenSession())
            {
                session.Attach(message);
                await session.SubmitChanges("Add new message");
            }
        }
    }
}
