using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;
using Bespoke.Sph.SubscribersInfrastructure;
using Humanizer;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;

namespace Bespoke.Sph.Persistence
{
    public class PersistenceContextSubscriber : Subscriber
    {
        public override string QueueName => "persistence";
        public override string[] RoutingKeys => new[] {"persistence"};
        private TaskCompletionSource<bool> m_stoppingTcs;
        private readonly List<EntityPersistence> m_receivers = new List<EntityPersistence>();

        public override void Run(IConnection connection)
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
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
            this.WriteMessage($"!!Stoping : {this.QueueName}");

            m_consumer.Received -= Received;
            m_stoppingTcs?.SetResult(true);

            //unsubscribe from the EntityPersisce.ReceivedSingle event
            foreach (var receiver in m_receivers)
            {
                m_consumer.Received -= receiver.ReceivedSingle;
            }

            while (m_processing > 0)
            {
            }


            if (null != m_channel)
            {
                m_channel.Close();
                m_channel.Dispose();
                m_channel = null;
            }

            this.WriteMessage("!!Stopped : "+ this.QueueName);
        }

        private IModel m_channel;
        private TaskBasicConsumer m_consumer;
        private int m_processing;

        public void StartConsume(IConnection connection)
        {
            const bool NO_ACK = false;
            this.OnStart();
            m_channel = connection.CreateModel();
            DeclareQueue();

            m_consumer = new TaskBasicConsumer(m_channel);
            m_consumer.Received += Received;
            m_channel.BasicConsume(this.QueueName, NO_ACK, m_consumer);

            m_receivers.Clear();
            var receivers = new SphDataContext().LoadFromSources<EntityDefinition>()
                .Where(x => x.IsPublished)
                .Select(StartConsume);

            m_receivers.AddRange(receivers);
        }

        private void DeclareQueue()
        {
            const string EXCHANGE_NAME = "sph.topic";
            const string DEAD_LETTER_EXCHANGE = "sph.ms-dead-letter";
            const string DEAD_LETTER_QUEUE = "ms_dead_letter_queue";
            m_channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic, true);

            m_channel.ExchangeDeclare(DEAD_LETTER_EXCHANGE, ExchangeType.Topic, true);
            var args = new Dictionary<string, object> {{"x-dead-letter-exchange", DEAD_LETTER_EXCHANGE}};
            m_channel.QueueDeclare(this.QueueName, true, false, false, args);

            m_channel.QueueDeclare(DEAD_LETTER_QUEUE, true, false, false, args);
            m_channel.QueueBind(DEAD_LETTER_QUEUE, DEAD_LETTER_EXCHANGE, "#", null);


            foreach (var s in this.RoutingKeys)
            {
                m_channel.QueueBind(this.QueueName, EXCHANGE_NAME, s, null);
            }
            m_channel.BasicQos(0, this.PrefetchCount, false);
        }

        private EntityPersistence StartConsume(EntityDefinition ed)
        {
            var receiver =
                new EntityPersistence(ed, m_channel, m=> this.WriteMessage(m), e => this.WriteError(e))
                {
                    PrefetchCount = this.PrefetchCount
                };
            receiver.DeclareQueue();
            const bool NO_ACK = false;

            m_consumer = new TaskBasicConsumer(m_channel);
            m_consumer.Received += receiver.ReceivedSingle;
            m_channel.BasicConsume(receiver.QueueName, NO_ACK, m_consumer);
            return receiver;
        }


        private async void Received(object sender, ReceivedMessageArgs e)
        {
            Interlocked.Increment(ref m_processing);

            var json = await e.Body.DecompressAsync();
            var headers = new MessageHeaders(e);
            var operation = headers.Operation;
            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();

            var jo = JObject.Parse(json);
            var entities = jo.SelectToken("$.attached").Select(t => t.ToString().DeserializeFromJson<Entity>())
                .ToList();
            var deletedItems = jo.SelectToken("$.deleted").Select(t => t.ToString().DeserializeFromJson<Entity>())
                .ToList();

            var persistence = ObjectBuilder.GetObject<IPersistence>();
            if (headers.GetValue<bool>("data-import"))
            {
                var retry = headers.GetNullableValue<int>("sql.retry") ?? 5;
                var wait = headers.GetNullableValue<int>("sql.wait") ?? 2500;
                var bulk = await DataImportUtility.InsertImportDataAsync(entities.ToArray(), retry, wait,
                    m => this.WriteMessage(m));
                if (bulk)
                    m_channel.BasicAck(e.DeliveryTag, false);
                else
                    m_channel.BasicReject(e.DeliveryTag, false);
                return;
            }

            this.WriteMessage($"{headers.Operation} for {"item".ToQuantity(entities.Count)}");
            foreach (var item in entities)
            {
                this.WriteMessage($"{headers.Operation} for {item.GetType().Name}{{ Id : \"{item.Id}\"}}");
            }
            foreach (var item in deletedItems)
            {
                this.WriteMessage($"Deleting({headers.Operation}) {item.GetEntityType().Name}{{ Id : \"{item.Id}\"}}");
            }

            try
            {
                // get changes to items
                var previous = await ChangeUtility.GetPersistedItems(entities);
                var logs = ChangeUtility.ComputeChanges(entities, previous, operation, headers);
                var addedItems = ChangeUtility.GetAddedItems(entities, previous);
                var changedItems = ChangeUtility.GetChangedItems(entities, previous);
                entities.AddRange(logs);

                var persistedEntities = from r in entities
                    let opt = r.GetPersistenceOption()
                    where opt.IsSqlDatabase
                    select r;
                var deletedEntities = from r in deletedItems
                    let opt = r.GetPersistenceOption()
                    where opt.IsSqlDatabase
                    select r;

                var so = await persistence
                    .SubmitChanges(persistedEntities.ToArray(), deletedEntities.ToArray(), null, headers.Username)
                    .ConfigureAwait(false);
                Trace.WriteIf(null != so.Exeption, so.Exeption);

                var logsAddedTask = publisher.PublishAdded(operation, logs, headers.GetRawHeaders());
                var addedTask = publisher.PublishAdded(operation, addedItems, headers.GetRawHeaders());
                var changedTask = publisher.PublishChanges(operation, changedItems, logs, headers.GetRawHeaders());
                var deletedTask = publisher.PublishDeleted(operation, deletedItems, headers.GetRawHeaders());
                await Task.WhenAll(addedTask, changedTask, deletedTask, logsAddedTask).ConfigureAwait(false);


                m_channel.BasicAck(e.DeliveryTag, false);
            }
            catch (SqlException exc)
            {
                // republish the message to a delayed queue
                var delay = ConfigurationManager.SqlPersistenceDelay;
                var maxTry = ConfigurationManager.SqlPersistenceMaxTry;
                if ((headers.TryCount ?? 0) < maxTry)
                {
                    var count = (headers.TryCount ?? 0) + 1;
                    this.WriteMessage($"{count.Ordinalize()} retry on SqlException : {exc.Message}");

                    var ph = headers.GetRawHeaders();
                    ph.AddOrReplace(MessageHeaders.SPH_DELAY, delay);
                    ph.AddOrReplace(MessageHeaders.SPH_TRYCOUNT, count);

                    m_channel.BasicAck(e.DeliveryTag, false);
                    await publisher.SubmitChangesAsync(operation, entities, deletedItems, ph);
                }
                else
                {
                    this.WriteMessage($"Error in {this.GetType().Name}");
                    this.WriteError(exc);
                    m_channel.BasicReject(e.DeliveryTag, false);
                }
            }
            catch (Exception exc)
            {
                this.WriteMessage($"Error in {this.GetType().Name}");
                this.WriteError(exc);
                m_channel.BasicReject(e.DeliveryTag, false);
            }
            finally
            {
                Interlocked.Decrement(ref m_processing);
            }
        }
    }
}