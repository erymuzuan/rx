using System;
using System.Collections.Concurrent;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using RabbitMQ.Client;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    public class Program
    {
        private readonly SubscriberConfig[] m_startOptions;

        public Program(params SubscriberConfig[] startOptions)
        {
            m_startOptions = startOptions;
        }
        private INotificationService m_notificationService;
        private static INotificationService m_notificationService2;
        public string Password { get; set; }
        public string UserName { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }

        public ObjectCollection<Subscriber> SubscriberCollection { get; } = new ObjectCollection<Subscriber>();


        public INotificationService NotificationService
        {
            get { return m_notificationService; }
            set
            {
                m_notificationService = value;
                m_notificationService2 = value;
            }
        }


        private IConnection m_connection;
        public void Start(SubscriberMetadata[] subscribersMetadata)
        {
            this.NotificationService.Write("config {0}:{1}:{2}", this.HostName, this.UserName, this.Password);
            this.NotificationService.Write("Starts...");
            var list = new ConcurrentBag<Subscriber>();
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            AppDomain.CurrentDomain.UnhandledException += AppdomainUnhandledException;

            var factory = new ConnectionFactory
            {
                UserName = this.UserName,
                VirtualHost = this.VirtualHost,
                Password = this.Password,
                HostName = this.HostName,
                Port = this.Port
            };
            m_connection = factory.CreateConnection();

            Parallel.ForEach(subscribersMetadata, (mt, c) =>
            {
                // Get the worker service instance, for this subscriber
                var config = this.GetConfig(mt);
                if (null == config) return;

                var count = config.InstancesCount ?? 1;
                mt.Instance = count;

                for (var i = 0; i < count; i++)
                {
                    this.NotificationService.Write("Starts..." + mt.FullName);
                    try
                    {
                        mt.PrefetchCount = config.PrefetchCount ?? 1;
                        var worker = StartSubscriber(mt, m_connection);
                        if (null != worker)
                        {
                            list.Add(worker);
                        }
                        else
                        {
                            this.NotificationService.WriteError("Cannot start {0}", mt.FullName);
                        }

                    }
                    catch (Exception e)
                    {
                        this.NotificationService.WriteError(e, $"Fail to start {mt.Name}");
                    }
                }
            });
            m_stopping = false;
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



        private Subscriber StartSubscriber(SubscriberMetadata metadata, IConnection connection)
        {
            
            var dll = Path.GetFileNameWithoutExtension(metadata.Assembly);
            if (string.IsNullOrWhiteSpace(dll)) return null;
            var subs = Activator.CreateInstance(dll, metadata.FullName).Unwrap() as Subscriber;
            if (null == subs) return null;
            subs.NotificicationService = this.NotificationService;
            subs.PrefetchCount = metadata.PrefetchCount;
            subs.Run(connection);

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
            if (null != m_connection)
            {
                m_connection.Close();
                m_connection.Dispose();
                m_connection = null;
            }



        }
    }
}
