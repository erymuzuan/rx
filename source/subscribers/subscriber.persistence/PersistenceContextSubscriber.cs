using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Humanizer;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;

namespace Bespoke.Sph.Persistence
{
    public class PersistenceContextSubscriber : Subscriber
    {
        public override string QueueName => "persistence";

        public override string[] RoutingKeys => new[] { "persistence" };

        private TaskCompletionSource<bool> m_stoppingTcs;
        /// <summary>
        /// The number of messages prefetch by the broker in a batch.
        /// </summary>
        /// <returns></returns>
        protected virtual uint GetParallelProcessing()
        {
            return 1;
        }

        public override void Run()
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
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
            this.WriteMessage("!!Stoping : {0}", this.QueueName);

            m_consumer.Received -= Received;
            m_stoppingTcs?.SetResult(true);

            while (m_processing > 0)
            {

            }

            if (null != m_connection)
            {
                m_connection.Close();
                m_connection.Dispose();
                m_connection = null;
            }

            if (null != m_channel)
            {
                m_channel.Close();
                m_channel.Dispose();
                m_channel = null;
            }

            this.WriteMessage("!!Stopped : {0}", this.QueueName);
        }

        private IConnection m_connection;
        private IModel m_channel;
        private TaskBasicConsumer m_consumer;
        private int m_processing;

        public void StartConsume()
        {
            const bool NO_ACK = false;
            const string EXCHANGE_NAME = "sph.topic";
            const string DEAD_LETTER_EXCHANGE = "sph.ms-dead-letter";
            const string DEAD_LETTER_QUEUE = "ms_dead_letter_queue";

            this.OnStart();

            var factory = new ConnectionFactory
            {
                UserName = this.UserName,
                VirtualHost = this.VirtualHost,
                Password = this.Password,
                HostName = this.HostName,
                Port = this.Port
            };
            m_connection = factory.CreateConnection();
            m_channel = m_connection.CreateModel();


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
            m_channel.BasicQos(0, (ushort)this.GetParallelProcessing(), false);

            m_consumer = new TaskBasicConsumer(m_channel);
            m_consumer.Received += Received;
            m_channel.BasicConsume(this.QueueName, NO_ACK, m_consumer);



        }
        private async Task<IEnumerable<Entity>> GetPreviousItems(IEnumerable<Entity> items)
        {
            var list = new ObjectCollection<Entity>();
            foreach (var item in items)
            {
                var o1 = item;
                var type = item.GetEntityType();

                var source = StoreAsSourceAttribute.GetAttribute(type);
                if (null != source)
                {
                    var file = $"{ConfigurationManager.SphSourceDirectory}\\{type.Name}\\{item.Id}.json";
                    if (!File.Exists(file))
                        continue;

                    var old = File.ReadAllText(file).DeserializeFromJson<Entity>();
                    list.Add(old);

                    continue;
                }


                var reposType = typeof(IRepository<>).MakeGenericType(type);
                var repos = ObjectBuilder.GetObject(reposType);

                var p = await repos.LoadOneAsync(o1.Id).ConfigureAwait(false);
                if (null != p)
                    list.Add(p);


            }
            return list;
        }

        private async void Received(object sender, ReceivedMessageArgs e)
        {
            Interlocked.Increment(ref m_processing);
            byte[] body = e.Body;
            var json = await this.DecompressAsync(body);
            var headers = new MessageHeaders(e);
            var operation = headers.Operation;
            var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();

            var jo = JObject.Parse(json);
            var entities = jo.SelectToken("$.attached").Select(t => t.ToString().DeserializeFromJson<Entity>()).ToList();
            var deletedItems = jo.SelectToken("$.deleted").Select(t => t.ToString().DeserializeFromJson<Entity>()).ToList();



            try
            {
                this.WriteMessage("{0} for {1}", headers.Operation, "item".ToQuantity(entities.Count));
                foreach (var item in entities)
                {
                    this.WriteMessage("{0} for {1}{{ Id : \"{2}\"}}", headers.Operation, item.GetType().Name, item.Id);
                }
                foreach (var item in deletedItems)
                {
                    this.WriteMessage("Deleting({0}) {1}{{ Id : \"{2}\"}}", headers.Operation, item.GetType().Name, item.Id);
                }
                // get changes to items
                var previous = await GetPreviousItems(entities);
                var logs = (from r in entities
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
                var addedItems = (from r in entities
                                  let e1 = previous.SingleOrDefault(t => t.Id == r.Id)
                                  where null == e1
                                  select r).ToArray();
                var changedItems = (from r in entities
                                    let e1 = previous.SingleOrDefault(t => t.Id == r.Id)
                                    where null != e1
                                    select r).ToArray();
                entities.AddRange(logs);

                var persistence = ObjectBuilder.GetObject<IPersistence>();
                var so = await persistence.SubmitChanges(entities, deletedItems, null, headers.Username)
                .ConfigureAwait(false);
                Debug.WriteLine(so.ToJsonString(true));


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