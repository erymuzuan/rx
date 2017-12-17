using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Messaging;
using Bespoke.Sph.Extensions;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ReadOnlyRepositoriesWorkers
{
    // ReSharper disable once UnusedMember.Global
    public class DataImportSubscriber : Subscriber
    {
        public override string QueueName => "readonly.data-import";
        public override string[] RoutingKeys => new[] {"persistence"};
        private TaskCompletionSource<bool> m_stoppingTcs;

        public override void Run(IMessageBroker broker)
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                RegisterServices();
                m_stoppingTcs = new TaskCompletionSource<bool>();
                this.StartConsume(broker).Wait();
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
            this.WriteMessage($"!!Stoping : {this.QueueName}");
            m_stoppingTcs?.SetResult(true);

            while (m_processing > 0)
            {
            }
            this.WriteMessage($"!!Stopped : {QueueName}");
        }

        private int m_processing;

        private async Task StartConsume(IMessageBroker broker)
        {
            const string DEAD_LETTER_QUEUE = "ms_dead_letter_queue";

            this.OnStart();

            await broker.CreateSubscriptionAsync(new QueueDeclareOption(this.QueueName, this.RoutingKeys)
            {
                DeadLetterQueue = DEAD_LETTER_QUEUE
            });

            broker.OnMessageDelivered(Received);
        }

        private async Task<MessageReceiveStatus> Received(BrokeredMessage message)
        {
            var syncManager = ObjectBuilder.GetObject<IReadOnlyRepositorySyncManager>();
            if (!message.IsDataImport) return MessageReceiveStatus.Dropped;

            try
            {
                Interlocked.Increment(ref m_processing);
                var body = message.Body;
                var json = await body.DecompressAsync();
                var jo = JObject.Parse(json);
                var entities = jo.SelectToken("$.attached").Select(t => t.ToString().DeserializeFromJson<Entity>())
                    .ToArray();

                await syncManager.BulkInsertAsync(entities);


                return MessageReceiveStatus.Accepted;
            }
            catch (Exception exc)
            {
                this.WriteMessage($"Error in {this.GetType().Name}");
                this.WriteError(exc);
                return MessageReceiveStatus.Rejected;
            }
            finally
            {
                Interlocked.Decrement(ref m_processing);
            }
        }
    }
}