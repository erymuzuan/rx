using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Messaging;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using BrokeredMessage = Bespoke.Sph.Domain.Messaging.BrokeredMessage;
using AzureBrokeredMessage = Microsoft.ServiceBus.Messaging.BrokeredMessage;

namespace Bespoke.Sph.Messaging.AzureMessaging
{
    public class ServiceBusMessageBroker : IMessageBroker
    {
        NamespaceManager m_namespaceMgr;
        MessagingFactory m_factory;
        TopicClient m_topicclient;

        public void Dispose()
        {
        }



        public void StartsConsume()
        {
            throw new NotImplementedException();
        }

        public Task ConnectAsync(Action<string, object> disconnected)
        {
            var connectionString = AzureServiceBusConfigurationManager.PrimaryConnectionString;
            m_namespaceMgr = NamespaceManager.CreateFromConnectionString(connectionString);
            m_factory = MessagingFactory.CreateFromConnectionString(connectionString);
            m_topicclient = m_factory.CreateTopicClient(AzureServiceBusConfigurationManager.DefaultTopicPath);
            return Task.FromResult(0);
        }

        public void OnMessageDelivered(Func<BrokeredMessage, Task<MessageReceiveStatus>> processItem, SubscriberOption subscription, double timeOut = Double.MaxValue)
        {
            throw new NotImplementedException();
        }

        public async void OnMessageDelivered(Func<BrokeredMessage, Task<MessageReceiveStatus>> processItem, double timeOut = double.MaxValue)
        {
            var message = new BrokeredMessage();// TODO : read from queue
            var status = await processItem(message);
            switch (status)
            {
                case MessageReceiveStatus.Accepted:
                    break;
                case MessageReceiveStatus.Rejected:
                    break;
                case MessageReceiveStatus.Requeued:
                    break;
                case MessageReceiveStatus.Dropped:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        public Task<QueueStatistics> GetStatisticsAsync(string queue)
        {
            throw new NotImplementedException();
        }

        public async Task CreateSubscriptionAsync(QueueSubscriptionOption option)
        {
            var topicPath = AzureServiceBusConfigurationManager.DefaultTopicPath;
            if (!m_namespaceMgr.TopicExists(topicPath))
            {
                await m_namespaceMgr.CreateTopicAsync(topicPath);
            }
            //TODO : all the routing keys into filter
            await m_namespaceMgr.CreateSubscriptionAsync(topicPath, option.Name, new SqlFilter($"entity = 'Test'"));

        }


        public Task SendToDeadLetterQueue(BrokeredMessage message)
        {
            throw new NotImplementedException();
        }

        public async Task SendAsync(BrokeredMessage message)
        {
            var msg = new AzureBrokeredMessage(message.Body) { Label = message.RoutingKey };

            var topics = message.RoutingKey.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if (topics.Length == 3)
            {
                msg.Properties.Add("entity", topics[0]);
                msg.Properties.Add("crud", topics[1]);
                msg.Properties.Add("operation", topics[1]);
            }
            else
            {
                msg.Properties.Add("operation", message.Operation);
            }

            msg.Properties.Add("try-count", message.TryCount);
            msg.Properties.Add("message-id", message.Id);
            msg.Properties.Add("data-import", message.IsDataImport);
            msg.Properties.Add("reply-to", message.ReplyTo);
            msg.Properties.Add("routing-key", message.RoutingKey);
            msg.Properties.Add("retry-delay", message.RetryDelay);
            msg.Properties.Add("username", message.Username);

            // Set the CorrelationId to the region.
            //  orderMsg.CorrelationId = order.Region;

            // Send the message.
            await m_topicclient.SendAsync(msg);

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
            var topicPath = AzureServiceBusConfigurationManager.DefaultTopicPath;
            if (!m_namespaceMgr.TopicExists(topicPath))
            {
                m_namespaceMgr.CreateTopic(topicPath);
            }
            if (m_namespaceMgr.SubscriptionExists(topicPath, queue))
                await m_namespaceMgr.DeleteSubscriptionAsync(topicPath, queue);
        }
    }
}
