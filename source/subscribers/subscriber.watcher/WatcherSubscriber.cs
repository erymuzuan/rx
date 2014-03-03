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

            this.ListenerCollection.Clear();

            foreach (var ed in definitions)
            {
                var listener = this.RegisterCustomEntityDependencies(ed);
                if (null == listener) continue;
                listener.Changed += new EntityChangedEventHandler<Entity>(EntityChanged);
                listener.Run();
                this.ListenerCollection.Add(listener);

            }
            // get the listeners


        }


        public dynamic RegisterCustomEntityDependencies(EntityDefinition ed1)
        {
            var sqlAssembly = Assembly.Load("rabbitmq.changepublisher");
            var sqlRepositoryType = sqlAssembly.GetType("Bespoke.Sph.RabbitMqPublisher.EntityChangedListener`1");

            try
            {
                var edAssembly = Assembly.Load(ConfigurationManager.ApplicationName + "." + ed1.Name);
                var edTypeName = string.Format("Bespoke.{0}_{1}.Domain.{2}", ConfigurationManager.ApplicationName, ed1.EntityDefinitionId, ed1.Name);
                var edType = edAssembly.GetType(edTypeName);
                if (null == edType)
                    Console.WriteLine("Cannot create type " + edTypeName);

                var listenerType = sqlRepositoryType.MakeGenericType(edType);
                dynamic listener = Activator.CreateInstance(listenerType);


                return listener;
            }
            catch (FileNotFoundException e)
            {
                //  Console.WriteLine(e);
            }

            return null;


        }

        private async void EntityChanged<T>(object sender, EntityChangedEventArgs<T> e) where T : Entity
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
                await this.SendMessage(w, e.Item);
            }
        }

        private async Task SendMessage<T>(Watcher watcher, T item) where T: Entity
        {
            var context = new SphDataContext();
            var message = new Message
            {
                Subject = "There are changes in your watched item: " + item.GetType().Name,
                UserName = watcher.User,
                Body = item.ToString()
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
