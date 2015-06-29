using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using RabbitMQ.Client.Framing;

namespace Bespoke.Sph.Messaging
{
    public partial class Broker
    {

        private Type[] LoadTypes(string f)
        {
            try
            {
                var dll = Assembly.LoadFile(f);
                return dll.GetTypes();
            }
            catch (ReflectionTypeLoadException e)
            {
                WebSocketNotificationService.Instance.WriteError(e, "Load Types Exception for " + f);
                return new Type[] { };
            }
        }

        private void CallOnStart(List<Subscriber> instances)
        {
            var onStart = typeof(Subscriber).GetMethod("OnStart", BindingFlags.NonPublic | BindingFlags.Instance);
            foreach (var sub in instances)
            {
                WriteInfo($"Starting {sub.QueueName} on {sub.GetType().Name}");
                WriteVerbose(string.Join(",", sub.RoutingKeys));
                sub.NotificicationService = WebSocketNotificationService.Instance;
                onStart.Invoke(sub, new object[] { });
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

            var starDot = list.Select(x => x.Replace("#", "*")).ToArray();
            list.AddRange(starDot);

            return list.Distinct().OrderBy(l => l).ToArray();
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



        private void SendMessage(string operation, Entity item, string crud, AuditTrail log, IDictionary<string, object> headers)
        {
            var type = item.GetEntityType().Name;
            var topic = $"{type}.{crud}.{operation}";
            if (string.IsNullOrWhiteSpace(operation))
                topic = $"{type}.{crud}.#";

            var possibleTopics = GetPossibleTopics(topic);


            foreach (var g in m_subscriptions)
            {
                if (g.Value.Any(s => possibleTopics.Contains(s)))
                {
                    var instance = (Subscriber)m_subscribers[g.Key];
                    Invoke(instance, item, crud, operation, log, headers);

                }
            }

            object listener;
            if (m_listeners.TryGetValue(item.GetEntityType(), out listener))
            {
                dynamic broadcast = listener;
                try
                {
                    broadcast.SendMessage(item, log);
                }
                catch (Exception e)
                {
                    ObjectBuilder.GetObject<ILogger>().Log(new LogEntry(e));
                }
            }
        }
        internal void RegisterListener<T>(ChangeListener<T> listener) where T : Entity
        {
            m_listeners.TryAdd(typeof(T), listener);
        }
        internal void RemoveListener<T>(ChangeListener<T> listener) where T : Entity
        {
            object list;
            m_listeners.TryRemove(typeof(T), out list);
        }

        private void Invoke(object sub, Entity item, string crud, string operation, AuditTrail log = null, IDictionary<string, object> headers = null)
        {
            var ds = ObjectBuilder.GetObject<IDirectoryService>();
            var logger = ObjectBuilder.GetObject<ILogger>();

            var args = new ReceivedMessageArgs { Properties = new BasicProperties { Headers = new Dictionary<string, object>() } };
            args.Properties.Headers.Add("crud", Encoding.UTF8.GetBytes(crud));
            args.Properties.Headers.Add("username", Encoding.UTF8.GetBytes(ds.CurrentUserName));
            args.Properties.Headers.Add("operation", Encoding.UTF8.GetBytes(operation));
            if (null != log)
                args.Properties.Headers.Add("log", log.ToJsonString(false));
            if (null != headers)
            {
                foreach (var h in headers)
                {
                    args.Properties.Headers.Add(h.Key, h.Value);
                }
            }

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
                WebSocketNotificationService.Instance.WriteError($"Cannot find ProcessMessage in type {sub.GetType().GetShortAssemblyQualifiedName()}");
            }
        }


        private void WriteInfo(string message)
        {
            WebSocketNotificationService.Instance.WriteInfo(message);
        }

        private void WriteVerbose(string message)
        {
            WebSocketNotificationService.Instance.WriteVerbose(message);
        }
        private void WriteError(Exception ex, string message)
        {
            WebSocketNotificationService.Instance.WriteError(ex, message);
        }


    }
}