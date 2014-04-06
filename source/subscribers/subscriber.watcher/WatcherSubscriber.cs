using System;
using System.IO;
using System.Linq;
using System.Reflection;
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
            get { return new[] { "Watcher.#.#" }; }
        }

        protected override Task ProcessMessage(Watcher item, MessageHeaders header)
        {
            this.WriteMessage("A watcher has been {0} : \r\n{1}", header.Crud, item);
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

            var edQuery = context.EntityDefinitions.Where(d => d.IsPublished == true);
            var edLo = await context.LoadAsync(edQuery, includeTotalRows: true);
            var definitions = new ObjectCollection<EntityDefinition>(edLo.ItemCollection);

            while (edLo.HasNextPage)
            {
                edLo = await context.LoadAsync(edQuery, edLo.CurrentPage + 1, includeTotalRows: true);
                definitions.AddRange(edLo.ItemCollection);
            }

            this.ListenerCollection.Clear();

            foreach (var ed in definitions)
            {
                var listener = this.RegisterCustomEntityDependencies(ed);
                if (null == listener) continue;
                listener.Run();
                this.ListenerCollection.Add(listener);

            }
            // get the listeners

        }


        public dynamic RegisterCustomEntityDependencies(EntityDefinition ed1)
        {
            var changePublisherAssembly = Assembly.Load("rabbitmq.changepublisher");
            var changePublisherType = changePublisherAssembly.GetType("Bespoke.Sph.RabbitMqPublisher.EntityChangedListener`1");

            try
            {
                var edAssembly = Assembly.Load(ConfigurationManager.ApplicationName + "." + ed1.Name);
                var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, ed1.EntityDefinitionId, ed1.Name);
                var edType = edAssembly.GetType(edTypeName);
                if (null == edType)
                    this.WriteError(new Exception("Cannot create type " + edTypeName));

                var listenerType = changePublisherType.MakeGenericType(edType);
                dynamic listener = Activator.CreateInstance(listenerType, ObjectBuilder.GetObject("IBrokerConnection"));
                listener.Callback = new Action<object>(arg => this.EntityChanged(listener, arg));


                return listener;
            }
            catch (FileNotFoundException e)
            {
                this.WriteError(e);
            }

            return null;


        }

        public async void EntityChanged(object sender, dynamic e)
        {
            this.WriteMessage("Changed to " + e);
            var entityName = e.Item.GetType().Name;
            var id = e.Item.GetId();
            var watchers = m_watchers
                .Where(w => w.EntityId == id && w.EntityName == entityName)
                .ToList();
            this.WriteMessage("There {0} watchers", watchers.Count);
            foreach (var w in watchers)
            {
                await this.SendMessage(w, e.Item, e.AuditTrail);
            }
        }

        private async Task SendMessage<T>(Watcher watcher, T item, AuditTrail log) where T : Entity
        {
            var context = new SphDataContext();
            var message = new Message
            {
                Subject = "There are changes in your watched item: " + item.GetType().Name,
                UserName = watcher.User,
                Body = string.Format("<p>{0}</p><div>{1}</div>", item, log)
            };


            using (var session = context.OpenSession())
            {
                session.Attach(message);
                await session.SubmitChanges("Add new message");
            }
        }

        private readonly ObjectCollection<dynamic> m_listenerCollection = new ObjectCollection<dynamic>();

        public ObjectCollection<dynamic> ListenerCollection
        {
            get { return m_listenerCollection; }
        }

        protected override void OnStop()
        {
            this.ListenerCollection.ForEach(l => l.Stop());
            base.OnStop();
        }
    }
}
