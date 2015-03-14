using System;
using System.Collections.Generic;
using System.Linq;
using Bespoke.Sph.RabbitMqPublisher;
using Newtonsoft.Json;
using RabbitMQ.Client;
using SuperSocket.WebSocket;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    [Serializable]
    public class ConsoleNotification : INotificationService
    {
        private readonly IBrokerConnection m_connectionInfo;
        private IConnection m_connection;
        private IModel m_channel;
        public const int PERSISTENT_DELIVERY_MODE = 2;

        public string Exchange { get; set; }
        public bool IsOpened { get; set; }

        public ConsoleNotification(IBrokerConnection connectionInfo)
        {
            m_connectionInfo = connectionInfo;
            this.Exchange = "sph.topic";
        }
       

        private void SendMessage(string message, string severity)
        {
            var arg = new { message, severity };
            var json = JsonConvert.SerializeObject(arg);

            if (!this.IsOpened)
                InitConnection();


            var routingKey = "logger." + severity;
            var body = System.Text.Encoding.Default.GetBytes(json);

            var props = m_channel.CreateBasicProperties();
            props.DeliveryMode = PERSISTENT_DELIVERY_MODE;
            props.SetPersistent(false);
            props.ContentType = "application/json";
            props.Headers = new Dictionary<string, object> { { "severity", severity } };
       
            m_channel.BasicPublish(this.Exchange, routingKey, props, body);

        }

        private void InitConnection()
        {

            var factory = new ConnectionFactory
            {
                UserName = m_connectionInfo.UserName,
                Password = m_connectionInfo.Password,
                HostName = m_connectionInfo.Host,
                Port = m_connectionInfo.Port,
                VirtualHost = m_connectionInfo.VirtualHost
            };
            m_connection = factory.CreateConnection();
            m_channel = m_connection.CreateModel();

            m_channel.ExchangeDeclare(this.Exchange, ExchangeType.Topic, true);
            this.IsOpened = true;
        }


   

        public void Write(string format, params object[] args)
        {
            try
            {
                this.SendMessage(string.Format(format, args), "info");
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("========== {0} : {1,12:hh:mm:ss.ff} ===========", "Infomation ", DateTime.Now);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(format, args);
                Console.WriteLine();
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void WriteError(string format, params object[] args)
        {
            try
            {
                this.SendMessage(string.Format(format, args), "error");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(format, args);
                Console.WriteLine();
            }
            finally
            {
                Console.ResetColor();
            }
        }
    }
}
