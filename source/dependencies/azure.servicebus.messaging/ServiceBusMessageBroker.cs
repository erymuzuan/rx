using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain.Messaging;
using Bespoke.Sph.Messaging.AzureMessaging.Extensions;
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
        TopicClient m_topicClient;
        private SubscriptionClient m_subClient;

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
            m_topicClient = m_factory.CreateTopicClient(AzureServiceBusConfigurationManager.DefaultTopicPath);
            return Task.FromResult(0);
        }

        public void OnMessageDelivered(Func<BrokeredMessage, Task<MessageReceiveStatus>> processItem, SubscriberOption subscription, double timeOut = Double.MaxValue)
        {
            m_subClient = SubscriptionClient.CreateFromConnectionString(AzureServiceBusConfigurationManager.PrimaryConnectionString, AzureServiceBusConfigurationManager.DefaultTopicPath, subscription.Name);

            // Set the options for using OnMessage
            var options = new OnMessageOptions()
            {
                AutoComplete = false,
                MaxConcurrentCalls = subscription.PrefetchCount,
                AutoRenewTimeout = TimeSpan.FromSeconds(30)
            };

            // Create a message pump using OnMessage
            m_subClient.OnMessage(async msg =>
           {
               var message = new BrokeredMessage
               {
                   Body = msg.GetBody<byte[]>(),
                   TryCount = msg.Properties.GetValue<string, int>("try-count"),
                   RetryDelay = msg.Properties.GetValue<string, TimeSpan>("retry-delay") ?? TimeSpan.Zero,
                   RoutingKey = msg.Properties.GetStringValue("routing-key"),
                   Id = msg.MessageId,
                   Crud = msg.Properties.GetValue<string, CrudOperation>("crud") ?? CrudOperation.None,
                   ReplyTo = msg.ReplyTo,
                   Operation = msg.Properties.GetStringValue("operation"),
                   Username = msg.Properties.GetStringValue("username")
               };
               var status = await processItem(message);
               switch (status)
               {
                   case MessageReceiveStatus.Accepted:
                       await msg.CompleteAsync();
                       break;
                   case MessageReceiveStatus.Rejected:
                       //TODO : for reject, we should get the error message
                       await msg.DeadLetterAsync("Rejected", "rejected description");
                       break;
                   case MessageReceiveStatus.Dropped:

                       await msg.CompleteAsync();
                       break;
                   case MessageReceiveStatus.Delayed:
                       //TODO : publish to delay queue
                       break;
                   case MessageReceiveStatus.Requeued:
                       await msg.DeferAsync();
                       break;
                   default:
                       throw new ArgumentOutOfRangeException(nameof(status), status, null);
               }

           }, options);



        }


        public Task<QueueStatistics> GetStatisticsAsync(string queue)
        {
            var subscription =
                m_namespaceMgr.GetSubscription(AzureServiceBusConfigurationManager.DefaultTopicPath, queue);
            return Task.FromResult(new QueueStatistics
            {
                Count = (int)subscription.MessageCount,
                PublishedRate = default,
                DeliveryRate = default

            });
        }

        public async Task CreateSubscriptionAsync(QueueSubscriptionOption option)
        {
            var topicPath = AzureServiceBusConfigurationManager.DefaultTopicPath;
            if (!m_namespaceMgr.TopicExists(topicPath))
            {
                await m_namespaceMgr.CreateTopicAsync(topicPath);
            }
            //TODO : all the routing keys into filter
            if (!m_namespaceMgr.SubscriptionExists(AzureServiceBusConfigurationManager.DefaultTopicPath, option.Name))
                await m_namespaceMgr.CreateSubscriptionAsync(topicPath, option.Name, new SqlFilter($"entity = 'Test'"));

        }


        public Task SendToDeadLetterQueue(BrokeredMessage message)
        {
            throw new NotImplementedException();
        }

        public async Task SendAsync(BrokeredMessage message)
        {
            var msg = new AzureBrokeredMessage(message.Body)
            {
                Label = message.RoutingKey,
                MessageId = message.Id
            };

            var topics = message.RoutingKey.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
            if (topics.Length == 3)
            {
                msg.Properties.Add("entity", topics[0]);
            }
            msg.Properties.Add("crud", (int)message.Crud);
            msg.Properties.Add("operation", message.Operation);

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
            await m_topicClient.SendAsync(msg);

        }

        public Task<BrokeredMessage> ReadFromDeadLetterAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<BrokeredMessage> GetMessageAsync(string queue)
        {
            var sub = m_factory.CreateSubscriptionClient(AzureServiceBusConfigurationManager.DefaultTopicPath, queue);
            var msg = await sub.ReceiveAsync(AzureServiceBusConfigurationManager.ReceiveMessageTimeOut);
            var bm = new BrokeredMessage(async (b, status) =>
           {
               switch (status)
               {
                   case MessageReceiveStatus.Accepted:
                       await msg.CompleteAsync();
                       break;
                   case MessageReceiveStatus.Rejected:
                       //TODO : for reject, we should get the error message
                       await msg.DeadLetterAsync("Rejected", "rejected description");
                       break;
                   case MessageReceiveStatus.Dropped:

                       await msg.CompleteAsync();
                       break;
                   case MessageReceiveStatus.Delayed:
                       //TODO : publish to delay queue
                       break;
                   case MessageReceiveStatus.Requeued:
                       await msg.DeferAsync();
                       break;
                   default:
                       throw new ArgumentOutOfRangeException(nameof(status), status, null);
               }

           })
            {
                Body = msg.GetBody<byte[]>(),
                TryCount = msg.Properties.GetValue<string, int>("try-count"),
                RetryDelay = msg.Properties.GetValue<string, TimeSpan>("retry-delay") ?? TimeSpan.Zero,
                RoutingKey = msg.Properties.GetStringValue("routing-key"),
                Id = msg.MessageId,
                Crud = msg.Properties.GetValue<string, CrudOperation>("crud") ?? CrudOperation.None,
                ReplyTo = msg.ReplyTo,
                Operation = msg.Properties.GetStringValue("operation"),
                Username = msg.Properties.GetStringValue("username")
            };

            return bm;

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
