﻿using System;
using System.Collections.Generic;
using System.Globalization;
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
    // ReSharper disable once UnusedMember.Global
    public class RabbitMqMessageBroker : IMessageBroker
    {
        private string Password { get; }
        private string UserName { get; }
        private string HostName { get; }
        private int Port { get; }
        private string VirtualHost { get; }
        private int ProcessId { get; }

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
            this.ProcessId = System.Diagnostics.Process.GetCurrentProcess().Id;
        }


        public void Dispose()
        {
            m_channel?.Close();
            m_channel?.Dispose();
            m_connection?.Close();
            m_connection?.Dispose();

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
            m_channel = m_connection.CreateModel();
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
                         if (message.RetryDelay == default)
                             throw new Exception("You have to set the RetryDelay for the message TTL");

                         m_channel.BasicAck(e.DeliveryTag, false);
                         PublishToDelayQueue(message, subscription.QueueName);
                         break;
                     case MessageReceiveStatus.Requeued:
                         //TODO : silently requeued or call send
                         break;
                     default:
                         throw new ArgumentOutOfRangeException();
                 }

             };
            var prefetchCount = subscription.PrefetchCount <= 1 ? 1 : subscription.PrefetchCount;
            lock (m_lock)
            {
                m_channel.BasicQos(0, (ushort)prefetchCount, false);
                var tag = m_channel.BasicConsume(subscription.QueueName, NO_ACK, $"{ProcessId}_{subscription.Name}", m_consumer);
                ObjectBuilder.GetObject<ILogger>().WriteVerbose($"Subscribing to {subscription.QueueName}({tag}) on thread {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            }
        }

        public async Task<QueueStatistics> GetStatisticsAsync(string queue)
        {
            var handler = new HttpClientHandler { Credentials = new NetworkCredential(RabbitMqConfigurationManager.UserName, RabbitMqConfigurationManager.Password) };
            var client = new HttpClient(handler) { BaseAddress = new Uri($"{RabbitMqConfigurationManager.ManagementScheme}://{RabbitMqConfigurationManager.Host}:{RabbitMqConfigurationManager.ManagementPort}") };

            var response = await client.GetAsync($"api/queues/{ConfigurationManager.ApplicationName}/{queue}");
            if (!response.IsSuccessStatusCode)
                return new QueueStatistics();

            var json = await response.ReadContentAsJsonAsync();
            var messagesToken = json.SelectToken("$.messages");
            //NOTE : some newer RabbitMq management plugin didn't return the correct messages
            while (null == messagesToken)
            {
                await Task.Delay(100);
                response = await client.GetAsync($"api/queues/{ConfigurationManager.ApplicationName}/{queue}");
                if (!response.IsSuccessStatusCode)
                    return new QueueStatistics();

                json = await response.ReadContentAsJsonAsync();
                messagesToken = json.SelectToken("$.messages");
            }


            var length = messagesToken.Value<int>();
            var statsToken = json.SelectToken("$.message_stats");
            double published = default;
            double delivered = default;
            var unackaed = json.SelectToken("$.messages_unacknowledged")?.Value<int>() ?? 0;

            if (null != statsToken)
            {
                var publishToken = statsToken.SelectToken("$.publish_details");
                if (null != publishToken)
                    published = publishToken.SelectToken("$.rate").Value<double>();
                var deliverToken = statsToken.SelectToken("$.deliver_details");
                if (null != deliverToken)
                    delivered = deliverToken.SelectToken("$.rate").Value<double>();
            }

            return new QueueStatistics
            {
                PublishedRate = published,
                DeliveryRate = delivered,
                Count = length,
                Processing = unackaed
            };
        }
        private IModel m_channel;
        private TaskBasicConsumer m_consumer;

        private readonly object m_lock = new object();

        public Task CreateSubscriptionAsync(QueueDeclareOption option)
        {

            ObjectBuilder.GetObject<ILogger>().WriteVerbose($"Creating subscription on thread {System.Threading.Thread.CurrentThread.ManagedThreadId}");
            lock (m_lock)
            {
                var exchangeName = RabbitMqConfigurationManager.DefaultExchange;
                var deadLetterExchange = option.DeadLetterTopic ?? RabbitMqConfigurationManager.DefaultDeadLetterExchange;
                var deadLetterQueue = option.DeadLetterQueue ?? RabbitMqConfigurationManager.DefaultDeadLetterQueue;


                m_channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true);
                m_channel.ExchangeDeclare(deadLetterExchange, ExchangeType.Topic, true);
                var args = new Dictionary<string, object> { { "x-dead-letter-exchange", deadLetterExchange } };
                m_channel.QueueDeclare(option.QueueName, true, false, false, args);

                m_channel.QueueDeclare(deadLetterQueue, true, false, false, args);
                m_channel.QueueBind(deadLetterQueue, deadLetterExchange, "#", null);
                m_channel.QueueBind(deadLetterQueue, deadLetterExchange, "*.added", null);
                m_channel.QueueBind(deadLetterQueue, deadLetterExchange, "*.changed", null);

                m_channel.QueueBind(option.QueueName, exchangeName, option.QueueName, null);
                foreach (var s in option.RoutingKeys)
                {
                    m_channel.QueueBind(option.QueueName, exchangeName, s, null);
                }
                // delay exchange and queue
                var delayExchange = option.DelayedExchange ?? ("rx.delay.exchange." + option.QueueName);
                var delayQueue = "rx.delay.queue." + option.QueueName;
                var delayQueueArgs = new Dictionary<string, object>
                {
                    {"x-dead-letter-exchange", exchangeName},
                    {"x-dead-letter-routing-key", option.QueueName}
                };

                m_channel.ExchangeDeclare(delayExchange, "direct");
                m_channel.QueueDeclare(delayQueue, true, false, false, delayQueueArgs);
                m_channel.QueueBind(delayQueue, delayExchange, string.Empty, null);

                m_channel.BasicQos(0, (ushort)option.PrefetchCount, false);

                return Task.FromResult(0);

            }
        }


        private void PublishToDelayQueue(BrokeredMessage message, string queue)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            var delayExchange = "rx.delay.exchange." + queue;

            message.TryCount = (message.TryCount ?? 0) + 1;

            var props = m_channel.CreateBasicProperties();
            props.DeliveryMode = 2;
            props.Persistent = true;
            props.ContentType = "application/json";
            var headers = new MessageHeaders(message);
            props.Headers = headers.ToDictionary();
            props.Headers.AddOrReplace(MessageHeaders.SPH_TRYCOUNT, message.TryCount);

            var delay = message.RetryDelay.TotalMilliseconds;
            props.Expiration = delay.ToString(CultureInfo.InvariantCulture);

            logger.WriteInfo($@"Doing the delay for {message.RetryDelay} for the {message.TryCount} time");
            m_channel.BasicPublish(delayExchange, string.Empty, props, message.Body);
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
            m_channel.BasicPublish(RabbitMqConfigurationManager.DefaultExchange, message.RoutingKey, props, message.Body);
            return Task.FromResult(0);
        }


        public Task<BrokeredMessage> ReadFromDeadLetterAsync()
        {
            throw new NotImplementedException();
        }

        public Task<BrokeredMessage> GetMessageAsync(string queue)
        {
            var result = this.m_channel.BasicGet(queue, false);
            var header = new MessageHeaders(new ReceivedMessageArgs
            {
                Body = result.Body,
                Properties = result.BasicProperties,
                DeliveryTag = result.DeliveryTag,
                Exchange = result.Exchange,
                RoutingKey = result.RoutingKey,
                Redelivered = result.Redelivered
            });
            void MessageAcknowledged(BrokeredMessage msg, MessageReceiveStatus status)
            {
                switch (status)
                {
                    case MessageReceiveStatus.Accepted:
                        m_channel.BasicAck(result.DeliveryTag, false);
                        break;
                    case MessageReceiveStatus.Rejected:
                        m_channel.BasicReject(result.DeliveryTag, false);
                        break;
                    case MessageReceiveStatus.Dropped:
                        m_channel.BasicAck(result.DeliveryTag, false);
                        break;
                    case MessageReceiveStatus.Delayed:
                        PublishToDelayQueue(msg, queue);
                        break;
                    case MessageReceiveStatus.Requeued:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(status), status, null);
                }
            }
            var message = new BrokeredMessage(MessageAcknowledged)
            {

                Body = result.Body,
                Crud = header.Crud,
                Id = header.MessageId,
                RoutingKey = result.RoutingKey,
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

            return Task.FromResult(message);
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
