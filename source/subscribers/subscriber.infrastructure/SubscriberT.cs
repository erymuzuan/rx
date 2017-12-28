using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Domain.Messaging;
using Bespoke.Sph.Extensions;
using EventLog = Bespoke.Sph.Domain.EventLog;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    [Serializable]
    public abstract class Subscriber<T> : Subscriber where T : Entity
    {
        private TaskCompletionSource<bool> m_stoppingTcs;
        private EntityDefinition m_entityDefinition;
        private static IMessageBroker Broker => ObjectBuilder.GetObject<IMessageBroker>();
        protected abstract Task ProcessMessage(T item, BrokeredMessage message);
        public override void Run(IMessageBroker broker)
        {
            var repos = ObjectBuilder.GetObject<ISourceRepository>();
            this.m_entityDefinition = repos.LoadOneAsync<EntityDefinition>(x => x.Name == typeof(T).Name).Result;

            var sw = new Stopwatch();
            sw.Start();
            try
            {
                this.WriteMessage($"Starting {this.GetType().Name}....");
                RegisterServices();
                m_stoppingTcs = new TaskCompletionSource<bool>();
                this.StartConsume();
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


            m_stoppingTcs?.SetResult(true);

            while (m_processing > 0)
            {
            }

            Broker.Dispose();
            this.WriteMessage($"Stopped : {this.QueueName}");
        }


        private int m_processing;

        private void StartConsume()
        {
            this.OnStart();
            var option = new SubscriberOption(this.QueueName)
            {
                PrefetchCount = this.PrefetchCount
            };
            Broker.CreateSubscriptionAsync(new QueueDeclareOption(this.QueueName, this.RoutingKeys)).Wait();
            if (m_entityDefinition?.EnableTracking ?? false)
                Broker.OnMessageDelivered(ReceivedWithTracker, option);
            else
                Broker.OnMessageDelivered(Received, option);
        }


        private async Task<MessageReceiveStatus> Received(BrokeredMessage message)
        {
            Interlocked.Increment(ref m_processing);

            var json = await message.Body.DecompressAsync();
            var item = json.DeserializeFromJson<T>();
            try
            {
                await ProcessMessage(item, message);
                return MessageReceiveStatus.Accepted;
            }
            catch (Exception exc)
            {
                this.NotificicationService.WriteError(exc, $"Exception is thrown in {QueueName}");

                var entry = new LogEntry(exc) { Source = this.QueueName, Log = EventLog.Subscribers };
                entry.OtherInfo.Add("Type", typeof(T).Name.ToLowerInvariant());
                entry.OtherInfo.Add("Id", item.Id);
                entry.OtherInfo.Add("Requeued", false);
                entry.OtherInfo.Add("RequeuedBy", "");
                entry.OtherInfo.Add("RequeuedOn", "");
                entry.OtherInfo.Add("Id2", item.Id.Replace("-", ""));

                var logger = ObjectBuilder.GetObject<ILogger>();
                logger.Log(entry);

                return MessageReceiveStatus.Rejected;
            }
            finally
            {
                Interlocked.Decrement(ref m_processing);
            }
        }

        private async Task<MessageReceiveStatus> ReceivedWithTracker(BrokeredMessage message)
        {
            var tracker = ObjectBuilder.GetObject<IMessageTracker>();
            var cancelledMessageRepository = ObjectBuilder.GetObject<ICancelledMessageRepository>();
            var trackingTasks = new List<Task>();

            Interlocked.Increment(ref m_processing);

            var json = await message.Body.DecompressAsync();
            var id = "";
            try
            {
                var item = json.DeserializeFromJson<T>();
                id = item.Id;

                var sw = new Stopwatch();
                sw.Start();
                trackingTasks.Add(tracker.RegisterStartProcessingAsync(
                    new MessageTrackingEvent(item, message.Id, message.RoutingKey, this.QueueName)));
                var cancelled = await cancelledMessageRepository.CheckMessageAsync(message.Id, this.QueueName);
                if (!cancelled)
                {
                    await ProcessMessage(item, message);

                    var completedEvent =
                        new MessageTrackingEvent(item, message.Id, message.RoutingKey, this.QueueName)
                        {
                            ProcessingTimeSpan = sw.Elapsed
                        };
                    var completed = tracker.RegisterCompletedAsync(completedEvent);
                    trackingTasks.Add(completed);
                }
                else
                {
                    trackingTasks.Add(tracker.RegisterCancelledAsync(
                        new MessageTrackingEvent(item, message.Id, message.RoutingKey, this.QueueName)));
                    trackingTasks.Add(cancelledMessageRepository.RemoveAsync(message.Id, this.QueueName));
                }
                sw.Stop();
                return MessageReceiveStatus.Accepted;
            }
            catch (Exception exc)
            {
                trackingTasks.Add(tracker.RegisterDlqedAsync(new MessageTrackingEvent(json.DeserializeFromJson<T>(),
                    message.Id, message.Operation, this.QueueName)));

                this.NotificicationService.WriteError(exc, $"Exception is thrown in {QueueName}");

                var entry = new LogEntry(exc) { Source = this.QueueName, Log = EventLog.Subscribers };
                entry.OtherInfo.Add("Type", typeof(T).Name.ToLowerInvariant());
                entry.OtherInfo.Add("Id", id);
                entry.OtherInfo.Add("Requeued", false);
                entry.OtherInfo.Add("RequeuedBy", "");
                entry.OtherInfo.Add("RequeuedOn", "");
                entry.OtherInfo.Add("Id2", id.Replace("-", ""));

                var logger = ObjectBuilder.GetObject<ILogger>();
                logger.Log(entry);

                return MessageReceiveStatus.Rejected;
            }
            finally
            {
                Interlocked.Decrement(ref m_processing);
                await Task.WhenAll(trackingTasks);
            }
        }
    }
}