using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using RabbitMQ.Client;

namespace Bespoke.Sph.MessageTrackerSla
{
    public class MessageSlaTrackerSubscriber : Subscriber
    {
        public const string DELAY_EXCHANGE = "rx.delay.exchange.messages.sla";
        public const string DELAY_QUEUE = "rx.delay.queue.messages.sla";
        public const string NOTIFICATION_EXCHANGE = "rx.notification.exchange.messages.sla";
        public const string NOTIFICATION_QUEUE = "rx.notification.queue.messages.sla";
        public const int PERSISTENT_DELIVERY_MODE = 2;
        public override string QueueName => NOTIFICATION_QUEUE;

        public override string[] RoutingKeys => new[] { NOTIFICATION_QUEUE };
        private TaskCompletionSource<bool> m_stoppingTcs;

        public override void Run(IConnection connection)
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                m_stoppingTcs = new TaskCompletionSource<bool>();

                m_channel = connection.CreateModel();
                this.CreateSlaMonitorQueue();
                this.StartConsume();

                PrintSubscriberInformation(sw.Elapsed);
                sw.Stop();

            }
            catch (Exception e)
            {
                this.WriteError(e);
            }
        }

        private void CreateSlaMonitorQueue()
        {
            var delayQueueArgs = new Dictionary<string, object>
            {
                {"x-dead-letter-exchange", NOTIFICATION_EXCHANGE}
            };
            m_channel.ExchangeDeclare(DELAY_EXCHANGE, "direct");
            m_channel.ExchangeDeclare(NOTIFICATION_EXCHANGE, "direct");
            m_channel.QueueDeclare(DELAY_QUEUE, true, false, false, delayQueueArgs);
            m_channel.QueueBind(DELAY_QUEUE, DELAY_EXCHANGE, string.Empty, null);

            m_channel.QueueDeclare(NOTIFICATION_QUEUE, true, false, false, new Dictionary<string, object>());
            m_channel.QueueBind(NOTIFICATION_QUEUE, NOTIFICATION_EXCHANGE, string.Empty, null);
        }

        protected override void OnStop()
        {
            this.WriteMessage("!!Stoping : {0}", this.QueueName);

            m_consumer.Received -= Received;
            m_stoppingTcs?.SetResult(true);

            while (m_processing > 0)
            {

            }


            if (null != m_channel)
            {
                m_channel.Close();
                m_channel.Dispose();
                m_channel = null;
            }

            this.WriteMessage("!!Stopped : {0}", this.QueueName);
        }

        private IModel m_channel;
        private TaskBasicConsumer m_consumer;
        private int m_processing;

        public void StartConsume()
        {
            const bool NO_ACK = false;
            this.OnStart();

            m_consumer = new TaskBasicConsumer(m_channel);
            m_consumer.Received += Received;
            m_channel.BasicConsume(this.QueueName, NO_ACK, m_consumer);
        }

        private async void Received(object sender, ReceivedMessageArgs e)
        {
            Interlocked.Increment(ref m_processing);

            try
            {
                var body = e.Body;
                var json = await DecompressAsync(body);
                var headers = new MessageHeaders(e);

                var @event = json.DeserializeFromJson<MessageSlaEvent>();
                this.WriteMessage($" TODO : cross check with message event, if the message-id:{@event.MessageId}+worker:{@event.Worker}+processcompleted exist ");
                this.WriteMessage($"Header messageId : {headers.MessageId}");

                var tracker = ObjectBuilder.GetObject<IMessageTracker>();
                var status = await tracker.GetProcessStatusAsync(@event);

                var manager = ObjectBuilder.GetObject<IMessageSlaManager>();
                await manager.ExecuteOnNotificationAsync(status, @event);




                m_channel.BasicAck(e.DeliveryTag, false);
            }
            finally
            {
                Interlocked.Decrement(ref m_processing);
            }
        }


        private async Task<string> DecompressAsync(byte[] content)
        {
            using (var orginalStream = new MemoryStream(content))
            using (var destinationStream = new MemoryStream())
            using (var gzip = new GZipStream(orginalStream, CompressionMode.Decompress))
            {
                try
                {
                    await gzip.CopyToAsync(destinationStream);
                }
                catch (InvalidDataException)
                {
                    orginalStream.CopyTo(destinationStream);
                }
                destinationStream.Position = 0;
                using (var sr = new StreamReader(destinationStream))
                {
                    var json = await sr.ReadToEndAsync();
                    return json;
                }
            }
        }
    }


}