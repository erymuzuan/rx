using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using Bespoke.SphCommercialSpaces.Domain;
using FluentDateTime;
using RabbitMQ.Client;

//using sql = Bespoke.Sph.SqlRepository;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    [Serializable]
    public abstract class Subscriber<T> : MarshalByRefObject where T : Entity
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; }
        public abstract string QueueName { get; }
        public abstract string[] RoutingKeys { get; }

        protected abstract Task ProcessMessage(T item, MessageHeaders header);

        protected void WriteError(Exception exception)
        {
            var message = new StringBuilder();
            var exc = exception;
            while (null != exc)
            {
                message.AppendLine(exc.GetType().FullName);
                message.AppendLine(exc.Message);
                message.AppendLine(exc.StackTrace);
                message.AppendLine();
                message.AppendLine();
                exc = exc.InnerException;
            }
            this.NotificicationService.WriteError("{0}", new object[] { message.ToString() });
        }

        protected void WriteMessage(object value)
        {
            this.NotificicationService.Write("{0}", new[] { value });
        }

        protected void WriteMessage(string format, params object[] args)
        {
            this.NotificicationService.Write(format, args);
        }

        public INotificationService NotificicationService { get; set; }
        public string VirtualHost { get; set; }


        public async void Run()
        {
            RegisterServices();
            PrintSubscriberInformation();

            const bool noAck = false;
            const string exchangeName = "ruang.komersial.changes";
            const string deadLetterExchange = "sph.ms-dead-letter";
            const string deadLetterQueue = "ms_dead_letter_queue";

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
                var args = new Dictionary<string, string> { { "x-dead-letter-exchange", deadLetterExchange } };
                channel.QueueDeclare(this.QueueName, true, false, false, args);
                channel.QueueDeclare(deadLetterQueue, true, false, false, args);
                channel.QueueBind(deadLetterQueue, deadLetterExchange, "#", null);
                channel.QueueBind(deadLetterQueue, deadLetterExchange, "*.added", null);
                channel.QueueBind(deadLetterQueue, deadLetterExchange, "*.changed", null);

                foreach (var s in this.RoutingKeys)
                {
                    channel.QueueBind(this.QueueName, exchangeName, s, null);
                }

                var consumer = new TaskBasicConsumer(channel);
                consumer.Received += async (s, e) =>
                    {
                        byte[] body = e.Body;
                        var xml = await this.DecompressAsync(body);

                        var header = new MessageHeaders(e);
                        var item = XmlSerializerService.DeserializeFromXml<T>(xml.Replace("utf-16", "utf-8"));
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
                while (!m_stopped)
                {
                    await Task.Delay(5.Seconds()).ConfigureAwait(false);
                }


            }
            // ReSharper disable FunctionNeverReturns
        }

        private bool m_stopped;
        public void Stop()
        {
            m_stopped = true;
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
                    var xml = await sr.ReadToEndAsync();
                    return xml;
                }
            }
        }


        private void PrintSubscriberInformation()
        {
            var message = new StringBuilder();
            message.AppendFormat("{0,-15}: {1}\r\n", "Queue Name", this.QueueName);
            message.AppendFormat("{0,-15}: {1}\r\n", "Routing Keys", string.Join(",", this.RoutingKeys));
            message.AppendFormat("{0,-15}: {1}\r\n", "Config file", Path.GetFileName(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile));
            message.AppendFormat("{0,-15}: {1}\r\n", "App domain", AppDomain.CurrentDomain.FriendlyName);
            this.WriteMessage(message.ToString());

        }


        protected virtual void RegisterServices()
        {
            var connection = new Bespoke.Sph.RabbitMqPublisher.DefaultBrokerConnection
            {
                Host = this.HostName,
                VirtualHost = this.VirtualHost,
                Username = this.UserName,
                Password = this.Password,
                Port = this.Port
            };
        }


        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
