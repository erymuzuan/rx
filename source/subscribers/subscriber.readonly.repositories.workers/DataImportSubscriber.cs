using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;
using Bespoke.Sph.SubscribersInfrastructure;
using Newtonsoft.Json.Linq;
using RabbitMQ.Client;

namespace Bespoke.Sph.ReadOnlyRepositoriesWorkers
{
    public class DataImportSubscriber : Subscriber
    {
        public override string QueueName => "es.data-import";
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
            this.WriteMessage($"!!Stoping : {this.QueueName}");

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

            this.WriteMessage($"!!Stopped : {QueueName}");
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

        private async void Received(object sender, ReceivedMessageArgs e)
        {
            var syncManager = ObjectBuilder.GetObject<IReadOnlyRepositorySyncManager>();
            var headers = new MessageHeaders(e);
            if (!headers.GetValue<bool>("data-import"))
            {
                m_channel.BasicAck(e.DeliveryTag, false);
                return;
            }

            try
            {
                Interlocked.Increment(ref m_processing);
                var body = e.Body;
                var json = await body.DecompressAsync();
                var jo = JObject.Parse(json);
                var entities = jo.SelectToken("$.attached").Select(t => t.ToString().DeserializeFromJson<Entity>())
                    .ToArray();

                await syncManager.BulkInsertAsync(entities);


                m_channel.BasicAck(e.DeliveryTag, false);
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