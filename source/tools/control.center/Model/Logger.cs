using System;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Polly;
using Polly.CircuitBreaker;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Bespoke.Sph.ControlCenter.Model
{

    public class Logger
    {
        private readonly CircuitBreakerPolicy m_circuit;
        public const int NON_PERSISTENT_DELIVERY_MODE = 2;
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }

        public Severity TraceSwitch { get; set; }

        public Logger()
        {

            m_circuit = Policy.Handle<BrokerUnreachableException>()
                 .CircuitBreaker(3, TimeSpan.FromSeconds(5));
        }

        private void SendMessage(string json, Severity severity)
        {
            var factory = new ConnectionFactory
            {
                UserName = this.UserName ?? "guest",
                Password = this.Password ?? "guest",
                HostName = this.Host ?? "localhost",
                Port = this.Port == 0 ? 5672 : this.Port,
                VirtualHost = this.VirtualHost ?? "DevV1"
            };
            Action send = () =>
            {
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
            };

            try
            {
                Policy.Handle<BrokerUnreachableException>()
                    .WaitAndRetry(3, c => TimeSpan.FromMilliseconds(c * 500))
                    .Execute(send);
            }
            catch
            {
                //ignore
            }
        }

        public Task LogAsync(LogEntry entry)
        {
            this.Log(entry);
            return Task.FromResult(0);
        }

        public void Log(LogEntry entry)
        {
            try
            {
                if ((int)entry.Severity < (int)this.TraceSwitch)
                    return;
                var json = GetJsonContent(entry);
                this.QueueUserWorkItem(SendMessage, json, entry.Severity);
            }
            catch
            {
                // ignored
            }
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
    }
}