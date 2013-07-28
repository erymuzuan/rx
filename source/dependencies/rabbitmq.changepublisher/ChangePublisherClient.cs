using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel.Composition;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.SphCommercialSpaces.Domain;
using RabbitMQ.Client;
using System.Linq;

namespace Bespoke.Sph.RabbitMqPublisher
{
    [Export(typeof(IEntityChangePublisher))]
    public class ChangePublisherClient : IEntityChangePublisher
    {
        private readonly IBrokerConnection m_connection;
        public const int PERSISTENT_DELIVERY_MODE = 2;

        public string Exchange { get; set; }

        public ChangePublisherClient(IBrokerConnection connection)
        {
            m_connection = connection;
            this.Exchange = "ruang.komersial.changes";
        }


        public async Task PublishAdded(string operation, IEnumerable<Entity> attachedCollection)
        {
            var items = attachedCollection.ToArray();
            await SendMessage("added", operation, items);
        }

        public async Task PublishChanges(string operation, IEnumerable<Entity> attachedCollection, IEnumerable<AuditTrail> logs)
        {
            var items = attachedCollection.ToArray();
            await SendMessage("changed", operation, items);
        }

        public async Task PublishDeleted(string operation, IEnumerable<Entity> deletedCollection)
        {
            await SendMessage("deleted", operation, deletedCollection.ToArray());
        }

        private int GetId(Entity item)
        {
            var type = this.GetEntityType(item);
            var id = type.GetProperties().AsQueryable().Single(p => p.PropertyType == typeof(int)
                                                                    && p.Name == type.Name + "Id");
            return (int)id.GetValue(item);
        }
        private async Task SendMessage(string action, string operation, IEnumerable<Entity> items, IEnumerable<AuditTrail> logs = null)
        {
            Console.WriteLine("sending....");
            var factory = new ConnectionFactory
            {
                UserName = m_connection.Username,
                Password = m_connection.Password,
                HostName = m_connection.Host,
                Port = m_connection.Port,
                VirtualHost = m_connection.VirtualHost
            };
            using (var conn = factory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                channel.ExchangeDeclare(this.Exchange, ExchangeType.Topic, true);
                foreach (var item in items)
                {
                    var entityType = this.GetEntityType(item);
                    var log = string.Empty;
                    var id = this.GetId(item);
                    if (null != logs && id > 0)
                    {
                        var audit = logs.SingleOrDefault(l => l.Type == entityType.Name && l.EntityId == id);
                        if (null != audit)
                            log = audit.ToXmlString();
                    }
                    var routingKey = entityType.Name + "." + action;
                    var item1 = item;
                    var xml = item1.ToXmlString();
                    var body = await CompressAsync(xml);

                    var props = channel.CreateBasicProperties();
                    props.DeliveryMode = PERSISTENT_DELIVERY_MODE;
                    props.ContentType = "application/xml";
                    props.Headers = new Dictionary<string, string> { { "operation", operation }, { "crud", action }, { "log", log } };

                    channel.BasicPublish(this.Exchange, routingKey, props, body);

                    Console.WriteLine("Published to {0}, exc : {1}, keys : {2}", factory.VirtualHost, this.Exchange, routingKey);
                }

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


            var ms = new MemoryStream();
            var sw = new GZipStream(ms, CompressionMode.Compress);

            await sw.WriteAsync(content, 0, content.Length);
            //NOTE : DO NOT FLUSH cause bytes will go missing...
            sw.Close();

            content = ms.ToArray();

            ms.Close();
            sw.Dispose();
            ms.Dispose();
            return content;
        }


        private Type GetEntityType(Entity item)
        {
            var type = item.GetType();
            var attr = type.GetCustomAttribute<EntityTypeAttribute>();
            if (null != attr) return attr.Type;
            return type;
        }



    }
}
