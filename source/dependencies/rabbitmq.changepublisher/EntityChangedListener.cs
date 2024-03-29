﻿using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Messaging;
using Bespoke.Sph.Messaging.RabbitMqMessagings;
using RabbitMQ.Client;

namespace Bespoke.Sph.RabbitMqPublisher
{
    public class EntityChangedListener<T> : IDisposable, IEntityChangedListener<T> where T : Entity
    {
        public string QueueName { get; }
        public string[] RoutingKeys { get; }
        private bool m_isRun;
        private IConnection m_connection;
        private IModel m_channel;
        private SynchronizationContext m_currentContext;

        public EntityChangedListener()
        {
            var guid = Process.GetCurrentProcess().Id.ToString(CultureInfo.InvariantCulture).PadLeft(4, '0');
            this.QueueName = $"_{Environment.MachineName}_{guid}_{typeof (T).Name}";
            this.RoutingKeys = new[] { typeof(T).Name + ".#.#" };
            m_currentContext = SynchronizationContext.Current;
        }


        public void Run()
        {
            if (m_isRun) return;
            m_isRun = true;


            const bool NO_ACK = true;
            const string EXCHANGE_NAME = "rx.topics";

            var factory = new ConnectionFactory
            {
                UserName = RabbitMqConfigurationManager.UserName,
                VirtualHost = RabbitMqConfigurationManager.VirtualHost,
                Password = RabbitMqConfigurationManager.Password,
                HostName = RabbitMqConfigurationManager.Host,
                Port = RabbitMqConfigurationManager.Port
            };
            m_connection = factory.CreateConnection();
            m_channel = m_connection.CreateModel();

            m_channel.ExchangeDeclare(EXCHANGE_NAME, ExchangeType.Topic, true);
            m_channel.QueueDeclare(this.QueueName, false, true, true, null);
            foreach (var s in this.RoutingKeys)
            {
                m_channel.QueueBind(this.QueueName, EXCHANGE_NAME, s, null);
            }

            var consumer = new TaskBasicConsumer(m_channel);
            consumer.Received += MessageReceived;

            m_channel.BasicConsume(this.QueueName, NO_ACK, consumer);

        }

        public event EntityChangedEventHandler<T> Changed;

        public void Run(SynchronizationContext synchronizationContext)
        {
            m_currentContext = synchronizationContext;
            this.Run();
        }

        private async void MessageReceived(object sender, ReceivedMessageArgs e)
        {
            var body = e.Body;
            var json = await this.DecompressAsync(body);
            var t = json.DeserializeFromJson<T>();
            var arg = new EntityChangedEventArgs<T>
            {
                Item = t,
                AuditTrail = this.GetLog(e)
            };

            if (null != this.Changed && null != t)
            {
                if (null != m_currentContext)
                    m_currentContext.Post(d => this.Changed(this, arg), arg);
                else
                    this.Changed(this, arg);// worker thread

            }

            Callback?.Invoke(arg);
        }

        public Action<object> Callback { get; set; }


        private string ByteToString(byte[] content)
        {
            using (var orginalStream = new MemoryStream(content))
            {
                using (var sr = new StreamReader(orginalStream))
                {
                    var text = sr.ReadToEnd();
                    return text;
                }
            }
        }

        private AuditTrail GetLog(ReceivedMessageArgs args)
        {
            if (args.Properties.Headers["log"] is byte[] operationBytes)
            {
                var json = ByteToString(operationBytes);
                if (string.IsNullOrWhiteSpace(json)) return null;
                return json.DeserializeFromJson<AuditTrail>();
            }

            return null;

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
