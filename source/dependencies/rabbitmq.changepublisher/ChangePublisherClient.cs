using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using RabbitMQ.Client;
using System.Linq;
using Humanizer;

namespace Bespoke.Sph.RabbitMqPublisher
{
    [Export(typeof(IEntityChangePublisher))]
    public class ChangePublisherClient : IEntityChangePublisher, IDisposable
    {

        private IConnection m_connection;
        private IModel m_channel;
        public const int PERSISTENT_DELIVERY_MODE = 2;

        public string Exchange { get; set; }

        public ChangePublisherClient()
        {
            this.Exchange = "sph.topic";
        }


        public async Task PublishAdded(string operation, IEnumerable<Entity> attachedCollection, IDictionary<string, object> headers)
        {
            var items = attachedCollection.ToArray();
            await SendMessage("added", operation, items, headers);
        }

        public async Task PublishChanges(string operation, IEnumerable<Entity> attachedCollection, IEnumerable<AuditTrail> logs, IDictionary<string, object> headers)
        {
            var items = attachedCollection.ToArray();
            await SendMessage("changed", operation, items, headers, logs);
        }

        public async Task PublishDeleted(string operation, IEnumerable<Entity> deletedCollection, IDictionary<string, object> headers)
        {
            await SendMessage("deleted", operation, deletedCollection.ToArray(), headers);
        }

        public async Task SubmitChangesAsync(string operation, IEnumerable<Entity> attachedEntities, IEnumerable<Entity> deletedCollection, IDictionary<string, object> headers)
        {
            headers.AddOrReplace("operation", operation);
            if (!this.IsOpened)
                InitConnection();



            const string ROUTING_KEY = "persistence";
            var attachedJson = attachedEntities.Select(x => x.ToJsonString(true));
            var deletedJson = deletedCollection.Select(x => x.ToJsonString(true));
            var attached = string.Join(",\r\n", attachedJson);
            var deleted = string.Join(",\r\n", deletedJson);
            var json =
                $@"
{{
    ""attached"":[
                    {attached
                    }
                ],
    ""deleted"":[
                    {deleted}
                ]
}}";

            var body = await CompressAsync(json);

            var props = m_channel.CreateBasicProperties();
            props.DeliveryMode = PERSISTENT_DELIVERY_MODE;
            props.ContentType = "application/json";
            props.Headers = headers;

            if (headers.ContainsKey("sph.delay"))
            {
                PublishToDelayQueue(props, body, ROUTING_KEY);
                return;
            }

            m_channel.BasicPublish(this.Exchange, ROUTING_KEY, props, body);

        }

        private void PublishToDelayQueue(IBasicProperties props, byte[] body, string routingKey)
        {
            var count = 91;
            if (props.Headers.ContainsKey("sph.trycount"))
                count = (int)props.Headers["sph.trycount"];
            Console.WriteLine(@"Doing the delay for {0} ms for the {1} time", props.Headers["sph.delay"], count.Ordinalize());
            const string RETRY_EXCHANGE = "sph.retry.exchange";
            const string RETRY_QUEUE = "sph.retry.queue";
            var delay = (long)props.Headers["sph.delay"]; // in ms

            var queueArgs = new Dictionary<string, object> {
                    {"x-dead-letter-exchange", this.Exchange },
                    {"x-dead-letter-routing-key",routingKey}
                };
            props.Expiration = delay.ToString(CultureInfo.InvariantCulture);

            m_channel.ExchangeDeclare(RETRY_EXCHANGE, "direct");
            m_channel.QueueDeclare(RETRY_QUEUE, true, false, false, queueArgs);
            m_channel.QueueBind(RETRY_QUEUE, RETRY_EXCHANGE, string.Empty, null);

            m_channel.BasicPublish(RETRY_EXCHANGE, string.Empty, props, body);
        }

        private void InitConnection()
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


            m_channel.ExchangeDeclare(this.Exchange, ExchangeType.Topic, true);

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

        private async Task SendMessage(string action, string operation, IEnumerable<Entity> items, IDictionary<string, object> headers, IEnumerable<AuditTrail> logs = null)
        {
            if (!this.IsOpened)
                InitConnection();

            foreach (var item in items)
            {
                var entityType = this.GetEntityType(item);
                var log = string.Empty;
                var id = item.Id;
                if (null != logs && !string.IsNullOrWhiteSpace(id))
                {
                    var audit = logs.SingleOrDefault(l => l.Type == entityType.Name && l.EntityId == id);
                    if (null != audit)
                        log = audit.ToJsonString();
                }
                var routingKey = $"{entityType.Name}.{action}.{operation}";
                var item1 = item;
                var json = item1.ToJsonString();
                var body = await CompressAsync(json);

                var props = m_channel.CreateBasicProperties();
                props.DeliveryMode = PERSISTENT_DELIVERY_MODE;
                props.Persistent = true;
                props.ContentType = "application/json";
                props.Headers = new Dictionary<string, object> { { "operation", operation }, { "crud", action }, { "log", log } };
                if (null != headers)
                {
                    foreach (var k in headers.Keys)
                    {
                        if (!props.Headers.ContainsKey(k))
                            props.Headers.Add(k, headers[k]);

                    }
                }
                if (null != headers && headers.ContainsKey("sph.delay"))
                {
                    PublishToDelayQueue(props, body, routingKey);
                    return;
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
