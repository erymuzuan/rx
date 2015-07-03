using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using RabbitMQ.Client;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    [Serializable]
    public abstract class Subscriber<T> : Subscriber where T : Entity
    {
        private TaskCompletionSource<bool> m_stoppingTcs;
        protected abstract Task ProcessMessage(T item, MessageHeaders header);
     
        public override void Run()
        {
            var sw = new Stopwatch();
            sw.Start();
            try
            {
                this.WriteMessage("Starting {0}....", this.GetType().Name);
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

        protected void BasicReject(ulong tag, bool requeue = false)
        {
            m_channel.BasicReject(tag, requeue);
        }
        protected void BasicAck(ulong tag, bool multiple = false)
        {
            m_channel.BasicAck(tag, multiple);
        }
        protected void BasicNack(ulong tag, bool multiple = false, bool requeue = false)
        {
            m_channel.BasicNack(tag, multiple, requeue);
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
            m_channel.QueueBind(DEAD_LETTER_QUEUE, DEAD_LETTER_EXCHANGE, "*.added", null);
            m_channel.QueueBind(DEAD_LETTER_QUEUE, DEAD_LETTER_EXCHANGE, "*.changed", null);

            foreach (var s in this.RoutingKeys)
            {
                m_channel.QueueBind(this.QueueName, EXCHANGE_NAME, s, null);
            }
            // delay exchange and queue
            var delayExchange = "sph.delay.exchange." + this.QueueName;
            var delayQueue = "sph.delay.queue." + this.QueueName;
            var delayQueueArgs = new Dictionary<string, object>
            {
                {"x-dead-letter-exchange", delayExchange},
                {"x-dead-letter-routing-key", this.QueueName}
            };
            m_channel.ExchangeDeclare(delayExchange, "direct");
            m_channel.QueueDeclare(delayQueue, true, false, false, delayQueueArgs);
            m_channel.QueueBind(delayQueue, delayExchange, string.Empty, null);



            m_channel.BasicQos(0, this.PrefectchCount, false);

            m_consumer = new TaskBasicConsumer(m_channel);
            m_consumer.Received += Received;
            m_channel.BasicConsume(this.QueueName, NO_ACK, m_consumer);



        }



        private async void Received(object sender, ReceivedMessageArgs e)
        {
            Interlocked.Increment(ref m_processing);
            byte[] body = e.Body;
            var json = await this.DecompressAsync(body);
            var header = new MessageHeaders(e);
            try
            {
                var item = json.DeserializeFromJson<T>();
                await ProcessMessage(item, header);
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