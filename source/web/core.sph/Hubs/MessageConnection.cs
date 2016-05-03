using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Microsoft.AspNet.SignalR;

namespace Bespoke.Sph.Web.Hubs
{
    public class MessageConnection : PersistentConnection, IDisposable
    {
        private readonly IEntityChangedListener<Message> m_listener;
        private readonly ConcurrentDictionary<string, IList<string>> m_connections = new ConcurrentDictionary<string, IList<string>>();

        public MessageConnection()
        {
            m_listener?.Stop();
            m_listener = ObjectBuilder.GetObject<IEntityChangedListener<Message>>();
            m_listener.Changed += ListenerChanged;
            m_listener.Run();

        }
        protected override Task OnConnected(IRequest request, string connectionId)
        {

            var user = request?.User?.Identity?.Name;
            if (string.IsNullOrWhiteSpace(user))
                return Task.FromResult(0);

            IList<string> connections;
            if (m_connections.TryGetValue(user, out connections))
            {
                var list = new List<string>(connections.ToArray()) { connectionId };
                m_connections.TryUpdate(user, list, connections);
            }
            else
            {
                m_connections.TryAdd(user, new List<string> { connectionId });
            }

            return Connection.Send(connectionId, $"You are now connected to messaging connection");
        }

        protected override Task OnDisconnected(IRequest request, string connectionId, bool stopCalled)
        {
            var user = request?.User?.Identity?.Name;
            if (string.IsNullOrWhiteSpace(user))
                return base.OnDisconnected(request, connectionId, stopCalled);
            IList<string> connections;
            if (m_connections.TryGetValue(user, out connections))
            {
                var list = new List<string>(connections);
                list.Remove(connectionId);
                m_connections.TryUpdate(user, list, connections);
            }
            return base.OnDisconnected(request, connectionId, stopCalled);
        }

        private async void ListenerChanged(object sender, EntityChangedEventArgs<Message> e)
        {
            IList<string> connections;
            if (!m_connections.TryGetValue(e.Item.UserName, out connections)) return;
            if (connections.Count == 0) return;

            var context = new SphDataContext();
            var query = context.CreateQueryable<Message>()
                .Where(x => x.UserName == e.Item.UserName && x.IsRead == false)
                .OrderByDescending(x => x.ChangedDate);
            var lo = await context.LoadAsync(query, 1, 5, true);

            var data = new { message = e.Item, messages = lo.ItemCollection, unread = lo.TotalRows };
            await Connection.Send(connections, data);
        }


        public void Dispose()
        {
            m_listener?.Stop();
        }
    }
}