using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using RabbitMQ.Client;

namespace Bespoke.Sph.ControlCenter.Model
{

    public class Logger
    {
        public const int NON_PERSISTENT_DELIVERY_MODE = 2;
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }

        public Severity TraceSwitch { get; set; }


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
            try
            {
                if ((int)entry.Severity < (int)this.TraceSwitch)
                    return;
                var json = GetJsonContent(entry);
                this.SendMessage(json, entry.Severity);
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