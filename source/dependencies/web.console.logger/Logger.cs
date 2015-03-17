using System.ComponentModel.Composition;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.RabbitMqPublisher;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;

namespace web.console.logger
{
    [Export(typeof(ILogger))]
    public class Logger : ILogger
    {
        public const int NON_PERSISTENT_DELIVERY_MODE = 2;
        public IBrokerConnection BrokerConnection { get; set; }
        public Severity TraceSwitch { get; set; }


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

        public Task LogAsync(LogEntry entry)
        {
            this.Log(entry);
            return Task.FromResult(0);
        }



        public void Log(LogEntry entry)
        {
            if ((int)entry.Severity < (int)this.TraceSwitch)
                return;

            var json = GetJsonContent(entry);
            this.SendMessage(json, entry.Severity);
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