using System;
using System.Collections.Generic;
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
                this.StartConsume().Wait();
            }
            catch (Exception e)
            {
                this.WriteMessage(e.ToString());
                if (null != e.InnerException)
                    this.WriteMessage(e.InnerException.ToString());
                var aeg = e as AggregateException;
                if (null != aeg)
                {
                    foreach (var exc in aeg.InnerExceptions)
                    {
                        this.WriteMessage(exc.ToString());
                    }

                }
            }
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
                var processing = 0;
                consumer.Received += async (s, e) =>
                {
                    Interlocked.Increment(ref processing);
                    byte[] body = e.Body;
                    var json = await this.DecompressAsync(body);
                    var header = new MessageHeaders(e);
                    var item = json.DeserializeFromJson<T>();
                    try
                    {
                        await ProcessMessage(item, header);
                        channel.BasicAck(e.DeliveryTag, false);
                    }
                    catch (Exception exc)
                    {
                        this.WriteMessage("Error in {0}", this.GetType().Name);
                        this.WriteError(exc);
                        channel.BasicReject(e.DeliveryTag, false);
                    }
                    finally
                    {
                        Interlocked.Decrement(ref processing);
                    }
                };


                channel.BasicConsume(this.QueueName, noAck, consumer);

                var stopRequested = await Stoping();
                while (processing > 0)
                {
                }
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