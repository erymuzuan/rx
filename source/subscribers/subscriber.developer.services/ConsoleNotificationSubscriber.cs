using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.SubscribersInfrastructure;
using RabbitMQ.Client;
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

        private TaskCompletionSource<bool> m_stoppingTcs;
        public override void Run()
        {
            try
            {
                RegisterServices();
                PrintSubscriberInformation();
                m_stoppingTcs = new TaskCompletionSource<bool>();
                this.StartConsume();
            }
            catch (Exception e)
            {
                this.WriteError(e);
            }

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
        protected override void OnStop()
        {
            this.WriteMessage("!!Stoping : {0}", this.QueueName);

            m_consumer.Received -= Received;
            if (null != m_stoppingTcs)
                m_stoppingTcs.SetResult(true);

            while (m_processing > 0)
            {

            }

            if (null != m_connection)
            {
                m_connection.Close();
                m_connection.Dispose();
                m_connection = null;
            }

            if (null != m_channel)
            {
                m_channel.Close();
                m_channel.Dispose();
                m_channel = null;
            }

            this.WriteMessage("!!Stopped : {0}", this.QueueName);
        }
        private void NewMessageReceived(WebSocketSession session, string value)
        {

            // 
        }

        private IConnection m_connection;
        private IModel m_channel;
        private TaskBasicConsumer m_consumer;
        private int m_processing;

        public void StartConsume()
        {
            const bool NO_ACK = false;
            const string EXCHANGE_NAME = "sph.topic";

            this.OnStart();

            var factory = new ConnectionFactory
            {
                UserName = this.UserName,
                VirtualHost = this.VirtualHost,
                Password = this.Password,
                HostName = this.HostName,
                Port = this.Port
            };
            m_connection = factory.CreateConnection();
            m_channel = m_connection.CreateModel();


            m_channel.QueueDeclare(this.QueueName, false, false, false, new Dictionary<string, object>());

            foreach (var s in this.RoutingKeys)
            {
                m_channel.QueueBind(this.QueueName, EXCHANGE_NAME, s, null);
            }
            m_channel.BasicQos(0, 1, false);

            m_consumer = new TaskBasicConsumer(m_channel);
            m_consumer.Received += Received;
            m_channel.BasicConsume(this.QueueName, NO_ACK, m_consumer);

        }

        private void Received(object sender, ReceivedMessageArgs e)
        {
            Interlocked.Increment(ref m_processing);
            var body = e.Body;
            var json = Encoding.Default.GetString(body);
            try
            {

                m_appServer.GetAllSessions().ToList().ForEach(x => x.Send(json));
                m_channel.BasicAck(e.DeliveryTag, false);
            }
            catch (Exception exc)
            {
                this.WriteMessage("Error in {0}", this.GetType().Name);
                this.WriteError(exc);
                m_channel.BasicReject(e.DeliveryTag, false);
            }
            finally
            {
                Interlocked.Decrement(ref m_processing);
            }
        }






        public static string ParseArg(string name)
        {
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
            return null == val ? null : val.Replace("/" + name + ":", string.Empty);
        }
    }
}
