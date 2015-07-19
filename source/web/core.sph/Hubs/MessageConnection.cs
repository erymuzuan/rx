using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Microsoft.AspNet.SignalR;

namespace Bespoke.Sph.Web.Hubs
{
    public class MessageConnection : PersistentConnection
    {
        private IEntityChangedListener<Message> m_listener;
        private readonly ConcurrentDictionary<string, string> m_connections = new ConcurrentDictionary<string, string>();
        protected override Task OnConnected(IRequest request, string connectionId)
        {
            m_listener?.Stop();
            m_listener = ObjectBuilder.GetObject<IEntityChangedListener<Message>>();
            m_listener.Changed += ListenerChanged;
            m_listener.Run();

            var user = request?.User?.Identity?.Name;
            if (!string.IsNullOrWhiteSpace(user))
                m_connections.TryAdd(request.User.Identity.Name, connectionId);

            return Connection.Send(connectionId, $"You are now connected to messaging connection");
        }

        async void ListenerChanged(object sender, EntityChangedEventArgs<Message> e)
        {
            var conn = "";
            if (!m_connections.TryGetValue(e.Item.UserName, out conn)) return;


            var context = new SphDataContext();
            var query = context.CreateQueryable<Message>()
                .Where(x => x.UserName == e.Item.UserName && x.IsRead == false)
                .OrderByDescending(x => x.ChangedDate);
            var lo = await context.LoadAsync(query, 1, 5, true);

            var data = new { message = e.Item, messages = lo.ItemCollection, unread = lo.TotalRows };
            await Connection.Send(conn, data);
        }

    

    }
}