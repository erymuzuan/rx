using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Humanizer;
using RabbitMQ.Client.Framing;

namespace Bespoke.Sph.Messaging
{
    public class Broker : IEntityChangePublisher
    {
        private readonly Dictionary<string, string[]> m_subsriptions = new Dictionary<string, string[]>();
        private readonly Dictionary<string, object> m_subsribers = new Dictionary<string, object>();
        public Broker(IDictionary<object, string[]> subsriptions)
        {
            foreach (var sb in subsriptions)
            {
                var type = sb.Key.GetType().GetShortAssemblyQualifiedName();
                if (!m_subsriptions.ContainsKey(type))
                    m_subsriptions.Add(type, sb.Value);
                if (!m_subsribers.ContainsKey(type))
                    m_subsribers.Add(type, sb.Key);
            }
        }

        private static Type[] LoadTypes(string f)
        {
            try
            {
                var dll = Assembly.LoadFile(f);
                return dll.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                Console.WriteLine(e);
                return new Type[] { };
            }
        }
        public Broker()
        {
            var types = from f in Directory.GetFiles(ConfigurationManager.SubscriberPath, "subscriber.*.dll")
                        let fileName = Path.GetFileName(f)
                        where fileName != "subscriber.persistence.dll" && fileName != "subscriber.infrastructure.dll"
                        let subscribers = LoadTypes(f)
                                .Where(x => x.Name.EndsWith("Subscriber"))
                                .Where(x => !x.IsAbstract)
                        select subscribers;

            var all = types.SelectMany(x => x.ToArray()).ToList();

            var instances = (from s in all
                             select Activator.CreateInstance(s)).ToList();
            foreach (var sb in instances)
            {
                dynamic sb1 = sb;
                var type = sb.GetType().GetShortAssemblyQualifiedName();
                if (!m_subsriptions.ContainsKey(type))
                    m_subsriptions.Add(type, sb1.RoutingKeys);
                if (!m_subsribers.ContainsKey(type))
                    m_subsribers.Add(type, sb);

            }
        }


        // NOTE : this is good up to 4 level topic, i.e a.b.c.d
        private string[] GetPossibleTopics(string topic)
        {
            var topics = topic.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            var list = new List<string>();
            foreach (var t in topics)
            {
                var k = topic.Replace(t, "#");
                list.Add(k);
                foreach (var m in topics)
                {
                    var n = k.Replace(m, "#");
                    list.Add(n);
                    list.AddRange(topics.Select(x => n.Replace(x, "#")));
                }
            }

            var stars = list.Select(x => x.Replace("#.#", "*")).ToArray();
            var stars2 = list.Select(x => x.Replace("#.#.#", "*")).ToArray();
            list.AddRange(stars);
            list.AddRange(stars2);
            list.Add("*");
            list.Add(topic);
            list.Add(string.Join(".", Enumerable.Range(1, topics.Length).Select(x => "#")));// #.#.#

            return list.Distinct().OrderBy(l => l).ToArray();
        }

        private string m_keys = "";
        public override string ToString()
        {
            return m_keys;
        }

        public Task PublishAdded(string operation, IEnumerable<Entity> attachedCollection, IDictionary<string, object> headers)
        {
            m_keys = "";
            foreach (var item in attachedCollection)
            {
                var log = new AuditTrail { Type = item.GetType().Name, EntityId = item.Id, Id = Guid.NewGuid().ToString(), Operation = operation, Note = "Added" };
                SendMessage(operation, headers, item, "added",log);
            }
            return Task.FromResult(0);
        }



        public Task PublishChanges(string operation, IEnumerable<Entity> attachedCollection, IEnumerable<AuditTrail> logs, IDictionary<string, object> headers)
        {
            m_keys = "";
            var allLogs = logs.ToList();
            foreach (var item in attachedCollection)
            {
                var audit = allLogs.SingleOrDefault(x => x.EntityId == item.Id && x.Type == item.GetType().Name);
                SendMessage(operation, headers, item, "changed", audit);
            }
            return Task.FromResult(0);
        }

        private void SendMessage(string operation, IDictionary<string, object> headers, Entity item, string crud, AuditTrail log)
        {
            var type = item.GetType().Name;
            var topic = $"{type}.{crud}.{operation}";
            var possibleTopics = GetPossibleTopics(topic);

            var subsribers = new List<string>();

            foreach (var g in m_subsriptions)
            {
                if (g.Value.Any(s => possibleTopics.Contains(s)))
                {
                    dynamic k = m_subsribers[g.Key];
                    subsribers.Add(k.QueueName);
                    var instance = m_subsribers[g.Key];
                    Invoke(instance, item, headers, crud, operation);
                }
            }
            m_keys = string.Join(";", subsribers);

            object listener;
            if (m_listeners.TryGetValue(item.GetType(), out listener))
            {
                dynamic broadcast = listener;
                broadcast.SendMessage(item, log);
            }
        }
        private readonly ConcurrentDictionary<Type, object> m_listeners = new ConcurrentDictionary<Type, object>();
        internal void RegisterListener<T>(ChangeListener<T> listener) where T : Entity
        {
            m_listeners.TryAdd(typeof(T), listener);
        }
        internal void RemoveListener<T>(ChangeListener<T> listener) where T : Entity
        {
            object list;
            m_listeners.TryRemove(typeof(T), out list);
        }

        private void Invoke(object sub, Entity item, IDictionary<string, object> headers, string crud, string operation)
        {
            var ds = ObjectBuilder.GetObject<IDirectoryService>();
            var logger = ObjectBuilder.GetObject<ILogger>();

            var args = new ReceivedMessageArgs { Properties = new BasicProperties { Headers = new Dictionary<string, object>() } };
            args.Properties.Headers.Add("crud", Encoding.UTF8.GetBytes(crud));
            args.Properties.Headers.Add("username", Encoding.UTF8.GetBytes(ds.CurrentUserName));
            args.Properties.Headers.Add("operation", Encoding.UTF8.GetBytes(operation));
            var headers2 = new MessageHeaders(args);

            var pm = sub.GetType().GetMethod("ProcessMessage", BindingFlags.NonPublic | BindingFlags.Instance);
            if (null != pm)
            {
                try
                {
                    pm.Invoke(sub, new object[] { item, headers2 });
                }
                catch (Exception e)
                {
                    logger.Log(new LogEntry(e, new[] { $"Fail to execute ProcessMessage on {sub.GetType().GetShortAssemblyQualifiedName()}", operation, crud, item.ToJsonString(true) }));
                    Console.WriteLine(e);
                }
            }
            else
            {
                Console.WriteLine($"Cannot find ProcessMessage in type {sub.GetType().GetShortAssemblyQualifiedName()}");
            }
        }

        public Task PublishDeleted(string operation, IEnumerable<Entity> deletedCollection, IDictionary<string, object> headers)
        {
            m_keys = "";
            foreach (var item in deletedCollection)
            {
                var log = new AuditTrail { Type = item.GetType().Name, EntityId = item.Id, Id = Guid.NewGuid().ToString(), Operation = operation, Note = "Delete" };
                SendMessage(operation, headers, item, "deleted", log);
            }
            return Task.FromResult(0);
        }

        private async Task<IEnumerable<Entity>> GetPreviousItems(IEnumerable<Entity> items)
        {
            var list = new ObjectCollection<Entity>();
            foreach (var item in items)
            {
                var o1 = item;
                var type = item.GetEntityType();
                var reposType = typeof(IRepository<>).MakeGenericType(type);
                var repos = ObjectBuilder.GetObject(reposType);

                var p = await repos.LoadOneAsync(o1.Id).ConfigureAwait(false);
                if (null != p)
                    list.Add(p);


            }
            return list;
        }
        public async Task SubmitChangesAsync(string operation, IEnumerable<Entity> attachedEntities, IEnumerable<Entity> deletedEntities, IDictionary<string, object> headers)
        {
            var userName = ObjectBuilder.GetObject<IDirectoryService>().CurrentUserName;
            var entities = attachedEntities.ToList();
            var deletedItems = deletedEntities.ToArray();
            Console.WriteLine($"{operation} for {"item".ToQuantity(entities.Count)}");
            foreach (var item in entities)
            {
                Console.WriteLine($"{operation} for {item.GetType().Name}{{ Id : \"{item.Id}\"}}");
            }
            foreach (var item in deletedItems)
            {
                Console.WriteLine($@"Deleting({operation}) {item.GetType().Name}{{ Id : ""{item.Id}""}}");
            }
            // get changes to items
            var previous = await GetPreviousItems(entities);
            var logs = (from r in entities
                        let e1 = previous.SingleOrDefault(t => t.Id == r.Id)
                        where null != e1
                        let diffs = (new ChangeGenerator().GetChanges(e1, r))
                        let logId = Guid.NewGuid().ToString()
                        select new AuditTrail(diffs)
                        {
                            Operation = operation,
                            DateTime = DateTime.Now,
                            User = userName,
                            Type = r.GetType().Name,
                            EntityId = r.Id,
                            Id = logId,
                            WebId = logId,
                            Note = "-"
                        }).ToArray();
            var addedItems = (from r in entities
                              let e1 = previous.SingleOrDefault(t => t.Id == r.Id)
                              where null == e1
                              select r).ToArray();
            var changedItems = (from r in entities
                                let e1 = previous.SingleOrDefault(t => t.Id == r.Id)
                                where null != e1
                                select r).ToArray();
            entities.AddRange(logs);

            var persistence = ObjectBuilder.GetObject<IPersistence>();
            var so = await persistence.SubmitChanges(entities, deletedItems, null, userName)
            .ConfigureAwait(false);
            Debug.WriteLine(so);


            var logsAddedTask = this.PublishAdded(operation, logs, headers);
            var addedTask = this.PublishAdded(operation, addedItems, headers);
            var changedTask = this.PublishChanges(operation, changedItems, logs, headers);
            var deletedTask = this.PublishDeleted(operation, deletedItems, headers);
            await Task.WhenAll(addedTask, changedTask, deletedTask, logsAddedTask).ConfigureAwait(false);
        }
    }
}