using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.ControlCenter.Model;
using Bespoke.Sph.SubscribersInfrastructure;
using RabbitMQ.Client;
using SuperSocket.WebSocket;


namespace Bespoke.Sph.ControlCenter
{
    public class ConsoleNotificationSubscriber
    {
        private readonly SphSettings m_settings;
        public ConsoleNotificationSubscriber(SphSettings settings)
        {
            m_settings = settings;
        }

        private WebSocketServer m_appServer;
        public string[] RoutingKeys => new[] { "logger.#" };
        public string QueueName => "console_logger";

        private TaskCompletionSource<bool> m_stoppingTcs;
        public bool Start(int port = 50230)
        {
            m_appServer = new WebSocketServer();
            if (!m_appServer.Setup(port))
            {
                return false;
            }
            m_appServer.NewMessageReceived += NewMessageReceived;
            return m_appServer.Start();
        }

        public bool Stop()
        {
            m_appServer.Stop();
            return true;
        }

        public void Listen()
        {

            try
            {
                m_stoppingTcs = new TaskCompletionSource<bool>();
                this.StartConsume();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        protected void OnStop()
        {
            m_consumer.Received -= Received;
            m_stoppingTcs?.SetResult(true);

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

        }

        private void NewMessageReceived(WebSocketSession session, string value)
        {
            Console.WriteLine("Getting new message from {0} => {1}", session.SessionID, value);
            // session.Send("[{\"name\" : \"test01\"}]");

        }

        private IConnection m_connection;
        private IModel m_channel;
        private TaskBasicConsumer m_consumer;
        private int m_processing;

        public void StartConsume()
        {
            const bool NO_ACK = false;
            const string EXCHANGE_NAME = "sph.topic";
            const string DEAD_LETTER_EXCHANGE = "sph.ms-dead-letter";


            var factory = new ConnectionFactory
            {
                UserName = m_settings.RabbitMqUserName ?? "guest",
                VirtualHost = m_settings.ApplicationName,
                Password = m_settings.RabbitMqPassword ?? "guest",
                HostName = m_settings.RabbitMqHost ?? "localhost",
                Port = m_settings.RabbitMqPort ?? 5672
            };
            m_connection = factory.CreateConnection();
            m_channel = m_connection.CreateModel();


            m_channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic, true);
            m_channel.ExchangeDeclare(DEAD_LETTER_EXCHANGE, ExchangeType.Topic, true);
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
            catch (Exception)
            {
                m_channel.BasicReject(e.DeliveryTag, false);
            }
            finally
            {
                Interlocked.Decrement(ref m_processing);
            }
        }




    }
}
