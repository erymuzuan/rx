using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using RabbitMQ.Client;
using System.Linq;

namespace Bespoke.Sph.RabbitMqPublisher
{
    [Export(typeof(IEntityChangePublisher))]
    public class ChangePublisherClient : IEntityChangePublisher, IDisposable
    {
        private readonly IBrokerConnection m_connectionInfo;
        private IConnection m_connection;
        private IModel m_channel;
        public const int PERSISTENT_DELIVERY_MODE = 2;

        public string Exchange { get; set; }

        public ChangePublisherClient(IBrokerConnection connectionInfo)
        {
            m_connectionInfo = connectionInfo;
            this.Exchange = "sph.topic";
        }


        public async Task PublishAdded(string operation, IEnumerable<Entity> attachedCollection, Dictionary<string, object> headers)
        {
            var items = attachedCollection.ToArray();
            await SendMessage("added", operation, items, headers);
        }

        public async Task PublishChanges(string operation, IEnumerable<Entity> attachedCollection, IEnumerable<AuditTrail> logs, Dictionary<string, object> headers)
        {
            var items = attachedCollection.ToArray();
            await SendMessage("changed", operation, items, headers, logs);
        }

        public async Task PublishDeleted(string operation, IEnumerable<Entity> deletedCollection, Dictionary<string, object> headers)
        {
            await SendMessage("deleted", operation, deletedCollection.ToArray(), headers);
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

        }

        public void Close()
        {
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

        private async Task SendMessage(string action, string operation, IEnumerable<Entity> items, Dictionary<string, object> headers, IEnumerable<AuditTrail> logs = null)
        {
          if(!this.IsOpened)
              InitConnection();

            foreach (var item in items)
            {
                var entityType = this.GetEntityType(item);
                var log = string.Empty;
                var id = item.GetId();
                if (null != logs && id > 0)
                {
                    var audit = logs.SingleOrDefault(l => l.Type == entityType.Name && l.EntityId == id);
                    if (null != audit)
                        log = audit.ToJsonString();
                }
                var routingKey = string.Format("{0}.{1}.{2}", entityType.Name, action, operation);
                var item1 = item;
                var json = item1.ToJsonString();
                var body = await CompressAsync(json);

                var props = m_channel.CreateBasicProperties();
                props.DeliveryMode = PERSISTENT_DELIVERY_MODE;
                props.ContentType = "application/json";
                props.Headers = new Dictionary<string, object> { { "operation", operation }, { "crud", action }, { "log", log } };
                if (null != headers)
                {
                    foreach (var k in headers.Keys)
                    {
                        props.Headers.Add(k, headers[k]);

                    }
                }

                m_channel.BasicPublish(this.Exchange, routingKey, props, body);

            }


        }



        private async Task<byte[]> CompressAsync(string value)
        {
            var content = new byte[value.Length];
            int index = 0;
            foreach (char item in value)
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


        private Type GetEntityType(Entity item)
        {
            var type = item.GetType();
            var attr = type.GetCustomAttribute<EntityTypeAttribute>();
            if (null != attr) return attr.Type;
            return type;
        }


        public void Dispose()
        {
            this.Close();
        }
    }
}
