using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Station.Domain;
using Bespoke.Station.SqlRepository;
using Bespoke.Station.Subscribers.Infrastructure;
using Bespoke.Station.SubscribersInfrastructure;
using Newtonsoft.Json;
using RabbitMQ.Client;

namespace Bespoke.Station.PersistenceConsole
{
    public static class TimeSpanExtension
    {
        public static TimeSpan FromMonth(this int value)
        {
            return (DateTime.Today.AddMonths(value) - DateTime.Today);
        }   
    }
    class Program
    {
        const bool NoAck = false;

        const string RequestQueueName = "persistence_request";
        const string RequestRoutingKey = "persistence_request";

        private static void Main()
        {
            var host        = ParseArg("h") ?? "localhost";
            var exchange    = ParseArg("e") ?? "station.ms.persistence";
            var username    = ParseArg("u") ?? "guest";
            var password    = ParseArg("p") ?? "guest";
            var vhost       = ParseArg("v") ?? "i90009000";
            var port        = ParseArgInt("port") ?? 5672;

            INotificationService log = new ConsoleNotification();
            if (ParseArg("log") != "console")
                log = new EventLogNotification();
            else
                Console.Title = vhost;

            log.Write("Connecting to {2}:{3}@{0}:{1}/{4}", host, port, username, password, vhost);
           
            var factory = new ConnectionFactory
                {
                    UserName = username,
                    Password = password,
                    HostName = host,
                    Port = port,
                    VirtualHost = vhost
                };

            using (var conn = factory.CreateConnection())
            using (var channel = conn.CreateModel())
            {
                channel.ExchangeDeclare(exchange, ExchangeType.Direct, true, false, null);
                channel.QueueDeclare(RequestQueueName, false, false, false, null);
                channel.QueueBind(RequestQueueName, exchange, RequestRoutingKey, null);

                var consumer = new TaskBasicConsumer(channel);
                consumer.Received += async (s, e) =>
                    {
                        channel.BasicAck(e.DeliveryTag, false);
                        var xml = System.Text.Encoding.UTF8.GetString(e.Body);
                        try
                        {
                            var changes = XmlSerializerService.DeserializeFromXml<ChangeSubmission>(xml);
                            var so = await Save(changes);

                            var responseJson = JsonConvert.SerializeObject(so);
                            var response = System.Text.Encoding.UTF8.GetBytes(responseJson);

                            var props = channel.CreateBasicProperties();
                            props.CorrelationId = e.Properties.CorrelationId;
                            props.DeliveryMode = 2;
                            props.ContentType = "application/json";

                            channel.BasicPublish("", e.Properties.ReplyTo, props, response);
                        }
                        catch (Exception exc)
                        {
                            log.WriteError("{0}", exc);
                        }

                    };
                channel.BasicConsume(RequestQueueName, NoAck, consumer);
                while (true)
                {
                    Thread.Sleep(3.FromMonth());
                }

            }

// ReSharper disable FunctionNeverReturns
        }
// ReSharper restore FunctionNeverReturns


        private static int? ParseArgInt(string name)
        {
            var val = ParseArg(name);
            if (string.IsNullOrWhiteSpace(val)) return null;
            int number;
            if (int.TryParse(val, out number)) return number;
            return null;
        }

        private static string ParseArg(string name)
        {
            var placeHolder = string.Format("/{0}:", name);
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith(placeHolder));
            if (null == val) return null;
            return val.Replace(placeHolder, string.Empty);
        }

        private async static Task<SubmitOperation> Save(ChangeSubmission changes)
        {
            var persistence = new SqlPersistence();
            return await persistence.SubmitChanges(changes.ChangedCollection, changes.DeletedCollection, null);

        }
    }
}
