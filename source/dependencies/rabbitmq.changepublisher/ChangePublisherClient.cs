using System;
using System.Collections.Generic;
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

        public ChangePublisherClient(IBrokerConnection connection )
        {
            m_connection = connection;
            this.Exchange = "ruang.komersial.changes";
        }


        public async Task PublishAdded(IEnumerable<Entity> attachedCollection)
        {
            var items = attachedCollection.ToArray();
            await SendMessage("added", items);
        }

        public async Task PublishChanges(IEnumerable<Entity> attachedCollection)
        {
            var items = attachedCollection.ToArray();
            await SendMessage("changed", items);
        }

        public async Task PublishDeleted(IEnumerable<Entity> deletedCollection)
        {
            await SendMessage("deleted", deletedCollection.ToArray());
        }

        private async Task SendMessage(string action, params Entity[] items)
        {

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
                    var routingKey = entityType.Name + "." + action;
                    var item1 = item;
                    var xml = item1.ToXmlString();
                    var body = await CompressAsync(xml);

                    var props = channel.CreateBasicProperties();
                    props.DeliveryMode = PERSISTENT_DELIVERY_MODE;
                    props.ContentType = "application/xml";


                    channel.BasicPublish(this.Exchange, routingKey, props, body);
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
