using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.WathersSubscribers
{
    public class WatcherSubscriber : Subscriber<Watcher>
    {
        public override string QueueName => "watcher_queue";

        public override string[] RoutingKeys => new[] { "Watcher.#.#" };

        protected override Task ProcessMessage(Watcher item, MessageHeaders header)
        {
            this.WriteMessage("A watcher has been {0} : \r\n{1}", header.Crud, item);
            if (header.Crud == CrudOperation.Added)
            {
                m_watchers.Add(item);
            }
            if (header.Crud == CrudOperation.Deleted)
            {
                m_watchers.RemoveAll(w => w.Id == item.Id);
            }
            if (header.Crud == CrudOperation.Changed)
            {
                m_watchers.RemoveAll(w => w.Id == item.Id);
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
                var edAssembly = Assembly.Load(ConfigurationManager.ApplicationName + "." + ed1.Name);
                var edTypeName = $"Bespoke.{ConfigurationManager.ApplicationName}_{ed1.Id}.Domain.{ed1.Name}";
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
                this.WriteError(new Exception(e.Message));
            }

            return null;


        }

        public async void EntityChanged(object sender, dynamic e)
        {
            var entityName = e.Item.GetType().Name;
            var id = e.Item.Id;
            var query = @"{
   ""query"": {
      ""filtered"": {
         ""filter"": {
            ""bool"": {
               ""must"": [
                  {
                     ""term"": {
                        ""EntityName"": ""<EntityName>""
                     }
                  },
                  {
                      ""term"": {
                         ""EntityId"": ""<EntityId>""
                      }
                  }
               ],
               ""must_not"": []
            }
         }
      }
   }
}".Replace("<EntityName>", entityName)
  .Replace("<EntityId>", id);

            var request = new StringContent(query);
            var url = $"{ConfigurationManager.ApplicationName.ToLowerInvariant()}_sys/watcher/_search";

            var watchers = new ObjectCollection<string>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);

                var response = await client.PostAsync(url, request);
                var content = response.Content as StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es " + request);

                var text = await content.ReadAsStringAsync();
                var jo = JObject.Parse(text);
                var tokens = jo.SelectToken("$.hits.hits");
                if (null == tokens)
                {
                    this.WriteMessage("No getting the expected json from watcher _search !!!!!!!");
                    return;
                }
                var list = from t in tokens
                           let user = t.SelectToken("$._source.User")
                           where null != user
                           select user.Value<string>();
                watchers.ClearAndAddRange(list);

            }

            this.WriteMessage("Changed to " + e);

            this.WriteMessage("There {0} watchers", watchers.Count);
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
