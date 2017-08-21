using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using RabbitMQ.Client;

namespace Bespoke.Sph.RabbitMqPublisher
{
    public class MessageSlaManager : IMessageSlaManager, IDisposable
    {
        public const string SLA_NOTIFICATION_EXCHANGE = "sph.messages.sla";
        public string QueueName => "messages-sla";

        private IConnection m_connection;
        private IModel m_channel;
        public const int PERSISTENT_DELIVERY_MODE = 2;
        
        public async Task PublishSlaOnAcceptanceAsync(MessageSlaEvent @event)
        {
            if (!this.IsOpened)
                InitConnection();

            var headers = new Dictionary<string, object>();
            var body = await CompressAsync(@event.ToJson());
            var messageId = Guid.NewGuid().ToString("N").ToUpperInvariant();
            headers.AddIfNotExist("message-id", messageId);

            var props = m_channel.CreateBasicProperties();
            props.DeliveryMode = PERSISTENT_DELIVERY_MODE;
            props.ContentType = "application/json";
            props.Headers = headers;

            props.Expiration = @event.ProcessingTimeSpanInMiliseconds.ToString(CultureInfo.InvariantCulture);
            m_channel.BasicPublish(SLA_NOTIFICATION_EXCHANGE, QueueName, props, body);
        }



        private static async Task<byte[]> CompressAsync(string value)
        {
            var content = new byte[value.Length];
            var index = 0;
            foreach (var item in value)
            {
                content[index++] = (byte)item;
            }


            using (var ms = new MemoryStream())
            using (var sw = new GZipStream(ms, CompressionMode.Compress))
            {
                await sw.WriteAsync(content, 0, content.Length);
                //NOTE : DO NOT FLUSH cause bytes will go missing...
                sw.Close();

                content = ms.ToArray();
            }
            return content;
        }


        private  void InitConnection()
        {
            var factory = new ConnectionFactory
            {
                UserName = ConfigurationManager.RabbitMqUserName,
                Password = ConfigurationManager.RabbitMqPassword,
                HostName = ConfigurationManager.RabbitMqHost,
                Port = ConfigurationManager.RabbitMqPort,
                VirtualHost = ConfigurationManager.RabbitMqVirtualHost
            };
            m_connection = factory.CreateConnection();
            m_channel = m_connection.CreateModel();

        }

        public void Close()
        {
            m_connection?.Close();
            m_connection?.Dispose();
            m_connection = null;

            m_channel?.Close();
            m_channel?.Dispose();
            m_channel = null;
        }

        private bool IsOpened
        {
            get
            {
                if (null == m_connection) return false;
                if (null == m_channel) return false;
                if (null == m_connection) return false;
                if (!m_channel.IsOpen) return false;
                if (!m_connection.IsOpen) return false;

                return true;
            }
        }
        public void Dispose()
        {
            m_connection?.Dispose();
            m_channel?.Dispose();
        }
    }
}
