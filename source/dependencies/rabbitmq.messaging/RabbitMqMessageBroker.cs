using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Messaging;
using Bespoke.Sph.Extensions;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;

namespace Bespoke.Sph.Messaging.RabbitMqMessagings
{
    public class RabbitMqMessageBroker : IMessageBroker
    {
        public string Password { get; }
        public string UserName { get; }
        public string HostName { get; }
        public int Port { get; }
        public string VirtualHost { get; }

        public RabbitMqMessageBroker() : this(RabbitMqConfigurationManager.UserName, RabbitMqConfigurationManager.Password,
            RabbitMqConfigurationManager.Host, RabbitMqConfigurationManager.Port,
            RabbitMqConfigurationManager.VirtualHost)
        {
        }
        public RabbitMqMessageBroker(string userName, string password, string host, int port, string vhost)
        {
            Password = password;
            UserName = userName;
            HostName = host;
            Port = port;
            VirtualHost = vhost;
        }

        public void Dispose()
        {
            m_channel?.Close();
            m_channel?.Dispose();
            m_connection?.Close();
            m_connection?.Dispose();

        }

        public void StartsConsume()
        {
            throw new NotImplementedException();
        }

        private IConnection m_connection;
        public Task ConnectAsync(Action<string, object> disconnected)
        {
            var factory = new ConnectionFactory
            {
                UserName = this.UserName,
                VirtualHost = this.VirtualHost,
                Password = this.Password,
                HostName = this.HostName,
                Port = this.Port
            };
            m_connection = factory.CreateConnection();
            m_connection.ConnectionShutdown += (o, e) =>
            {
                disconnected(e.ReplyText, e);
            };
            return Task.FromResult(0);
        }

        public void OnMessageDelivered(Func<BrokeredMessage, Task<MessageReceiveStatus>> processItem, SubscriberOption subscription, double timeOut = double.MaxValue)
        {
            const bool NO_ACK = false;
            m_consumer = new TaskBasicConsumer(m_channel);
            m_consumer.Received += async (o, e) =>
             {
                 var header = new MessageHeaders(e);
                 var message = new BrokeredMessage
                 {
                     Body = e.Body,
                     Crud = header.Crud,
                     Id = header.MessageId,
                     RoutingKey = e.RoutingKey,
                     Username = header.Username,
                     Operation = header.Operation,
                     TryCount = header.TryCount,
                     ReplyTo = header.ReplyTo,
                     RetryDelay = TimeSpan.FromMilliseconds(5000)//TODO : get the delay from ...???,

                 };
                 var rawHeaders = header.GetRawHeaders();
                 foreach (var key in rawHeaders.Keys)
                 {
                     message.Headers.AddOrReplace(key, rawHeaders[key]);
                 }

                 var status = await processItem(message);
                 switch (status)
                 {
                     case MessageReceiveStatus.Accepted:
                         m_channel.BasicAck(e.DeliveryTag, false);
                         break;
                     case MessageReceiveStatus.Rejected:
                         m_channel.BasicReject(e.DeliveryTag, false);
                         break;
                     case MessageReceiveStatus.Dropped:
                         m_channel.BasicAck(e.DeliveryTag, false);
                         break;
                     case MessageReceiveStatus.Delayed:
                         // TODO : send to delay queue with TTL and DLQ
                         break;
                     case MessageReceiveStatus.Requeued:
                         //TODO : silently requeued or call send
                         break;
                     default:
                         throw new ArgumentOutOfRangeException();
                 }

             };
            var prefetchCount = subscription.PrefetchCount <= 1 ? 1 : subscription.PrefetchCount;
            m_channel.BasicQos(0, (ushort)prefetchCount, false);
            m_channel.BasicConsume(subscription.Name, NO_ACK, m_consumer);
        }

        public async Task<QueueStatistics> GetStatisticsAsync(string queue)
        {
            var handler = new HttpClientHandler { Credentials = new NetworkCredential(RabbitMqConfigurationManager.UserName, RabbitMqConfigurationManager.Password) };
            var client = new HttpClient(handler) { BaseAddress = new Uri($"{RabbitMqConfigurationManager.ManagementScheme}://{RabbitMqConfigurationManager.Host}:{RabbitMqConfigurationManager.ManagementPort}") };

            var response = await client.GetStringAsync($"api/queues/{ConfigurationManager.ApplicationName}/{queue}");

            var json = JObject.Parse(response);
            var published = json.SelectToken("$.message_stats.publish_details.rate").Value<double>();
            var delivered = json.SelectToken("$.message_stats.deliver_details.rate").Value<double>();
            var length = json.SelectToken("$.messages").Value<int>();

            return new QueueStatistics
            {
                PublishedRate = published,
                DeliveryRate = delivered,
                Count = length
            };
        }
        private IModel m_channel;
        private TaskBasicConsumer m_consumer;

        public Task CreateSubscriptionAsync(QueueSubscriptionOption option)
        {
            const string EXCHANGE_NAME = "sph.topic";
            const string DEAD_LETTER_EXCHANGE = "sph.ms-dead-letter";
            const string DEAD_LETTER_QUEUE = "ms_dead_letter_queue";


            m_channel = m_connection.CreateModel();

            m_channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic, true);
            m_channel.ExchangeDeclare(DEAD_LETTER_EXCHANGE, ExchangeType.Topic, true);
            var args = new Dictionary<string, object> { { "x-dead-letter-exchange", DEAD_LETTER_EXCHANGE } };
            m_channel.QueueDeclare(option.Name, true, false, false, args);

            m_channel.QueueDeclare(DEAD_LETTER_QUEUE, true, false, false, args);
            m_channel.QueueBind(DEAD_LETTER_QUEUE, DEAD_LETTER_EXCHANGE, "#", null);
            m_channel.QueueBind(DEAD_LETTER_QUEUE, DEAD_LETTER_EXCHANGE, "*.added", null);
            m_channel.QueueBind(DEAD_LETTER_QUEUE, DEAD_LETTER_EXCHANGE, "*.changed", null);

            foreach (var s in option.RoutingKeys)
            {
                m_channel.QueueBind(option.Name, EXCHANGE_NAME, s, null);
            }
            // delay exchange and queue
            var delayExchange = "sph.delay.exchange." + option.Name;
            var delayQueue = "sph.delay.queue." + option.Name;
            var delayQueueArgs = new Dictionary<string, object>
            {
                {"x-dead-letter-exchange", delayExchange},
                {"x-dead-letter-routing-key", option.Name}
            };

            m_channel.ExchangeDeclare(delayExchange, "direct");
            m_channel.QueueDeclare(delayQueue, true, false, false, delayQueueArgs);
            m_channel.QueueBind(delayQueue, delayExchange, string.Empty, null);

            m_channel.BasicQos(0, (ushort)option.PrefetchCount, false);

            return Task.FromResult(0);
        }

        public Task SendToDeadLetterQueue(BrokeredMessage message)
        {
            throw new NotImplementedException();
        }

        public Task SendAsync(BrokeredMessage message)
        {
            var props = m_channel.CreateBasicProperties();
            props.DeliveryMode = 2;
            props.Persistent = true;
            props.ContentType = "application/json";
            props.Headers = new MessageHeaders(message).ToDictionary();
            m_channel.BasicPublish("sph.topic", message.RoutingKey, props, message.Body);
            return Task.FromResult(0);
        }


        public Task<BrokeredMessage> ReadFromDeadLetterAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BrokeredMessage> GetMessageAsync(string queue)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveSubscriptionAsync(string queue)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            logger.WriteInfo($"Deleting {queue} queue");
            var url = $"http://{RabbitMqConfigurationManager.Host}:{RabbitMqConfigurationManager.ManagementPort}";
            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(RabbitMqConfigurationManager.UserName, RabbitMqConfigurationManager.Password)
            };
            using (var client = new HttpClient(handler) { BaseAddress = new Uri(url) })
            {
                logger.WriteInfo($"Deleting the queue for trigger : {queue}");
                var response = await client.DeleteAsync($"/api/queues/{ConfigurationManager.ApplicationName}/{queue}");
                if (response.StatusCode != HttpStatusCode.NoContent)
                {
                    logger.WriteError(($"Cannot delete queue {queue}  return code is {response.StatusCode}"));
                }
                var ttlResponse = await client.DeleteAsync($"/api/queues/{ConfigurationManager.ApplicationName}/sph.delay.queue.{ queue}");
                if (ttlResponse.StatusCode != HttpStatusCode.NoContent)
                {
                    logger.WriteError(($"Cannot delete queue {queue}  return code is {response.StatusCode}"));
                }
            }
        }
    }
}
