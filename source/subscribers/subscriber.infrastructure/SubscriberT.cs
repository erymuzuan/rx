using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
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
            RegisterServices();
            PrintSubscriberInformation();
            m_stoppingTcs = new TaskCompletionSource<bool>();
            this.StartConsume().Wait();
        }

        protected override void OnStop()
        {
            m_stoppingTcs.SetResult(true);
        }

        public async Task StartConsume()
        {
            const bool noAck = false;
            const string exchangeName = "sph.topic";
            const string deadLetterExchange = "sph.ms-dead-letter";
            const string deadLetterQueue = "ms_dead_letter_queue";

            this.OnStart();

            var factory = new ConnectionFactory
            {
                UserName = this.UserName,
                VirtualHost = this.VirtualHost,
                Password = this.Password,
                HostName = this.HostName,
                Port = this.Port
            };

            using (var conn = factory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true);

                channel.ExchangeDeclare(deadLetterExchange, ExchangeType.Topic, true);
                var args = new Dictionary<string, object> { { "x-dead-letter-exchange", deadLetterExchange } };
                channel.QueueDeclare(this.QueueName, true, false, false, args);
                 
                channel.QueueDeclare(deadLetterQueue, true, false, false, args);
                channel.QueueBind(deadLetterQueue, deadLetterExchange, "#", null);
                channel.QueueBind(deadLetterQueue, deadLetterExchange, "*.added", null);
                channel.QueueBind(deadLetterQueue, deadLetterExchange, "*.changed", null);


                foreach (var s in this.RoutingKeys)
                {
                    channel.QueueBind(this.QueueName, exchangeName, s, null);
                }
                channel.BasicQos(0, (ushort)this.GetParallelProcessing(), false);
              
                var consumer = new TaskBasicConsumer(channel);
                consumer.Received += async (s, e) =>
                {
                    byte[] body = e.Body;
                    var json = await this.DecompressAsync(body);
                    var header = new MessageHeaders(e);
                    var item = json.DeserializeFromJson<T>();
                    ProcessMessage(item, header)
                        .ContinueWith(_ =>
                        {
                            if (_.IsFaulted && null != _.Exception)
                            {
                                var exc = _.Exception;
                                this.WriteError(exc);
                                channel.BasicReject(e.DeliveryTag, false);
                            }
                            else
                            {
                                channel.BasicAck(e.DeliveryTag, false);
                            }
                        })
                        .Wait();

                };


                channel.BasicConsume(this.QueueName, noAck, consumer);

                var stopRequested = await Stoping();
                if (stopRequested)
                {
                    channel.Close();
                    conn.Close();
                    this.WriteMessage("!!Stoping : {0}", this.QueueName);

                }


            }
        }

        private Task<bool> Stoping()
        {
            return m_stoppingTcs.Task;
        }

        // ReSharper restore FunctionNeverReturns

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