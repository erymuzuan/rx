using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;
using Humanizer;
using RabbitMQ.Client;
using EventLog = Bespoke.Sph.Domain.EventLog;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    [Serializable]
    public abstract class Subscriber<T> : Subscriber where T : Entity
    {
        private TaskCompletionSource<bool> m_stoppingTcs;
        private EntityDefinition m_entityDefinition;
        protected abstract Task ProcessMessage(T item, MessageHeaders header);

        public override void Run(IConnection connection)
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                this.WriteMessage($"Starting {this.GetType().Name}....");
                RegisterServices();
                m_stoppingTcs = new TaskCompletionSource<bool>();
                this.StartConsume(connection);
                PrintSubscriberInformation(sw.Elapsed);
                sw.Stop();
            }
            catch (Exception e)
            {
                this.WriteError(e);
            }
        }

        protected override void OnStop()
        {
            this.WriteMessage($"Stoping : {this.QueueName}");
            
            if (m_entityDefinition?.EnableTracking ?? false)
                m_consumer.Received -= ReceivedWithTracker;
            else
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

            this.WriteMessage($"Stopped : {this.QueueName}");
        }

        protected void BasicReject(ulong tag, bool requeue = false)
        {
            m_channel.BasicReject(tag, requeue);
        }

        protected void BasicAck(ulong tag, bool multiple = false)
        {
            m_channel.BasicAck(tag, multiple);
        }

        protected void BasicNack(ulong tag, bool multiple = false, bool requeue = false)
        {
            m_channel.BasicNack(tag, multiple, requeue);
        }

        private IModel m_channel;
        private TaskBasicConsumer m_consumer;
        private int m_processing;

        public void StartConsume(IConnection connection)
        {
            const bool NO_ACK = false;
            const string EXCHANGE_NAME = "sph.topic";
            const string DEAD_LETTER_EXCHANGE = "sph.ms-dead-letter";
            const string DEAD_LETTER_QUEUE = "ms_dead_letter_queue";

            this.OnStart();

            m_channel = connection.CreateModel();

            m_channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic, true);
            m_channel.ExchangeDeclare(DEAD_LETTER_EXCHANGE, ExchangeType.Topic, true);
            var args = new Dictionary<string, object> {{"x-dead-letter-exchange", DEAD_LETTER_EXCHANGE}};
            m_channel.QueueDeclare(this.QueueName, true, false, false, args);

            m_channel.QueueDeclare(DEAD_LETTER_QUEUE, true, false, false, args);
            m_channel.QueueBind(DEAD_LETTER_QUEUE, DEAD_LETTER_EXCHANGE, "#", null);
            m_channel.QueueBind(DEAD_LETTER_QUEUE, DEAD_LETTER_EXCHANGE, "*.added", null);
            m_channel.QueueBind(DEAD_LETTER_QUEUE, DEAD_LETTER_EXCHANGE, "*.changed", null);

            foreach (var s in this.RoutingKeys)
            {
                m_channel.QueueBind(this.QueueName, EXCHANGE_NAME, s, null);
            }
            // delay exchange and queue
            var delayExchange = "sph.delay.exchange." + this.QueueName;
            var delayQueue = "sph.delay.queue." + this.QueueName;
            var delayQueueArgs = new Dictionary<string, object>
            {
                {"x-dead-letter-exchange", delayExchange},
                {"x-dead-letter-routing-key", this.QueueName}
            };
            m_channel.ExchangeDeclare(delayExchange, "direct");
            m_channel.QueueDeclare(delayQueue, true, false, false, delayQueueArgs);
            m_channel.QueueBind(delayQueue, delayExchange, string.Empty, null);

            m_channel.BasicQos(0, this.PrefetchCount, false);

            m_consumer = new TaskBasicConsumer(m_channel);


            m_entityDefinition = (new SphDataContext()).LoadOneFromSources<EntityDefinition>(x => x.Name == typeof(T).Name);
            if (m_entityDefinition?.EnableTracking ?? false)
                m_consumer.Received += ReceivedWithTracker;
            else
                m_consumer.Received += Received;
            
            m_channel.BasicConsume(this.QueueName, NO_ACK, m_consumer);
        }


        private async void Received(object sender, ReceivedMessageArgs e)
        {
            Interlocked.Increment(ref m_processing);

            var body = e.Body;
            var json = await this.DecompressAsync(body);
            var header = new MessageHeaders(e);
            var id = "";
            try
            {
                var item = json.DeserializeFromJson<T>();
                id = item.Id;

                await ProcessMessage(item, header);
                m_channel.BasicAck(e.DeliveryTag, false);
            }
            catch (Exception exc)
            {
                m_channel.BasicReject(e.DeliveryTag, false);

                this.NotificicationService.WriteError(exc, $"Exception is thrown in {QueueName}");

                var entry = new LogEntry(exc) {Source = this.QueueName, Log = EventLog.Subscribers};
                entry.OtherInfo.Add("Type", typeof(T).Name.ToLowerInvariant());
                entry.OtherInfo.Add("Id", id);
                entry.OtherInfo.Add("Requeued", false);
                entry.OtherInfo.Add("RequeuedBy", "");
                entry.OtherInfo.Add("RequeuedOn", "");
                entry.OtherInfo.Add("Id2", id.Replace("-", ""));

                var logger = ObjectBuilder.GetObject<ILogger>();
                logger.Log(entry);
            }
            finally
            {
                Interlocked.Decrement(ref m_processing);
            }
        }

        private async void ReceivedWithTracker(object sender, ReceivedMessageArgs e)
        {
            var tracker = ObjectBuilder.GetObject<IMessageTracker>();
            var cancelledMessageRepository = ObjectBuilder.GetObject<ICancelledMessageRepository>();
            var trackingTasks = new List<Task>();

            Interlocked.Increment(ref m_processing);

            var body = e.Body;
            var json = await this.DecompressAsync(body);
            var header = new MessageHeaders(e);
            var id = "";
            try
            {
                var item = json.DeserializeFromJson<T>();
                id = item.Id;

                var sw = new Stopwatch();
                sw.Start();
                trackingTasks.Add(tracker.RegisterStartProcessingAsync(
                    new MessageTrackingEvent(item, header.MessageId, e.RoutingKey, this.QueueName)));
                var cancelled = await cancelledMessageRepository.CheckMessageAsync(header.MessageId, this.QueueName);
                if (!cancelled)
                {
                    await ProcessMessage(item, header);
                    m_channel.BasicAck(e.DeliveryTag, false);

                    var completedEvent =
                        new MessageTrackingEvent(item, header.MessageId, e.RoutingKey, this.QueueName)
                        {
                            ProcessingTimeSpan = sw.Elapsed
                        };
                    var completed = tracker.RegisterCompletedAsync(completedEvent);
                    trackingTasks.Add(completed);
                }
                else
                {
                    m_channel.BasicAck(e.DeliveryTag, false);
                    trackingTasks.Add(tracker.RegisterCancelledAsync(
                        new MessageTrackingEvent(item, header.MessageId, e.RoutingKey, this.QueueName)));
                    trackingTasks.Add(cancelledMessageRepository.RemoveAsync(header.MessageId, this.QueueName));
                }

                sw.Stop();
            }
            catch (Exception exc)
            {
                m_channel.BasicReject(e.DeliveryTag, false);

                trackingTasks.Add(tracker.RegisterDlqedAsync(new MessageTrackingEvent(json.DeserializeFromJson<T>(),
                    header.MessageId, header.Operation, this.QueueName)));

                this.NotificicationService.WriteError(exc, $"Exception is thrown in {QueueName}");

                var entry = new LogEntry(exc) {Source = this.QueueName, Log = EventLog.Subscribers};
                entry.OtherInfo.Add("Type", typeof(T).Name.ToLowerInvariant());
                entry.OtherInfo.Add("Id", id);
                entry.OtherInfo.Add("Requeued", false);
                entry.OtherInfo.Add("RequeuedBy", "");
                entry.OtherInfo.Add("RequeuedOn", "");
                entry.OtherInfo.Add("Id2", id.Replace("-", ""));

                var logger = ObjectBuilder.GetObject<ILogger>();
                logger.Log(entry);
            }
            finally
            {
                Interlocked.Decrement(ref m_processing);
                await Task.WhenAll(trackingTasks);
            }
        }

        protected void PublishToDelayQueue(IBasicProperties props, byte[] body, string routingKey)
        {
            var count = 91;
            if (props.Headers.ContainsKey("sph.trycount"))
                count = (int) props.Headers["sph.trycount"];
            Console.WriteLine(@"Doing the delay for {0} ms for the {1} time", props.Headers["sph.delay"],
                count.Ordinalize());
            const string RETRY_EXCHANGE = "sph.retry.exchange";
            var delay = (long) props.Headers["sph.delay"]; // in ms

            props.Expiration = delay.ToString(CultureInfo.InvariantCulture);

            m_channel.BasicPublish(RETRY_EXCHANGE, string.Empty, props, body);
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
                    var text = await sr.ReadToEndAsync();
                    return text;
                }
            }
        }
    }
}