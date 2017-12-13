using System;
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
            throw new NotImplementedException();
        }

        public void StartsConsume()
        {
            throw new NotImplementedException();
        }

        public event EventHandler<EventArgs> ConnectionShutdown;
        private IConnection m_connection;
        public Task ConnectAsync()
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
                ConnectionShutdown?.Invoke(o, e);
            };
            return Task.FromResult(0);
        }

        public void OnMessageDelivered(Func<BrokeredMessage, Task<MessageReceiveStatus>> processItem, double timeOut = double.MaxValue)
        {
            throw new NotImplementedException();
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

        public Task CreateSubscriptionAsync(QueueSubscriptionOption option)
        {
            throw new NotImplementedException();
        }

        public Task SendToDeathLetter(BrokeredMessage message)
        {
            throw new NotImplementedException();
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
            logger.WriteInfo($"Deleting trigger_subs_{queue} queue");
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
            }
        }
    }
}
