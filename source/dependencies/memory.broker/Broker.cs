using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
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
    public class Broker : IEntityChangePublisher, IDisposable
    {
        private readonly WebSocketNotificationService m_notificationService = new WebSocketNotificationService();
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

        private Type[] LoadTypes(string f)
        {
            try
            {
                var dll = Assembly.LoadFile(f);
                return dll.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                m_notificationService.WriteError(e, "Load Types Exception for " + f);
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
            m_notificationService.Start();
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

        public void Dispose()
        {
            m_notificationService.Stop();
        }

        public Task PublishAdded(string operation, IEnumerable<Entity> attachedCollection, IDictionary<string, object> headers)
        {
            foreach (var item in attachedCollection)
            {
                var log = new AuditTrail { Type = item.GetType().Name, EntityId = item.Id, Id = Guid.NewGuid().ToString(), Operation = operation, Note = "Added" };
                SendMessage(operation, item, "added", log);
            }
            return Task.FromResult(0);
        }



        public Task PublishChanges(string operation, IEnumerable<Entity> attachedCollection, IEnumerable<AuditTrail> logs, IDictionary<string, object> headers)
        {
            var allLogs = logs.ToList();
            foreach (var item in attachedCollection)
            {
                var audit = allLogs.SingleOrDefault(x => x.EntityId == item.Id && x.Type == item.GetType().Name);
                SendMessage(operation, item, "changed", audit);
            }
            return Task.FromResult(0);
        }

        private void SendMessage(string operation, Entity item, string crud, AuditTrail log)
        {
            var type = item.GetType().Name;
            var topic = $"{type}.{crud}.{operation}";
            var possibleTopics = GetPossibleTopics(topic);


            foreach (var g in m_subsriptions)
            {
                if (g.Value.Any(s => possibleTopics.Contains(s)))
                {
                    var instance = (Subscriber)m_subsribers[g.Key];
                    if (null == instance.NotificicationService)
                        instance.NotificicationService = m_notificationService;

                    Invoke(instance, item, crud, operation);

                }
            }

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

        private void Invoke(object sub, Entity item, string crud, string operation)
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

                WriteVerbose($"\tProcessMessage {item.GetType().Name}({operation}) in {sub.GetType().Name}");
                var task = pm.Invoke(sub, new object[] { item, headers2 }) as Task;
                task?.ContinueWith(_ =>
                {
                    var e = _.Exception;
                    if (null != _.Exception)
                    {
                        var otherInfo = new[] {
                            $"Fail to execute ProcessMessage on {sub.GetType().GetShortAssemblyQualifiedName()}",
                            "Operation :" + operation,
                            "CRUD : " + crud,
                            "ItemType : " + item.GetType().GetShortAssemblyQualifiedName(),
                            "ItemId : " + item.Id };
                        logger.Log(new LogEntry(e, otherInfo));
                    }
                })
                .Wait(TimeSpan.FromSeconds(this.WaitTimeForProcessMessage));

            }
            else
            {
                m_notificationService.WriteError($"Cannot find ProcessMessage in type {sub.GetType().GetShortAssemblyQualifiedName()}");
            }
        }

        public double WaitTimeForProcessMessage { get; set; } = 5;

        public Task PublishDeleted(string operation, IEnumerable<Entity> deletedCollection, IDictionary<string, object> headers)
        {
            foreach (var item in deletedCollection)
            {
                var log = new AuditTrail { Type = item.GetType().Name, EntityId = item.Id, Id = Guid.NewGuid().ToString(), Operation = operation, Note = "Delete" };
                SendMessage(operation, item, "deleted", log);
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

        private void WriteInfo(string message)
        {
            m_notificationService.WriteInfo(message);
        }

        private void WriteVerbose(string message)
        {
            m_notificationService.WriteVerbose(message);
        }

        public async Task SubmitChangesAsync(string operation, IEnumerable<Entity> attachedEntities, IEnumerable<Entity> deletedEntities, IDictionary<string, object> headers)
        {
            var userName = ObjectBuilder.GetObject<IDirectoryService>().CurrentUserName;
            var entities = attachedEntities.ToList();
            var deletedItems = deletedEntities.ToArray();
            WriteInfo($"{operation} for {"item".ToQuantity(entities.Count)}");
            foreach (var item in entities)
            {
                WriteInfo($"{operation} for {item.GetType().Name}{{ Id : \"{item.Id}\"}}");
            }
            foreach (var item in deletedItems)
            {
                WriteInfo($@"Deleting({operation}) {item.GetType().Name}{{ Id : ""{item.Id}""}}");
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
            WriteInfo($"SubmitChanges => {"row".ToQuantity(so.RowsAffected)} affected, faulted: {so.IsFaulted}");
            if(so.IsFaulted || null != so.Exeption)
            {
                m_notificationService.WriteError(so.Exeption, "Exception in call persistence.SubmitChanges");
            }


            var logsAddedTask = this.PublishAdded(operation, logs, headers);
            var addedTask = this.PublishAdded(operation, addedItems, headers);
            var changedTask = this.PublishChanges(operation, changedItems, logs, headers);
            var deletedTask = this.PublishDeleted(operation, deletedItems, headers);
            await Task.WhenAll(addedTask, changedTask, deletedTask, logsAddedTask).ConfigureAwait(false);
        }
    }
}