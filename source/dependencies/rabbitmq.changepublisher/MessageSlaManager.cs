using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
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

        public async Task PublishSlaOnAcceptanceAsync(MessageSlaEvent @event)
        {
            if (!this.IsOpened)
                InitConnection();

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
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(x => x.Name == @event.Entity);
            var sqlAssembly = Assembly.Load("sql.repository");
            var sqlRepositoryType = sqlAssembly.GetType("Bespoke.Sph.SqlRepository.SqlRepository`1");

            var edAssembly = Assembly.Load($"{ConfigurationManager.ApplicationName}.{ed.Name}");
            var edTypeName = $"{ed.CodeNamespace}.{ed.Name}";
            var edType = edAssembly.GetType(edTypeName);
            if (null == edType)
                Console.WriteLine(@"Cannot create type " + edTypeName);

            var reposType = sqlRepositoryType.MakeGenericType(edType);
            dynamic repository = Activator.CreateInstance(reposType);
            Entity item = await repository.LoadOneAsync(@event.ItemId);

            if ((status & MessageTrackingStatus.Completed) == MessageTrackingStatus.Completed)
            {
                var tasks = this.CompletedActionCollection.Where(x => x.UseAsync).Select(x => x.ExecuteAsync(status, item, @event));
                await Task.WhenAll(tasks);
                this.CompletedActionCollection.Where(x => !x.UseAsync).ToList().ForEach(x => x.Execute(status, item, @event));
                return; // done
            }


            if ((status & MessageTrackingStatus.NotStarted) == MessageTrackingStatus.NotStarted)
            {
                var tasks = this.NotStartedActionCollection.Where(x => x.UseAsync).Select(x => x.ExecuteAsync(status, item, @event));
                await Task.WhenAll(tasks);
                this.NotStartedActionCollection.Where(x => !x.UseAsync).ToList().ForEach(x => x.Execute(status, item, @event));
                return; // Not started
            }
            if ((status & MessageTrackingStatus.Error) == MessageTrackingStatus.Error)
            {
                var tasks = this.ErrorActionCollection.Where(x => x.UseAsync).Select(x => x.ExecuteAsync(status, item, @event));
                await Task.WhenAll(tasks);
                this.ErrorActionCollection.Where(x => !x.UseAsync).ToList().ForEach(x => x.Execute(status, item, @event));
                return; // Error
            }

            if ((status & MessageTrackingStatus.Terminated) == MessageTrackingStatus.Terminated)
            {
                var tasks = this.TerminatedActionCollection.Where(x => x.UseAsync).Select(x => x.ExecuteAsync(status, item, @event));
                await Task.WhenAll(tasks);
                this.TerminatedActionCollection.Where(x => !x.UseAsync).ToList().ForEach(x => x.Execute(status, item, @event));
            }
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

        private void InitConnection()
        {
            var factory = new ConnectionFactory
            {
                UserName = ConfigurationManager.RabbitMqUserName,
                Password = ConfigurationManager.RabbitMqPassword,
                HostName = ConfigurationManager.RabbitMqHost,
                Port = ConfigurationManager.RabbitMqPort,
                VirtualHost = ConfigurationManager.RabbitMqVirtualHost
            };
            m_connection = factory.CreateConnection();
            m_channel = m_connection.CreateModel();

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

        private bool IsOpened
        {
            get
            {
                if (null == m_connection) return false;
                if (null == m_channel) return false;
                if (null == m_connection) return false;
                if (!m_channel.IsOpen) return false;
                if (!m_connection.IsOpen) return false;

                return true;
            }
        }
        public void Dispose()
        {
            m_connection?.Dispose();
            m_channel?.Dispose();
        }
    }
}
