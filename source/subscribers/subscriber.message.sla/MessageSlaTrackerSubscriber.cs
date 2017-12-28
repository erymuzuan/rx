using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Messaging;
using Bespoke.Sph.Extensions;
using Bespoke.Sph.SubscribersInfrastructure;
using Polly;

namespace Bespoke.Sph.MessageTrackerSla
{
    // ReSharper disable once UnusedMember.Global
    public class MessageSlaTrackerSubscriber : Subscriber
    {
        public override string QueueName => MessageBrokerExtension.NOTIFICATION_QUEUE;

        public override string[] RoutingKeys => new[] { MessageBrokerExtension.NOTIFICATION_QUEUE };
        private TaskCompletionSource<bool> m_stoppingTcs;

        public override void Run(IMessageBroker broker)
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                m_stoppingTcs = new TaskCompletionSource<bool>();

                broker.CreateSlaMonitorQueueAsync().Wait();
                this.OnStart();
                broker.OnMessageDelivered(Received, new SubscriberOption(this.QueueName));

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
            this.WriteMessage($"!!Stoping : {QueueName}");

            m_stoppingTcs?.SetResult(true);

            while (m_processing > 0)
            {

            }
            this.WriteMessage($"!!Stopped : {QueueName}");
        }


        private int m_processing;


        private async Task<MessageReceiveStatus> Received(BrokeredMessage message)
        {
            Interlocked.Increment(ref m_processing);
            var logger = ObjectBuilder.GetObject<ILogger>();

            try
            {
                var json = await message.Body.DecompressAsync();

                var @event = json.DeserializeFromJson<MessageSlaEvent>();

                var tracker = ObjectBuilder.GetObject<IMessageTracker>();
                var statusResult = await Policy.Handle<Exception>()
                    .WaitAndRetryAsync(3, c => TimeSpan.FromMilliseconds(100 * Math.Pow(2, c)))
                    .ExecuteAndCaptureAsync(() => tracker.GetProcessStatusAsync(@event));
                if (null != statusResult.FinalException)
                {
                    logger.WriteError($"Fail to get process status for {@event.MessageId}  after 3 attempts");
                    await logger.LogAsync(new LogEntry(statusResult.FinalException));
                    return MessageReceiveStatus.Rejected;
                }
                var status = statusResult.Result;

                this.WriteMessage($@"[{message.Id}] is ""{status}"" for ""{@event.Worker}""");

                var manager = ObjectBuilder.GetObject<IMessageSlaManager>();
                await manager.ExecuteOnNotificationAsync(status, @event);

                return MessageReceiveStatus.Accepted;
            }
            finally
            {
                Interlocked.Decrement(ref m_processing);
            }
        }


    }


}