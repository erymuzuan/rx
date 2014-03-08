using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Microsoft.AspNet.SignalR;

namespace Bespoke.Sph.Web.Hubs
{
    public class MessageConnection : PersistentConnection
    {
        private IEntityChangedListener<Message> m_listener;
        protected override Task OnConnected(IRequest request, string connectionId)
        {
            if (null != m_listener)
            {
                m_listener.Stop();
            }
            m_listener = ObjectBuilder.GetObject<IEntityChangedListener<Message>>();
            m_listener.Changed += ListenerChanged;
            m_listener.Run();

            return Connection.Send(connectionId, (new Message { Subject = "Welcome" }).ToJsonString());
        }

        void ListenerChanged(object sender, EntityChangedEventArgs<Message> e)
        {
            var json = e.Item.ToJsonString();
            Connection.Broadcast(json);
        }

        protected override Task OnReceived(IRequest request, string connectionId, string data)
        {
            return Connection.Broadcast(data);
        }


    }
}