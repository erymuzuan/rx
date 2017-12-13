using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Messaging;
using Bespoke.Sph.Extensions;
using Humanizer;
using Newtonsoft.Json;

namespace Bespoke.Sph.Persistence
{
    internal class EntityPersistence
    {
        public EntityDefinition Ed { get; }
        public Action<string> WriteMessage { get; }
        public Action<Exception> WriteError { get; }
        private int m_processing;
        private readonly IMessageBroker m_broker;
        public ushort PrefetchCount { get; set; } = 1;
        public string QueueName => $"persistence.{Ed.Name}";

        public EntityPersistence(EntityDefinition ed, IMessageBroker broker, Action<string> writeMessage,
            Action<Exception> writeError)
        {
            Ed = ed;
            WriteMessage = writeMessage;
            WriteError = writeError;
            m_broker = broker;
        }

        public async Task DeclareQueue(IMessageBroker broker)
        {
            var binding = $"persistence.{Ed.Name}";
            const string DEAD_LETTER_QUEUE = "ms_dead_letter_queue";

            await broker.CreateSubscriptionAsync(new QueueSubscriptionOption(this.QueueName, binding)
            {
                DeadLetterQueue = DEAD_LETTER_QUEUE
            });
        }


        public async Task<MessageReceiveStatus> ReceivedSingle(BrokeredMessage msg)
        {
            Interlocked.Increment(ref m_processing);
            var json = await msg.Body.DecompressAsync();
            var operation = msg.Operation;
            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();

            var setting = new JsonSerializerSettings {TypeNameHandling = TypeNameHandling.All};
            var entities = new List<Entity>
            {
                JsonConvert.DeserializeObject(json, setting) as Entity
            };
            var item = entities.Single();

            var deletedItems = Array.Empty<Entity>();

            var persistence = ObjectBuilder.GetObject<IPersistence>();
            if (msg.IsDataImport)
            {
                var retry = msg.GetNullableValue<int>("sql.retry") ?? 5;
                var wait = msg.GetNullableValue<int>("sql.wait") ?? 2500;
                var bulk = await DataImportUtility.InsertImportDataAsync(new[] {item}, retry, wait, this.WriteMessage);
                return bulk ? MessageReceiveStatus.Accepted : MessageReceiveStatus.Rejected;
            }


            this.WriteMessage($"{msg.Operation} for {item.GetType().Name}{{ Id : \"{item.Id}\"}}");


            try
            {
                // get changes to items
                var previous = await ChangeUtility.GetPersistedItems(new[] {item});
                var logs = ChangeUtility.ComputeChanges(new[] {item}, previous, operation, msg);
                var addedItems = ChangeUtility.GetAddedItems(new[] {item}, previous);
                var changedItems = ChangeUtility.GetChangedItems(new[] {item}, previous);
                entities.AddRange(logs);

                var persistedEntities = from r in entities
                    let opt = r.GetPersistenceOption()
                    where opt.IsSqlDatabase
                    select r;
                var so = await persistence
                    .SubmitChanges(persistedEntities.ToArray(), Array.Empty<Entity>(), null, msg.Username)
                    .ConfigureAwait(false);
                Trace.WriteIf(null != so.Exeption, so.Exeption);

                var logsAddedTask = publisher.PublishAdded(operation, logs, msg.Headers);
                var addedTask = publisher.PublishAdded(operation, addedItems, msg.Headers);
                var changedTask = publisher.PublishChanges(operation, changedItems, logs, msg.Headers);
                await Task.WhenAll(addedTask, changedTask, logsAddedTask).ConfigureAwait(false);


                return MessageReceiveStatus.Accepted;
            }
            catch (SqlException exc)
            {
                // republish the message to a delayed queue
                var delay = ConfigurationManager.SqlPersistenceDelay;
                var maxTry = ConfigurationManager.SqlPersistenceMaxTry;
                if ((msg.TryCount ?? 0) < maxTry)
                {
                    var count = (msg.TryCount ?? 0) + 1;
                    this.WriteMessage($"{count.Ordinalize()} retry on SqlException : {exc.Message}");


                    msg.TryCount = count;
                    msg.RetryDelay = TimeSpan.FromMilliseconds(delay);

                    await publisher.SubmitChangesAsync(operation, entities, deletedItems, msg.Headers);
                    return MessageReceiveStatus.Dropped;
                }
                else
                {
                    this.WriteMessage($"Error in {this.GetType().Name}");
                    this.WriteError(exc);
                    
                    return MessageReceiveStatus.Rejected;
                }
            }
            catch (Exception exc)
            {
                this.WriteMessage("Error in " + this.GetType().Name);
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