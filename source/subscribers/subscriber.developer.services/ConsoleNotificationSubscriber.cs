using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using SuperSocket.WebSocket;

namespace subscriber.developer.services
{
    public class ConsoleNotificationSubscriber : Subscriber
    {
        [NonSerialized]
        private WebSocketServer m_appServer;

        public override string[] RoutingKeys
        {
            get { return new []{"logger.#"}; }
        }

        public override string QueueName
        {
            get { return "console_logger"; }
        }

        public override void Run()
        {
            int port;
            if (!int.TryParse(ParseArg("console.port"), out port))
            {
                port = 5030;
            }

            m_appServer = new WebSocketServer();
            if (!m_appServer.Setup(port))
            {
                //this.WriteError("Console Notification: Failed to setup WebSocket Server!");
                return;
            }
            m_appServer.NewMessageReceived += NewMessageReceived;

            if (!m_appServer.Start())
            {
                //this.WriteError("ConsoleNotification: Failed to start websocket server!");
            }
        }

        private void NewMessageReceived(WebSocketSession session, string value)
        {

            // m_appServer.GetAllSessions().ToList().ForEach(e => e.Send(json));
        }

        public static string ParseArg(string name)
        {
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
            if (null == val) return null;
            return val.Replace("/" + name + ":", string.Empty);
        }
    }
}
