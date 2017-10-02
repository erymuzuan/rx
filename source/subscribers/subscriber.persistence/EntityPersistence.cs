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
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Bespoke.Sph.Persistence
{
    internal class EntityPersistence
    {
        public EntityDefinition Ed { get; }
        public Action<string> WriteMessage { get; }
        public Action<Exception> WriteError { get; }
        private int m_processing;
        private readonly IModel m_channel;
        public ushort PrefetchCount { get; set; } = 1;
        public string QueueName => $"persistence.{Ed.Name}";

        public EntityPersistence(EntityDefinition ed, IModel channel, Action<string> writeMessage, Action<Exception> writeError)
        {
            Ed = ed;
            WriteMessage = writeMessage;
            WriteError = writeError;
            m_channel = channel;
        }

        public void DeclareQueue()
        {
            var binding = $"persistence.{Ed.Name}";
            const string EXCHANGE_NAME = "sph.topic";
            const string DEAD_LETTER_EXCHANGE = "sph.ms-dead-letter";
            const string DEAD_LETTER_QUEUE = "ms_dead_letter_queue";
            
            m_channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic, true);

            m_channel.ExchangeDeclare(DEAD_LETTER_EXCHANGE, ExchangeType.Topic, true);
            var args = new Dictionary<string, object> { { "x-dead-letter-exchange", DEAD_LETTER_EXCHANGE } };
            m_channel.QueueDeclare(this.QueueName, true, false, false, args);

            m_channel.QueueDeclare(DEAD_LETTER_QUEUE, true, false, false, args);
            m_channel.QueueBind(DEAD_LETTER_QUEUE, DEAD_LETTER_EXCHANGE, "#", null);

            m_channel.QueueBind(this.QueueName, EXCHANGE_NAME, binding, null);
            m_channel.BasicQos(0, this.PrefetchCount, false);

          
        }


        public async void ReceivedSingle(object sender, ReceivedMessageArgs e)
        {
            Interlocked.Increment(ref m_processing);
            var json = await e.Body.DecompressAsync();
            var headers = new MessageHeaders(e);
            var operation = headers.Operation;
            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();

            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            var entities = new List<Entity>
            {
                JsonConvert.DeserializeObject(json, setting) as Entity
            };
            var item = entities.Single();

            var deletedItems = Array.Empty<Entity>();

            var persistence = ObjectBuilder.GetObject<IPersistence>();
            if (headers.GetValue<bool>("data-import"))
            {
                var retry = headers.GetNullableValue<int>("sql.retry") ?? 5;
                var wait = headers.GetNullableValue<int>("sql.wait") ?? 2500;
                var bulk = await DataImportUtility.InsertImportDataAsync(new[] { item }, retry, wait, this.WriteMessage);
                if (bulk)
                    m_channel.BasicAck(e.DeliveryTag, false);
                else
                    m_channel.BasicReject(e.DeliveryTag, false);
                return;
            }


            this.WriteMessage($"{headers.Operation} for {item.GetType().Name}{{ Id : \"{item.Id}\"}}");



            try
            {
                // get changes to items
                var previous = await ChangeUtility.GetPersistedItems(new[] { item });
                var logs = ChangeUtility.ComputeChanges(new[] { item }, previous, operation, headers);
                var addedItems = ChangeUtility.GetAddedItems(new[] { item }, previous);
                var changedItems = ChangeUtility.GetChangedItems(new[] { item }, previous);
                entities.AddRange(logs);

                var persistedEntities = from r in entities
                                        let opt = r.GetPersistenceOption()
                                        where opt.IsSqlDatabase
                                        select r;
                var so = await persistence.SubmitChanges(persistedEntities.ToArray(), Array.Empty<Entity>(), null, headers.Username).ConfigureAwait(false);
                Trace.WriteIf(null != so.Exeption, so.Exeption);

                var logsAddedTask = publisher.PublishAdded(operation, logs, headers.GetRawHeaders());
                var addedTask = publisher.PublishAdded(operation, addedItems, headers.GetRawHeaders());
                var changedTask = publisher.PublishChanges(operation, changedItems, logs, headers.GetRawHeaders());
                await Task.WhenAll(addedTask, changedTask, logsAddedTask).ConfigureAwait(false);


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
                    publisher.SubmitChangesAsync(operation, entities, deletedItems, ph).Wait();

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
                this.WriteMessage("Error in" + this.GetType().Name);
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
