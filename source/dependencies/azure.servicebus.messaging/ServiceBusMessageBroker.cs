using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Messaging;
using Bespoke.Sph.Extensions;
using Bespoke.Sph.Messaging.AzureMessaging.Extensions;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;
using BrokeredMessage = Bespoke.Sph.Domain.Messaging.BrokeredMessage;
using Filter = Microsoft.ServiceBus.Messaging.Filter;

namespace Bespoke.Sph.Messaging.AzureMessaging
{
    public class ServiceBusMessageBroker : IMessageBroker
    {
        private NamespaceManager m_namespaceMgr;
        private MessagingFactory m_factory;
        private TopicClient m_topicClient;
        private readonly ConcurrentDictionary<string, SubscriptionClient> m_subscribers = new ConcurrentDictionary<string, SubscriptionClient>();
        private readonly string m_connectionString = AzureServiceBusConfigurationManager.PrimaryConnectionString;
        private readonly string m_topicPath = AzureServiceBusConfigurationManager.DefaultTopicPath;

        public void Dispose()
        {
            foreach (var sub in m_subscribers.Keys)
            {
                if (m_subscribers.TryGetValue(sub, out var client))
                    client.Close();
            }

            m_topicClient?.Close();
            m_factory?.Close();
        }


        public Task ConnectAsync(Action<string, object> disconnected)
        {
            var connectionString = m_connectionString;
            m_namespaceMgr = NamespaceManager.CreateFromConnectionString(connectionString);
            m_factory = MessagingFactory.CreateFromConnectionString(connectionString);
            m_topicClient = m_factory.CreateTopicClient(m_topicPath);
            return Task.FromResult(0);
        }

        public void OnMessageDelivered(Func<BrokeredMessage, Task<MessageReceiveStatus>> processItem, SubscriberOption subscription, double timeOut = Double.MaxValue)
        {
            var subClient = SubscriptionClient.CreateFromConnectionString(
                m_connectionString,
                m_topicPath,
                subscription.QueueName);

            // Set the options for using OnMessage
            var options = new OnMessageOptions
            {
                AutoComplete = false,
                MaxConcurrentCalls = subscription.PrefetchCount,
                AutoRenewTimeout = TimeSpan.FromMinutes(5)
            };

            // Create a message pump using OnMessage
            subClient.OnMessageAsync(async msg =>
           {
               var message = new BrokeredMessage
               {
                   Body = msg.GetBody<byte[]>(),
                   TryCount = msg.Properties.GetValue<string, int>("try-count"),
                   RetryDelay = msg.Properties.GetValue<string, TimeSpan>("retry-delay") ?? TimeSpan.Zero,
                   RoutingKey = msg.Properties.GetStringValue("routing-key"),
                   Id = msg.MessageId,
                   ReplyTo = msg.ReplyTo,
                   Entity = msg.Properties.GetStringValue("entity"),
                   Operation = msg.Properties.GetStringValue("operation"),
                   Username = msg.Properties.GetStringValue("username")
               };
               if (Enum.TryParse(msg.Properties.GetStringValue("crud"), true, out CrudOperation crud))
                   message.Crud = crud;

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
                       await PublishToDelayQueueAsync(message, subscription.QueueName);
                       try
                       {
                           await msg.CompleteAsync();
                       }
                       catch (Exception e)
                       {
                           ObjectBuilder.GetObject<ILogger>().WriteInfo(e.Message);
                           throw;
                       }
                       break;
                   case MessageReceiveStatus.Requeued:
                       await msg.DeferAsync();
                       break;
                   default:
                       throw new ArgumentOutOfRangeException(nameof(status), status, null);
               }

           }, options);

            m_subscribers.AddOrUpdate(subscription.QueueName + "::" + subscription.Name, subClient, (q, c1) => subClient);

        }
        private async Task PublishToDelayQueueAsync(BrokeredMessage message, string queue)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            var count = (message.TryCount ?? 0) + 1;

            logger.WriteInfo($@"Doing the delay for {message.RetryDelay} for the {count} time");

            var msg = message.ToAzureMessage();
            // remove the filter
            msg.Properties.AddOrReplace("entity", "-");
            msg.Properties.AddOrReplace("crud", "-");
            msg.Properties.AddOrReplace("operation", "-");

            msg.Properties.AddOrReplace("queue", queue);
            msg.Properties.AddOrReplace("delayed", 1);
            msg.Properties.AddOrReplace("try-count", count);
            msg.TimeToLive = message.RetryDelay;

            logger.WriteVerbose($"Sending message to  with TTL : {msg.TimeToLive}({msg.MessageId})");

            await m_topicClient.SendAsync(msg);
        }

        public Task<QueueStatistics> GetStatisticsAsync(string queue)
        {
            var subscription =
                m_namespaceMgr.GetSubscription(m_topicPath, queue);
            return Task.FromResult(new QueueStatistics
            {
                Count = (int)subscription.MessageCount,
                PublishedRate = default,
                DeliveryRate = default

            });
        }


        public async Task CreateSubscriptionAsync(QueueDeclareOption option)
        {
            var topicPath = m_topicPath;
            if (!m_namespaceMgr.TopicExists(topicPath))
            {
                await m_namespaceMgr.CreateTopicAsync(topicPath);
            }


            async Task CreateSubscriptionAsync(SubscriptionDescription sd, Filter filter = null)
            {
                if (!m_namespaceMgr.SubscriptionExists(m_topicPath, sd.Name))
                {
                    if (null != filter)
                        await m_namespaceMgr.CreateSubscriptionAsync(sd, filter);
                    else
                        await m_namespaceMgr.CreateSubscriptionAsync(sd);

                    var exist = m_namespaceMgr.SubscriptionExists(topicPath, option.QueueName);
                    while (!exist)
                    {
                        await Task.Delay(200);
                        exist = m_namespaceMgr.SubscriptionExists(topicPath, option.QueueName);
                    }
                }

            }

            var sql = new SqlFilter(GetSqlFilterExpressions(option));
            await CreateSubscriptionAsync(new SubscriptionDescription(topicPath, option.QueueName), sql);
            await CreateSubscriptionAsync(new SubscriptionDescription(topicPath, option.DelayedQueue ?? "rx-delayed-" + option.QueueName)
            {
                EnableDeadLetteringOnMessageExpiration = true,
                //ForwardDeadLetteredMessagesTo = option.QueueName
            }, new SqlFilter($"queue = '{option.QueueName}' AND delayed = 1"));
            // TODO : creates a new subscriber that subscribe to delayed queue

        }

        public string GetSqlFilterExpressions(QueueDeclareOption option)
        {
            var routes = new List<string>();
            var wildcards = new[] { "*", "#" };
            foreach (var route in option.RoutingKeys)
            {
                var predicates = new List<string>();
                var keys = route.Split(new[] { "." }, StringSplitOptions.RemoveEmptyEntries);
                if (keys.Length == 3)
                {
                    var entity = keys.First();
                    if (!wildcards.Contains(entity))
                        predicates.Add($"entity = '{entity}'");
                    var crud = keys[1];
                    if (!wildcards.Contains(crud))
                    {
                        if (Enum.TryParse(crud, true, out CrudOperation _))
                            predicates.Add($"crud = '{crud}'");
                    }
                    var operation = keys[2];
                    if (!wildcards.Contains(operation))
                        predicates.Add($"operation = '{operation}'");
                    routes.Add(predicates.ToString(" AND "));
                }
            }
            var sql = routes.ToString(" OR ");
            if (routes.Count > 1)
                sql = routes.Select(x => $"({x})").ToString(" OR ");
            ObjectBuilder.GetObject<ILogger>().WriteInfo($"Creating subscription with SqlFilter :{sql}");
            return sql;
        }


        public Task SendToDeadLetterQueue(BrokeredMessage message)
        {
            throw new NotImplementedException();
        }

        public async Task SendAsync(BrokeredMessage message)
        {
            var msg = message.ToAzureMessage();
            ObjectBuilder.GetObject<ILogger>().WriteVerbose($"Sending message to Azure Service Bus : {msg.Properties["entity"]}.{msg.Properties["crud"]}.{msg.Properties["operation"]}({msg.MessageId})");

            // Send the message.
            await m_topicClient.SendAsync(msg);

        }

        public Task<BrokeredMessage> ReadFromDeadLetterAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<BrokeredMessage> GetMessageAsync(string queue)
        {
            var sub = m_factory.CreateSubscriptionClient(m_topicPath, queue);
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
                       await PublishToDelayQueueAsync(b, queue);
                       await msg.CompleteAsync();
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
                ReplyTo = msg.ReplyTo,
                Operation = msg.Properties.GetStringValue("operation"),
                Entity = msg.Properties.GetStringValue("entity"),
                Username = msg.Properties.GetStringValue("username")
            };

            if (Enum.TryParse(msg.Properties.GetStringValue("crud"), true, out CrudOperation crud))
                bm.Crud = crud;

            return bm;

        }

        public async Task RemoveSubscriptionAsync(string queue)
        {
            var topicPath = m_topicPath;
            if (!m_namespaceMgr.TopicExists(topicPath))
            {
                m_namespaceMgr.CreateTopic(topicPath);
            }
            if (m_namespaceMgr.SubscriptionExists(topicPath, queue))
                await m_namespaceMgr.DeleteSubscriptionAsync(topicPath, queue);
        }
    }
}
