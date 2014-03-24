using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Bespoke.Station.MessagingPersistences
{
    public class MessagingBroker : IDisposable
    {
        public const int PERSISTENT_DELIVERY_MODE = 2;
        const string RequestRoutingKey = "persistence_request";

        private readonly IConnection m_conn;
        private readonly IModel m_channel;
        public string Exchange { get; set; }

        public MessagingBroker(BrokerConnection connection, string exchange)
        {
            this.Exchange = exchange;
            var factory = new ConnectionFactory
            {
                UserName = connection.UserName,
                Password = connection.Password,
                HostName = connection.Host,
                Port = connection.Port,
                VirtualHost = connection.VirtualHost
            };

            m_conn = factory.CreateConnection();
            m_channel = m_conn.CreateModel();


        }
        public MessagingBroker(string connection, string exchange)
        {
            this.Exchange = exchange;
            var factory = new ConnectionFactory
                {
                    Uri = connection
                };

            m_conn = factory.CreateConnection();
            m_channel = m_conn.CreateModel();


        }


        public Task<SubmitOperation> SubmitChanges(IEnumerable<Entity> addedOrUpdatedItems, IEnumerable<Entity> deletedItems)
        {
            var correlationId = Guid.NewGuid().ToString();
            var tcs = new TaskCompletionSource<SubmitOperation>();
            

            var queue = m_channel.QueueDeclare("", false, true, true, null);
            var changes = new ChangeSubmission
            {
                ChangedCollection = new ObjectCollection<Entity>(addedOrUpdatedItems),
                DeletedCollection = new ObjectCollection<Entity>(deletedItems)
            };
            var request = XmlSerializerService.ToUTF8EncodedXmlString(changes);
            var requestBody = System.Text.Encoding.UTF8.GetBytes(request);


            var props = m_channel.CreateBasicProperties();
            props.CorrelationId = correlationId;
            props.ReplyTo = queue.QueueName;
            props.DeliveryMode = PERSISTENT_DELIVERY_MODE;
            props.ContentType = "application/xml";
            m_channel.BasicPublish(this.Exchange, RequestRoutingKey, props, requestBody);


            
            // response
            var consumer = new TaskBasicConsumer(m_channel);
            consumer.Received += (s, e) =>
            {
                var response = System.Text.Encoding.UTF8.GetString(e.Body);
                if (e.Properties.CorrelationId == correlationId)
                {
                    tcs.SetResult(JsonConvert.DeserializeObject<SubmitOperation>(response));
                    m_channel.BasicAck(e.DeliveryTag, false);
                }
                else
                {
                    m_channel.BasicReject(e.DeliveryTag, true);
                }

            };
            m_channel.BasicConsume(queue.QueueName, true, consumer);


            return tcs.Task;

        }


        public void Dispose()
        {
            Console.WriteLine("disposed ....");
            m_channel.Dispose();
            m_conn.Dispose();
        }
    }
}
