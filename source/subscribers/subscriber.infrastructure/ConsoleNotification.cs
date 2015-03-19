using System;
using System.Text;
using System.Threading;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RabbitMqPublisher;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    [Serializable]
    public class ConsoleNotification : INotificationService
    {
        private readonly IBrokerConnection m_connectionInfo;
        private readonly object m_lock = new object();

        public const int PERSISTENT_DELIVERY_MODE = 2;
        public const int NON_PERSISTENT_DELIVERY_MODE = 1;

        public string Exchange { get; set; }
        public bool IsOpened { get; set; }

        public ConsoleNotification(IBrokerConnection connectionInfo)
        {
            m_connectionInfo = connectionInfo;
            this.Exchange = "sph.topic";
        }
        private static string GetJsonContent(LogEntry entry)
        {
            var setting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            setting.Converters.Add(new StringEnumConverter());
            setting.Formatting = Formatting.Indented;
            setting.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var json = JsonConvert.SerializeObject(entry, setting);
            return json;
        }

        private void SendMessage(LogEntry entry)
        {
            this.QueueUserWorkItem(() =>
            {
                entry.Log = EventLog.Subscribers;
                entry.Time = DateTime.Now;
                entry.Computer = Environment.MachineName;

                var json = GetJsonContent(entry);
                if (null == m_connectionInfo) return;
                var factory = new ConnectionFactory
                {
                    UserName = m_connectionInfo.UserName,
                    Password = m_connectionInfo.Password,
                    HostName = m_connectionInfo.Host,
                    Port = m_connectionInfo.Port,
                    VirtualHost = m_connectionInfo.VirtualHost
                };


                try
                {
                    using (var connection = factory.CreateConnection())
                    using (var channel = connection.CreateModel())
                    {
                        var routingKey = "logger." + entry.Severity;
                        var body = Encoding.Default.GetBytes(json);

                        var props = channel.CreateBasicProperties();
                        props.DeliveryMode = NON_PERSISTENT_DELIVERY_MODE;
                        props.ContentType = "application/json";

                        channel.BasicPublish(this.Exchange, routingKey, props, body);

                    }
                }
                catch
                {
                    // ignore
                }
            });


        }

        public void Write(string format, params object[] args)
        {

            try
            {
                Monitor.Enter(m_lock);
                var entry = new LogEntry
                {
                    Message = string.Format(format, args),
                    Severity = Severity.Info,
                    Time = DateTime.Now,
                    Log = EventLog.Subscribers
                };
                this.QueueUserWorkItem(SendMessage, entry);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("========== {0} : {1,12:hh:mm:ss.ff} ===========", "Infomation ", DateTime.Now);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(format, args);
                Console.WriteLine();
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.White;
                Monitor.Exit(m_lock);
            }
        }

        public void WriteError(string format, params object[] args)
        {
            try
            {
                this.SendMessage(new LogEntry
                {
                    Message = "Error",
                    Severity = Severity.Error,
                    Details = string.Format(format, args),
                    Time = DateTime.Now
                });
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(format, args);
                Console.WriteLine();
            }
            finally
            {
                Console.ResetColor();
            }
        }

        public void WriteError(Exception exception, string message2)
        {

            try
            {
                var error = new LogEntry(exception);
                this.SendMessage(error);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(exception.GetType().Name);
                Console.WriteLine(exception.Message);
                Console.WriteLine(error.Details);
                Console.WriteLine();
            }
            finally
            {
                Console.ResetColor();
            }
        }
    }
}
