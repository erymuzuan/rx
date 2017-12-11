using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Messaging;
using Bespoke.Sph.Extensions;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    public class Program
    {
        private readonly SubscriberConfig[] m_startOptions;
        public IMessageBroker MessageBroker { get; }

        public Program(params SubscriberConfig[] startOptions)
        {
            m_startOptions = startOptions;
            MessageBroker = ObjectBuilder.GetObject<IMessageBroker>();
        }
        private ILogger m_notificationService;
        private static ILogger m_notificationService2;
        public string Password { get; set; }
        public string UserName { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }

        public ConcurrentBag<Subscriber> SubscriberCollection { get; } = new ConcurrentBag<Subscriber>();


        public ILogger NotificationService
        {
            get => m_notificationService;
            set
            {
                m_notificationService = value;
                m_notificationService2 = value;
            }
        }


        public async Task Start(SubscriberMetadata[] subscribersMetadata)
        {
            this.SubscriberCollection.Clear();
            this.NotificationService.WriteInfo($"config {this.HostName}:{this.UserName}:{this.Password}");
            this.NotificationService.WriteInfo("Starts...");
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.UnhandledException += AppdomainUnhandledException;

            await this.MessageBroker.ConnectAsync();
            this.MessageBroker.ConnectionShutdown += (o, e) =>
            {
                this.Stop();
                //TODO : Get next available node if clustered??

            };

            Parallel.ForEach(subscribersMetadata, (mt, c) =>
            {
                // Get the worker service instance, for this subscriber
                var config = this.GetConfig(mt);
                if (null == config) return;

                var count = config.InstancesCount ?? 1;
                mt.Instance = count;
                mt.MaxInstances = config.MaxInstances;
                mt.MinInstances = config.MinInstances;

                for (var i = 0; i < count; i++)
                {
                    this.NotificationService.WriteInfo($"Starting... {mt.FullName}");
                    try
                    {
                        mt.PrefetchCount = (ushort)(config.PrefetchCount ?? 1);
                        var worker = StartSubscriber(mt);
                        if (null != worker)
                        {
                            this.SubscriberCollection.Add(worker);
                            this.NotificationService.WriteInfo($"Started : {mt.FullName}");
                        }
                        else
                        {
                            this.NotificationService.WriteError($"Cannot start {mt.FullName}");
                        }

                    }
                    catch (Exception e)
                    {
                        this.NotificationService.WriteError(e, $"Fail to start {mt.Name}");
                    }
                }
            });
            m_stopping = false;

            //
            await StartWorkersManager(subscribersMetadata);
        }

        private async Task StartWorkersManager(SubscriberMetadata[] mts)
        {
            var handler = new HttpClientHandler { Credentials = new NetworkCredential(ConfigurationManager.RabbitMqUserName, ConfigurationManager.RabbitMqPassword) };
            var client = new HttpClient(handler) { BaseAddress = new Uri($"{ConfigurationManager.RabbitMqManagementScheme}://{ConfigurationManager.RabbitMqHost}:{ConfigurationManager.RabbitMqManagementPort}") };

            var tasks = from mt in mts
                        select ManageQueueWorkloadAsync(client, mt);
            await Task.WhenAll(tasks);

            await Task.Delay(ConfigurationManager.ManageSubscribersWorkloadInterval);
            if (!m_stopping)
                await StartWorkersManager(mts);

        }

        private async Task ManageQueueWorkloadAsync(HttpClient client, SubscriberMetadata mt)
        {
            var response = await client.GetStringAsync($"api/queues/{ConfigurationManager.ApplicationName}/{mt.QueueName}");

            var json = JObject.Parse(response);
            var published = json.SelectToken("$.message_stats.publish_details.rate").Value<double>();
            var delivered = json.SelectToken("$.message_stats.deliver_details.rate").Value<double>();
            var length = json.SelectToken("$.messages").Value<double>();

            var subscribers = this.SubscriberCollection;
            var processing = subscribers.Sum(x => x.PrefetchCount);
            var overloaded = (published > delivered + processing) || (length > processing);

            this.NotificationService.WriteInfo(
                $"Published:{published}, Delivered : {delivered}, length : {length} , Processing {processing}");
            if (overloaded && mt.MaxInstances.HasValue && subscribers.Count < mt.MaxInstances.Value)
            {
                var sub1 = StartSubscriber(mt);
                if (null != sub1)
                    subscribers.Add(sub1);
            }

            if (!overloaded && mt.MinInstances.HasValue && subscribers.Count > mt.MinInstances.Value)
            {
                if (subscribers.TryTake(out var sub2))
                {
                    sub2.Stop();
                }
            }
            this.NotificationService.WriteInfo("Current subscribers " + subscribers.Count);
        }


        private SubscriberConfig GetConfig(SubscriberMetadata mt)
        {
            return m_startOptions.LastOrDefault(x => x.FullName == mt.FullName && Path.GetFileNameWithoutExtension(mt.Assembly) == x.Assembly);
        }

        private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var assembly = args.Name.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                    .First().Trim();
            var subs = $"{ConfigurationManager.SubscriberPath}\\{assembly}.dll";
            if (File.Exists(subs))
                return Assembly.LoadFile(subs);
            subs = $"{ConfigurationManager.CompilerOutputPath}\\{assembly}.dll";
            if (File.Exists(subs))
                return Assembly.LoadFile(subs);
            throw new Exception("Cannot load " + subs);
        }

        private Subscriber StartSubscriber(SubscriberMetadata metadata)
        {
            var dll = Path.GetFileNameWithoutExtension(metadata.Assembly);
            if (string.IsNullOrWhiteSpace(dll)) return null;
            if (!(Activator.CreateInstance(dll, metadata.FullName).Unwrap() is Subscriber subs)) return null;
            subs.NotificicationService = this.NotificationService;
            subs.PrefetchCount = metadata.PrefetchCount;
            subs.Run(this.MessageBroker);

            return subs;


        }


        private static void AppdomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var domain = (AppDomain)sender;
            var name = domain.FriendlyName;

            var message = new StringBuilder("Error in " + name);
            var exc = e.ExceptionObject as Exception;
            while (null != exc)
            {
                message.AppendFormat("{0,-25}: {1}", exc.GetType().FullName, exc.Message);
                message.AppendLine();
                message.Append(exc.StackTrace);
                message.AppendLine();
                message.AppendLine();
                exc = exc.InnerException;
            }
            m_notificationService2.WriteError(message.ToString());
        }

        private bool m_stopping;
        public void Stop()
        {
            if (m_stopping) return;

            m_stopping = true;
            this.SubscriberCollection.ForEach(s => s.Stop());
            this.SubscriberCollection.Clear();
            MessageBroker?.Dispose();
        }
    }
}
