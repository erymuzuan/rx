using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Humanizer;

namespace Bespoke.Sph.Messaging
{
    public partial class Broker : IEntityChangePublisher, IDisposable
    {
        public int WebSocketPort { get; set; } = 50238;
        public double WaitTimeForProcessMessage { get; set; } = 5;

        private readonly ConcurrentDictionary<Type, object> m_listeners = new ConcurrentDictionary<Type, object>();
        private readonly Dictionary<string, string[]> m_subscriptions = new Dictionary<string, string[]>();
        private readonly Dictionary<string, object> m_subscribers = new Dictionary<string, object>();
        //
        public Broker(IDictionary<object, string[]> subsriptions)
        {
            foreach (var sb in subsriptions)
            {
                var type = sb.Key.GetType().GetShortAssemblyQualifiedName();
                if (!m_subscriptions.ContainsKey(type))
                    m_subscriptions.Add(type, sb.Value);
                if (!m_subscribers.ContainsKey(type))
                    m_subscribers.Add(type, sb.Key);
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
                if (!m_subscriptions.ContainsKey(type))
                    m_subscriptions.Add(type, sb1.RoutingKeys);
                if (!m_subscribers.ContainsKey(type))
                    m_subscribers.Add(type, sb);

            }
            var started = WebSocketNotificationService.Instance.Start(this.WebSocketPort);
            if(!started)
                WriteError(new Exception("Fail to start websocket server on port " + this.WebSocketPort ),"" );
            this.QueueUserWorkItem(CallOnStart, instances.Cast<Subscriber>().ToList());
        }

        public void Dispose()
        {
            WebSocketNotificationService.Instance.Stop();
        }

        public Task PublishAdded(string operation, IEnumerable<Entity> attachedCollection, IDictionary<string, object> headers)
        {
            foreach (var item in attachedCollection)
            {
                var log = new AuditTrail { Type = item.GetType().Name, EntityId = item.Id, Id = Guid.NewGuid().ToString(), Operation = operation, Note = "Added" };
                SendMessage(operation, item, "added", log, headers);
            }
            return Task.FromResult(0);
        }


        public Task PublishChanges(string operation, IEnumerable<Entity> attachedCollection, IEnumerable<AuditTrail> logs, IDictionary<string, object> headers)
        {
            var allLogs = logs.ToList();
            foreach (var item in attachedCollection)
            {
                var audit = allLogs.SingleOrDefault(x => x.EntityId == item.Id && x.Type == item.GetType().Name);
                SendMessage(operation, item, "changed", audit, headers);
            }
            return Task.FromResult(0);
        }


        public Task PublishDeleted(string operation, IEnumerable<Entity> deletedCollection, IDictionary<string, object> headers)
        {
            foreach (var item in deletedCollection)
            {
                var log = new AuditTrail { Type = item.GetType().Name, EntityId = item.Id, Id = Guid.NewGuid().ToString(), Operation = operation, Note = "Delete" };
                SendMessage(operation, item, "deleted", log, headers);
            }
            return Task.FromResult(0);
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
            if (so.IsFaulted || null != so.Exeption)
            {
                WriteError(so.Exeption, "Exception in call persistence.SubmitChanges");
            }


            var logsAddedTask = this.PublishAdded(operation, logs, headers);
            var addedTask = this.PublishAdded(operation, addedItems, headers);
            var changedTask = this.PublishChanges(operation, changedItems, logs, headers);
            var deletedTask = this.PublishDeleted(operation, deletedItems, headers);
            await Task.WhenAll(addedTask, changedTask, deletedTask, logsAddedTask).ConfigureAwait(false);
        }
    }
}