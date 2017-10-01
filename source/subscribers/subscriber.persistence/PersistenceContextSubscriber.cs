﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Humanizer;
using Newtonsoft.Json.Linq;
using Polly;
using RabbitMQ.Client;

namespace Bespoke.Sph.Persistence
{
    public class PersistenceContextSubscriber : Subscriber
    {
        public override string QueueName => "persistence";
        public override string[] RoutingKeys => new[] { "persistence" };
        private TaskCompletionSource<bool> m_stoppingTcs;

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
            var args = new Dictionary<string, object> { { "x-dead-letter-exchange", DEAD_LETTER_EXCHANGE } };
            m_channel.QueueDeclare(this.QueueName, true, false, false, args);

            m_channel.QueueDeclare(DEAD_LETTER_QUEUE, true, false, false, args);
            m_channel.QueueBind(DEAD_LETTER_QUEUE, DEAD_LETTER_EXCHANGE, "#", null);


            foreach (var s in this.RoutingKeys)
            {
                m_channel.QueueBind(this.QueueName, EXCHANGE_NAME, s, null);
            }
            m_channel.BasicQos(0, this.PrefetchCount, false);

            m_consumer = new TaskBasicConsumer(m_channel);
            m_consumer.Received += Received;
            m_channel.BasicConsume(this.QueueName, NO_ACK, m_consumer);
        }

        private static async Task<Entity[]> GetPersistedItems(IEnumerable<Entity> items)
        {
            var list = new ObjectCollection<Entity>();
            foreach (var item in items)
            {
                var o1 = item;
                var type = item.GetEntityType();

                var option = item.GetPersistenceOption();
                if (option.IsSource)
                {
                    var file = $"{ConfigurationManager.SphSourceDirectory}\\{type.Name}\\{item.Id}.json";
                    if (!File.Exists(file))
                        continue;
                    var old = File.ReadAllText(file).DeserializeFromJson<Entity>();
                    list.Add(old);

                    continue;
                }
                if (!option.IsSqlDatabase) continue;
                if (!option.EnableAuditing) continue;

                var reposType = typeof(IRepository<>).MakeGenericType(type);
                var repos = ObjectBuilder.GetObject(reposType);

                var p = await repos.LoadOneAsync(o1.Id).ConfigureAwait(false);
                if (null != p)
                    list.Add(p);


            }
            return list.ToArray();
        }

        private async void Received(object sender, ReceivedMessageArgs e)
        {
            Interlocked.Increment(ref m_processing);
            
            var body = e.Body;
            var json = await DecompressAsync(body);
            var headers = new MessageHeaders(e);
            var operation = headers.Operation;
            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();

            var jo = JObject.Parse(json);
            var entities = jo.SelectToken("$.attached").Select(t => t.ToString().DeserializeFromJson<Entity>()).ToList();
            var deletedItems = jo.SelectToken("$.deleted").Select(t => t.ToString().DeserializeFromJson<Entity>()).ToList();

            var persistence = ObjectBuilder.GetObject<IPersistence>();
            if (headers.GetValue<bool>("data-import"))
            {
                var retry = headers.GetNullableValue<int>("sql.retry") ?? 5;
                var wait = headers.GetNullableValue<int>("sql.wait") ?? 2500;
                var bulk = await InsertImportDataAsync(entities.ToArray(), retry, wait);
                if (bulk)
                    m_channel.BasicAck(e.DeliveryTag, false);
                else
                    m_channel.BasicReject(e.DeliveryTag, false);
                return;
            }

            this.WriteMessage("{0} for {1}", headers.Operation, "item".ToQuantity(entities.Count));
            foreach (var item in entities)
            {
                this.WriteMessage("{0} for {1}{{ Id : \"{2}\"}}", headers.Operation, item.GetType().Name, item.Id);
            }
            foreach (var item in deletedItems)
            {
                this.WriteMessage("Deleting({0}) {1}{{ Id : \"{2}\"}}", headers.Operation, item.GetType().Name, item.Id);
            }

            try
            {  // get changes to items
                var previous = await GetPersistedItems(entities);
                var logs = ComputeChanges(entities, previous, operation, headers);
                var addedItems = GetAddedItems(entities, previous);
                var changedItems = GetChangedItems(entities, previous);
                entities.AddRange(logs);

                var persistedEntities = from r in entities
                                        let opt = r.GetPersistenceOption()
                                        where opt.IsSqlDatabase
                                        select r;
                var deletedEntities = from r in deletedItems
                                      let opt = r.GetPersistenceOption()
                                      where opt.IsSqlDatabase
                                      select r;

                var so = await persistence.SubmitChanges(persistedEntities.ToArray(), deletedEntities.ToArray(), null, headers.Username)
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
                    this.WriteMessage("{0} retry on SqlException : {1}", count.Ordinalize(), exc.Message);

                    var ph = headers.GetRawHeaders();
                    ph.AddOrReplace(MessageHeaders.SPH_DELAY, delay);
                    ph.AddOrReplace(MessageHeaders.SPH_TRYCOUNT, count);

                    m_channel.BasicAck(e.DeliveryTag, false);
                    publisher.SubmitChangesAsync(operation, entities, deletedItems, ph).Wait();

                }
                else
                {
                    this.WriteMessage("Error in {0}", this.GetType().Name);
                    this.WriteError(exc);
                    m_channel.BasicReject(e.DeliveryTag, false);
                }
            }
            catch (Exception exc)
            {
                this.WriteMessage("Error in {0}", this.GetType().Name);
                this.WriteError(exc);
                m_channel.BasicReject(e.DeliveryTag, false);
            }
            finally
            {
                Interlocked.Decrement(ref m_processing);
            }
        }

        private static IEnumerable<Entity> GetChangedItems(IEnumerable<Entity> entities, Entity[] previous)
        {
            return (from r in entities
                    let e1 = previous.SingleOrDefault(t => t.Id == r.Id)
                    where null != e1
                    select r).ToArray();
        }

        private static IEnumerable<Entity> GetAddedItems(IEnumerable<Entity> entities, Entity[] previous)
        {
            return (from r in entities
                    let e1 = previous.SingleOrDefault(t => t.Id == r.Id)
                    where null == e1
                    select r).ToArray();
        }

        private static AuditTrail[] ComputeChanges(IEnumerable<Entity> current, IEnumerable<Entity> previous, string operation, MessageHeaders headers)
        {
            var logs = (from r in current
                        let e1 = previous.SingleOrDefault(t => t.Id == r.Id)
                        where null != e1
                        let diffs = (new ChangeGenerator().GetChanges(e1, r))
                        let logId = Guid.NewGuid().ToString()
                        select new AuditTrail(diffs)
                        {
                            Operation = operation,
                            DateTime = DateTime.Now,
                            User = headers.Username,
                            Type = r.GetType().Name,
                            EntityId = r.Id,
                            Id = logId,
                            WebId = logId,
                            Note = "-"
                        }).ToArray();
            return logs;
        }

        private async Task<bool> InsertImportDataAsync(Entity[] entities, int retry, int wait)
        {
            var persistence = ObjectBuilder.GetObject<IPersistence>();
            try
            {
                var policy = Policy.Handle<SqlException>(ex => ex.Message.Contains("deadlocked"))
                    .WaitAndRetryAsync(retry, c => TimeSpan.FromMilliseconds(wait * c),
                        (ex, ts) =>
                        {
                            this.WriteMessage($"Waiting for retry in {ts.Seconds} seconds : \r\n{ex.Message}");
                        })
                    .ExecuteAsync(() => persistence.BulkInsertAsync(entities))
                    .ConfigureAwait(false);
                await policy;
                return true;
            }
            catch (Exception e)
            {
                ObjectBuilder.GetObject<ILogger>().Log(new LogEntry(e));
            }
            return false;
        }


        private static async Task<string> DecompressAsync(byte[] content)
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