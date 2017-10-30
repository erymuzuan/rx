using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;
using RabbitMQ.Client;

namespace Bespoke.Sph.RabbitMqPublisher
{
    public class MessageSlaManager : IMessageSlaManager, IDisposable
    {
        public const string DELAY_EXCHANGE = "rx.delay.exchange.messages.sla";
        public const string DELAY_QUEUE = "rx.delay.queue.messages.sla";
        public const string NOTIFICATION_EXCHANGE = "rx.notification.exchange.messages.sla";
        public const string NOTIFICATION_QUEUE = "rx.notification.queue.messages.sla";

        public ObjectCollection<MessageSlaNotificationAction> CompletedActionCollection { get; } = new ObjectCollection<MessageSlaNotificationAction>();
        public ObjectCollection<MessageSlaNotificationAction> StartedActionCollection { get; } = new ObjectCollection<MessageSlaNotificationAction>();
        public ObjectCollection<MessageSlaNotificationAction> TerminatedActionCollection { get; } = new ObjectCollection<MessageSlaNotificationAction>();
        public ObjectCollection<MessageSlaNotificationAction> NotStartedActionCollection { get; } = new ObjectCollection<MessageSlaNotificationAction>();
        public ObjectCollection<MessageSlaNotificationAction> ErrorActionCollection { get; } = new ObjectCollection<MessageSlaNotificationAction>();

        private IConnection m_connection;
        private IModel m_channel;
        public const int PERSISTENT_DELIVERY_MODE = 2;
        public Severity TraceSwitch { get; set; } = Severity.Info;

        public void Initialize()
        {
            var logger = new ConsoleLogger { TraceSwitch = this.TraceSwitch };
            var factory = new ConnectionFactory
            {
                UserName = ConfigurationManager.RabbitMqUserName,
                Password = ConfigurationManager.RabbitMqPassword,
                HostName = ConfigurationManager.RabbitMqHost,
                Port = ConfigurationManager.RabbitMqPort,
                VirtualHost = ConfigurationManager.RabbitMqVirtualHost
            };
            logger.WriteInfo($"Connecting to RabbitMq with {factory.HostName}:{factory.Port}/{factory.UserName}@{factory.Password}");
            m_connection = factory.CreateConnection();
            m_channel = m_connection.CreateModel();
            m_connection.ConnectionShutdown += ConnectionShutdown;

        }

        public async Task PublishSlaOnAcceptanceAsync(MessageSlaEvent @event)
        {
            var headers = new Dictionary<string, object>();
            var body = await CompressAsync(@event.ToJson());
            var messageId = Guid.NewGuid().ToString("N").ToUpperInvariant();
            headers.AddIfNotExist("message-id", messageId);

            var props = m_channel.CreateBasicProperties();
            props.DeliveryMode = PERSISTENT_DELIVERY_MODE;
            props.ContentType = "application/json";
            props.Headers = headers;

            props.Expiration = @event.ProcessingTimeSpanInMiliseconds.ToString(CultureInfo.InvariantCulture);
            m_channel.BasicPublish(DELAY_EXCHANGE, string.Empty, props, body);
        }

        public async Task ExecuteOnNotificationAsync(MessageTrackingStatus status, MessageSlaEvent @event)
        {
            async Task<bool> ExcuteAsync(IList<MessageSlaNotificationAction> actions)
            {
                var tasks = actions.Where(x => x.UseAsync).Select(x => x.ExecuteAsync(status,  @event));
                var results = (await Task.WhenAll(tasks)).ToList();
                results = results.Concat(actions.Where(x => !x.UseAsync).ToList().Select(x => x.Execute(status,  @event))).ToList();
                return results.All(x => x);
            }

            var ok = true;
            if ((status & MessageTrackingStatus.Completed) == MessageTrackingStatus.Completed)
            {
                ok = await ExcuteAsync(this.CompletedActionCollection);
            }

            if ((status & MessageTrackingStatus.NotStarted) == MessageTrackingStatus.NotStarted)
            {
                ok = await ExcuteAsync(this.NotStartedActionCollection);
            }
            if ((status & MessageTrackingStatus.Error) == MessageTrackingStatus.Error)
            {
                ok = await ExcuteAsync(this.ErrorActionCollection);
            }

            if ((status & MessageTrackingStatus.Terminated) == MessageTrackingStatus.Terminated)
            {
                ok = await ExcuteAsync(this.TerminatedActionCollection);
            }

            if (!ok)
            {
                var tracker = ObjectBuilder.GetObject<IMessageTracker>();
                var repos = ObjectBuilder.GetObject<ICancelledMessageRepository>();

                await Task.WhenAll(
                    repos.PutAsync(@event.MessageId, @event.Worker),
                    tracker.RegisterCancelRequestedAsync(new MessageTrackingEvent(@event))
                    );
            }
        }

        public async Task<bool> CheckMessageIsValidAndMarkReceivedAsync(string messageId, string worker)
        {
            var repos = ObjectBuilder.GetObject<ICancelledMessageRepository>();

            var cancelled = await repos.CheckMessageAsync(messageId, worker);
            if (cancelled) await repos.RemoveAsync(messageId, worker);
            return !cancelled;
        }

        private static async Task<byte[]> CompressAsync(string value)
        {
            var content = new byte[value.Length];
            var index = 0;
            foreach (var item in value)
            {
                content[index++] = (byte)item;
            }


            using (var ms = new MemoryStream())
            using (var sw = new GZipStream(ms, CompressionMode.Compress))
            {
                await sw.WriteAsync(content, 0, content.Length);
                //NOTE : DO NOT FLUSH cause bytes will go missing...
                sw.Close();

                content = ms.ToArray();
            }
            return content;
        }


        private void ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            if (sender is IConnection connection)
                connection.ConnectionShutdown -= ConnectionShutdown;
            this.Initialize();
        }

        public void Close()
        {
            m_connection?.Close();
            m_connection?.Dispose();
            m_connection = null;

            m_channel?.Close();
            m_channel?.Dispose();
            m_channel = null;
        }


        public void Dispose()
        {
            m_connection?.Dispose();
            m_channel?.Dispose();
        }

        
    }
}
