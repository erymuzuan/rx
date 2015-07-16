using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Microsoft.AspNet.SignalR;

namespace Bespoke.Sph.Web.Hubs
{
    public class AuditTrailHub : Hub
    {
        private IEntityChangedListener<AuditTrail> m_listener;
        public override Task OnConnected()
        {
            m_listener?.Stop();
            m_listener = ObjectBuilder.GetObject<IEntityChangedListener<AuditTrail>>();
            m_listener.Changed += ListenerChanged;
            m_listener.Run();

            return base.OnConnected();
            //return Connection.Send(connectionId, (new Message { Subject = "Welcome" }).ToJsonString());
        }

        void ListenerChanged(object sender, EntityChangedEventArgs<AuditTrail> e)
        {
            var json = e.Item.ToJsonString();
            Clients.All.publishChanges(json, e.AuditTrail.Operation);
        }

        public void Register(string entity, int id)
        {
            
        }
      
        public void Hello()
        {
            Clients.All.hello();
        }
    }
}