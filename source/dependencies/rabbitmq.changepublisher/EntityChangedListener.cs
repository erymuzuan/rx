using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.SphCommercialSpaces.Domain;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Bespoke.Sph.RabbitMqPublisher
{
    public class EntityChangedListener<T> : IDisposable, IEntityChangedListener<T> where T : Entity
    {
        public string QueueName { get; private set; }
        public string[] RoutingKeys { get; private set; }
        private bool m_isRun;
        private IConnection m_connection;
        private IModel m_channel;
        private SynchronizationContext m_currentContext;
        private readonly IBrokerConnection m_brokerConnection;

        public EntityChangedListener(IBrokerConnection connection)
        {
            m_brokerConnection = connection;
            var guid = Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture).PadLeft(4,'0');
            this.QueueName = string.Format("_{0}_{1}_{2}", Environment.MachineName, guid, typeof(T).Name);
            this.RoutingKeys = new[] { typeof(T).Name + ".*" };
            m_currentContext = SynchronizationContext.Current;
        }


        public void Run()
        {
            if (m_isRun) return;
            m_isRun = true;


            const bool noAck = true;
            const string exchangeName = "station.ms.changes";

            var factory = new ConnectionFactory
            {
                UserName = m_brokerConnection.Username,
                VirtualHost = m_brokerConnection.VirtualHost,
                Password = m_brokerConnection.Password,
                HostName = m_brokerConnection.Host,
                Port = m_brokerConnection.Port
            };
            m_connection = factory.CreateConnection();
            m_channel = m_connection.CreateModel();

            m_channel.ExchangeDeclare(exchangeName, ExchangeType.Topic, true);
            m_channel.QueueDeclare(this.QueueName, false, true, true, null);
            foreach (var s in this.RoutingKeys)
            {
                m_channel.QueueBind(this.QueueName, exchangeName, s, null);
            }

            var consumer = new TaskBasicConsumer(m_channel);
            consumer.Received += MessageReceived;

            m_channel.BasicConsume(this.QueueName, noAck, consumer);

        }

        public event EventHandler<T> Changed;

        public void Run(SynchronizationContext synchronizationContext)
        {
            m_currentContext = synchronizationContext;
            this.Run();
        }

        private async void MessageReceived(object sender, ReceivedMessageArgs e)
        {
            var body = e.Body;
            var json = await this.DecompressAsync(body);
            var t = await JsonConvert.DeserializeObjectAsync<T>(json);

            if (null != this.Changed && null != t)
            {
                if (null != m_currentContext)
                    m_currentContext.Post(d => this.Changed(this, t), t);
                else
                    this.Changed(this, t);// worker thread

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
                    var json = await sr.ReadToEndAsync();
                    return json;
                }
            }
        }

        public void Dispose()
        {
            this.Stop();
        }

        public void Stop()
        {
            if (null != m_connection)
            {
                m_connection.Close();
                m_connection.Dispose();
                m_connection = null;
            }

            if (null == m_channel) return;
            m_channel.Close();
            m_channel.Dispose();
            m_channel = null;
        }

    }
}
