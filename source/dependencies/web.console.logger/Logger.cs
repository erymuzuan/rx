using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RabbitMqPublisher;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace web.console.logger
{
    [Export(typeof(ILogger))]
    public class Logger : ILogger
    {
        public const int NON_PERSISTENT_DELIVERY_MODE = 2;
        public IBrokerConnection BrokerConnection { get; set; }

        public void Log(string operation, string message, Severity severity = Severity.Info, LogEntry entry = LogEntry.Application)
        {
            var logItem = new
            {
                severity = severity.ToString().ToLower(),
                message,
                operation,
                entry = entry.ToString()
            };
            this.SendMessage(JsonConvert.SerializeObject(logItem), Severity.Error);
        }

        public Task LogAsync(string operation, string message, Severity severity = Severity.Info, LogEntry entry = LogEntry.Application)
        {
            var log = new
            {
                severity = severity.ToString().ToLower(), message, operation,
                entry = entry.ToString()
            };
            this.SendMessage(JsonConvert.SerializeObject(log), Severity.Error);
            return Task.FromResult(0);
        }

        public async Task LogAsync(Exception exception, IReadOnlyDictionary<string, object> properties)
        {

            var message = new StringBuilder();
            var exc = exception;
            var aeg = exc as AggregateException;
            if (null != aeg)
            {
                foreach (var ie in aeg.InnerExceptions)
                {
                    await this.LogAsync(ie, properties);
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

            var error = new
            {
                severity = "error",
                stack_trace = exception.StackTrace,
                message = exception.Message
            };
            this.SendMessage(JsonConvert.SerializeObject(error), Severity.Error);

        }

        public void  Log(Exception exception, IReadOnlyDictionary<string, object> properties)
        {

            var message = new StringBuilder();
            var exc = exception;
            var aeg = exc as AggregateException;
            if (null != aeg)
            {
                foreach (var ie in aeg.InnerExceptions)
                {
                    this.Log(ie, properties);
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

            var error = new
            {
                severity = "error",
                stack_trace = exception.StackTrace,
                message = exception.Message
            };
            this.SendMessage(JsonConvert.SerializeObject(error), Severity.Error);

        }

        private void SendMessage(string json, Severity severity)
        {
            if (null == this.BrokerConnection)
            {
                this.BrokerConnection = ObjectBuilder.GetObject<IBrokerConnection>();
            }


            var factory = new ConnectionFactory
            {
                UserName = this.BrokerConnection.UserName,
                Password = this.BrokerConnection.Password,
                HostName = this.BrokerConnection.Host,
                Port = this.BrokerConnection.Port,
                VirtualHost = this.BrokerConnection.VirtualHost
            };


            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var routingKey = "logger." + severity;
                var body = Encoding.Default.GetBytes(json);

                var props = channel.CreateBasicProperties();
                props.DeliveryMode = NON_PERSISTENT_DELIVERY_MODE;
                props.ContentType = "application/json";

                channel.BasicPublish("sph.topic", routingKey, props, body);

            }


        }

    }
}