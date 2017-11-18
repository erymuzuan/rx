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
        public override string QueueName => "watcher_queue";

        public override string[] RoutingKeys => new[] { "Watcher.#.#" };

        protected override Task ProcessMessage(Watcher item, MessageHeaders header)
        {
            this.WriteMessage($"A watcher has been {header.Crud} : \r\n{item}");
            switch (header.Crud)
            {
                case CrudOperation.Added:
                    m_watchers.Add(item);
                    break;
                case CrudOperation.Deleted:
                    m_watchers.RemoveAll(w => w.Id == item.Id);
                    break;
                case CrudOperation.Changed:
                    m_watchers.RemoveAll(w => w.Id == item.Id);
                    m_watchers.Add(item);
                    break;
                case CrudOperation.None:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            return Task.FromResult(0);
        }

        private readonly ObjectCollection<Watcher> m_watchers = new ObjectCollection<Watcher>();

        protected override async void OnStart()
        {
            var context = new SphDataContext();
            // ReSharper disable once RedundantBoolCompare
            var query = context.Watchers.Where(w => w.IsActive == true);
            var lo = await context.LoadAsync(query, includeTotalRows: true);
            m_watchers.ClearAndAddRange(lo.ItemCollection);
            while (lo.HasNextPage)
            {
                lo = await context.LoadAsync(query, lo.CurrentPage + 1, includeTotalRows: true);
                m_watchers.AddRange(lo.ItemCollection);
            }

            var definitions = context.LoadFromSources<EntityDefinition>(x => x.IsPublished);

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
                var file = ConfigurationManager.ApplicationName + "." + ed1.Name;
                if (!File.Exists(file))
                    return null;
                var edAssembly = Assembly.Load(file);
                var edTypeName = $"{ed1.CodeNamespace}.{ed1.Name}";
                var edType = edAssembly.GetType(edTypeName);
                if (null == edType)
                    this.WriteError(new Exception("Cannot create type " + edTypeName));

                var listenerType = changePublisherType.MakeGenericType(edType);
                dynamic listener = Activator.CreateInstance(listenerType);
                listener.Callback = new Action<object>(arg => this.EntityChanged(listener, arg));


                return listener;
            }
            catch (FileNotFoundException e)
            {
                this.WriteError(new Exception(e.Message));
            }

            return null;


        }

        public async void EntityChanged(object sender, dynamic e)
        {
            var entityName = e.Item.GetType().Name;
            var id = e.Item.Id;
            var filters = new[]
            {
                new Filter("EntityName", Operator.Eq, entityName),
                new Filter("EntityId", Operator.Eq, id),
            };

            var repos = ObjectBuilder.GetObject<IReadOnlyRepository<Watcher>>();
            var lo = await repos.SearchAsync(new QueryDsl(filters, new[] { new Sort { Direction = SortDirection.Desc, Path = nameof(Watcher.CreatedDate) } }, 0, 100));

            var watchers = lo.ItemCollection.Select(x => x.User).ToList();
            this.WriteMessage("Changed to " + e);

            this.WriteMessage($"There { watchers.Count} watchers");
            var sendMessageTasks = from w in watchers
                                   select (Task)this.SendMessage(w, e.Item, e.AuditTrail);
            await Task.WhenAll(sendMessageTasks);

        }

        private async Task SendMessage<T>(string user, T item, AuditTrail log) where T : Entity
        {
            var context = new SphDataContext();
            var message = new Message
            {
                Subject = "There are changes in your watched item: " + item.GetType().Name,
                UserName = user,
                Body = $"<p>{item}</p><div>{log}</div>",
                Id = Guid.NewGuid().ToString()
            };


            using (var session = context.OpenSession())
            {
                session.Attach(message);
                await session.SubmitChanges("Add new message");
            }
        }

        public ObjectCollection<dynamic> ListenerCollection { get; } = new ObjectCollection<dynamic>();

        protected override void OnStop()
        {
            this.ListenerCollection.ForEach(l => l.Stop());
            base.OnStop();
        }
    }
}
