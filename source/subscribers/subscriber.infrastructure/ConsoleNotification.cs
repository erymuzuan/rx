using System;
using System.Text;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RabbitMqPublisher;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    [Serializable]
    public class ConsoleNotification : INotificationService
    {
        private readonly IBrokerConnection m_connectionInfo;

        public const int PERSISTENT_DELIVERY_MODE = 2;
        public const int NON_PERSISTENT_DELIVERY_MODE = 1;

        public string Exchange { get; set; }
        public bool IsOpened { get; set; }

        public ConsoleNotification(IBrokerConnection connectionInfo)
        {
            m_connectionInfo = connectionInfo;
            this.Exchange = "sph.topic";
        }


        private void SendMessage(string json, Severity severity)
        {
            if (null == m_connectionInfo) return;


            var factory = new ConnectionFactory
            {
                UserName = m_connectionInfo.UserName,
                Password = m_connectionInfo.Password,
                HostName = m_connectionInfo.Host,
                Port = m_connectionInfo.Port,
                VirtualHost = m_connectionInfo.VirtualHost
            };


            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var routingKey = "logger." + severity;
                var body = Encoding.Default.GetBytes(json);

                var props = channel.CreateBasicProperties();
                props.DeliveryMode = NON_PERSISTENT_DELIVERY_MODE;
                props.ContentType = "application/json";

                channel.BasicPublish(this.Exchange, routingKey, props, body);

            }


        }

        public void Write(string format, params object[] args)
        {
            try
            {
                var message = new { message = string.Format(format, args), severity = "info" };
                this.SendMessage(JsonConvert.SerializeObject(message), Severity.Info);
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("========== {0} : {1,12:hh:mm:ss.ff} ===========", "Infomation ", DateTime.Now);

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine(format, args);
                Console.WriteLine();
            }
            finally
            {
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        public void WriteError(string format, params object[] args)
        {
            try
            {
                this.SendMessage(string.Format(format, args), Severity.Error);
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
            var message = new StringBuilder();
            var exc = exception;
            var aeg = exc as AggregateException;
            if (null != aeg)
            {
                foreach (var ie in aeg.InnerExceptions)
                {
                    this.WriteError(ie, "");
                }

            }
            while (null != exc)
            {
                message.AppendLine(exc.GetType().FullName);
                message.AppendLine(exc.Message);
                message.AppendLine(exc.StackTrace);
                message.AppendLine();
                message.AppendLine();
                exc = exc.InnerException;
            }
            try
            {
                var error = new
                {
                    severity = "error",
                    stack_trace = exception.StackTrace,
                    message = exception.Message
                };
                this.SendMessage(JsonConvert.SerializeObject(error), Severity.Error);
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(message.ToString());
                Console.WriteLine();
            }
            finally
            {
                Console.ResetColor();
            }
        }
    }
}
