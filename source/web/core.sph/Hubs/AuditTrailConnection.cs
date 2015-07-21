using System.Collections.Concurrent;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Microsoft.AspNet.SignalR;

namespace Bespoke.Sph.Web.Hubs
{
    public class AuditTrailConnection : PersistentConnection
    {
        private readonly IEntityChangedListener<AuditTrail> m_listener;
        private readonly ConcurrentDictionary<string, string> m_watchers = new ConcurrentDictionary<string, string>();

        public AuditTrailConnection()
        {

            m_listener?.Stop();
            m_listener = ObjectBuilder.GetObject<IEntityChangedListener<AuditTrail>>();
            m_listener.Changed += ListenerChanged;
            m_listener.Run();
        }
        protected override Task OnConnected(IRequest request, string connectionId)
        {
            if (!request.User.Identity.IsAuthenticated)
                return base.OnConnected(request, connectionId);

            var key = request.QueryString["type"] + ":" + request.QueryString["id"];  
            m_watchers.TryAdd(key, connectionId);
            return base.OnConnected(request, connectionId);
        }

        protected override Task OnDisconnected(IRequest request, string connectionId, bool stopCalled)
        {
            if (!request.User.Identity.IsAuthenticated)
                return base.OnDisconnected(request, connectionId, stopCalled);

            var key = request.QueryString["type"] + ":" + request.QueryString["id"];
            var conn = "";
            m_watchers.TryRemove(key, out conn);
            return base.OnDisconnected(request, connectionId, stopCalled);
        }

        void ListenerChanged(object sender, EntityChangedEventArgs<AuditTrail> e)
        {
            var key = $"{e.Item.Type}:{e.Item.EntityId}";
            var conn = "";
            if (m_watchers.TryGetValue(key, out conn))
            {
                this.Connection.Send(conn, e.Item);
            }
        }

    }
}