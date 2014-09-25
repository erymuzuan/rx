using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;

namespace Bespoke.Sph.Persistence
{
    public class PersistenceContextSubscriber : Subscriber
    {
        public override string QueueName
        {
            get { return "persistence"; }
        }

        public override string[] RoutingKeys
        {
            get { return new[] { "persistence" }; }
        }

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
            try
            {
                RegisterServices();
                PrintSubscriberInformation();
                m_stoppingTcs = new TaskCompletionSource<bool>();
                this.StartConsume();
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
            if (null != m_stoppingTcs)
                m_stoppingTcs.SetResult(true);

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
                var reposType = typeof(IRepository<>).MakeGenericType(new[] { type });
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
            try
            {
                this.WriteMessage(headers.Operation);

                var jo = JObject.Parse(json);
                var attachedCollection = jo.SelectToken("$.attached").Select(t => t.ToString().DeserializeFromJson<Entity>()).ToList();
                var deletedCollection = jo.SelectToken("$.deleted").Select(t => t.ToString().DeserializeFromJson<Entity>()).ToList();

                // get changes to items
                var previous = await GetPreviousItems(attachedCollection);
                var logs = (from r in attachedCollection
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
                attachedCollection.AddRange(logs);


                var persistence = ObjectBuilder.GetObject<IPersistence>();
                var so = await persistence.SubmitChanges(attachedCollection, deletedCollection, null)
                .ConfigureAwait(false);
                Debug.WriteLine(so.ToJsonString(true));


                var publisher = ObjectBuilder.GetObject<IEntityChangePublisher>();
                var logsAddedTask = publisher.PublishAdded(operation, logs, headers.GetRawHeaders());
                var addedTask = publisher.PublishAdded(operation, attachedCollection, headers.GetRawHeaders());
                var changedTask = publisher.PublishChanges(operation, attachedCollection, logs, headers.GetRawHeaders());
                var deletedTask = publisher.PublishDeleted(operation, deletedCollection, headers.GetRawHeaders());
                await Task.WhenAll(addedTask, changedTask, deletedTask, logsAddedTask).ConfigureAwait(false);


                m_channel.BasicAck(e.DeliveryTag, false);
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