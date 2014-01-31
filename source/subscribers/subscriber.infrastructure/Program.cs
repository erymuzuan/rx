using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Humanizer;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    public class Program
    {
        private INotificationService m_notificationService;
        private static INotificationService m_notificationService2;
        public string Password { get; set; }
        public string UserName { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }
        private readonly ObjectCollection<Subscriber> m_subscriberCollection = new ObjectCollection<Subscriber>();
        private readonly ObjectCollection<AppDomain> m_appDomainCollection = new ObjectCollection<AppDomain>();
        public ObjectCollection<AppDomain> AppDomainCollection
        {
            get { return m_appDomainCollection; }
        }
        public ObjectCollection<Subscriber> SubscriberCollection
        {
            get { return m_subscriberCollection; }
        }


        public INotificationService NotificationService
        {
            get { return m_notificationService; }
            set
            {
                m_notificationService = value;
                m_notificationService2 = value;
            }
        }

        public Program()
        {

            var fsw = new FileSystemWatcher(AppDomain.CurrentDomain.BaseDirectory + @"..\subscribers")
            {
                EnableRaisingEvents = true
            };
            fsw.Changed += FswChanged;
        }



        public void Start(Subscriber[] subscribers, SubscriberMetadata[] subscribersMetadata)
        {
            this.NotificationService.Write("config {0}:{1}:{2}", this.HostName, this.UserName, this.Password);
            this.NotificationService.Write("Starts...");
            var threads = new List<Thread>();
            foreach (var metadata in subscribersMetadata)
            {
                this.NotificationService.Write("Starts..." + metadata.FullName);
                try
                {
                    var worker = StartAppDomain(metadata);
                    threads.Add(worker);
                }
                catch (Exception e)
                {
                    this.NotificationService.Write(e.ToString());
                }
            }
            foreach (var sub in subscribers)
            {
                this.NotificationService.Write("Starts..." + sub.GetType().FullName);
                try
                {
                    var worker = this.StartSubscriber(sub);
                    threads.Add(worker);
                }
                catch (Exception e)
                {
                    this.NotificationService.Write(e.ToString());
                }
            }

            threads.ForEach(t => t.Join());

        }

        async void FswChanged(object sender, FileSystemEventArgs e)
        {
            await this.Stop();
        }

        private Thread StartAppDomain(SubscriberMetadata metadata)
        {
            var appdomain = this.CreateAppDomain(metadata);
            appdomain.UnhandledException += AppdomainUnhandledException;
            var subscriber = appdomain.CreateInstanceAndUnwrap(metadata.Assembly, metadata.FullName) as Subscriber;
            var thread = new Thread(o =>
                {
                    var o1 = (Subscriber)o;
                    o1.HostName = this.HostName;
                    o1.VirtualHost = this.VirtualHost;
                    o1.UserName = this.UserName;
                    o1.Password = this.Password;
                    o1.Port = this.Port;
                    o1.NotificicationService = this.NotificationService;
                    o1.Run();
                });
            thread.Start(subscriber);

            this.AppDomainCollection.Add(appdomain);
            this.SubscriberCollection.Add(subscriber);

            return thread;
        }

        private Thread StartSubscriber(Subscriber subscriber)
        {
            var thread = new Thread(o =>
                {
                    var o1 = (Subscriber)o;
                    o1.HostName = this.HostName;
                    o1.VirtualHost = this.VirtualHost;
                    o1.UserName = this.UserName;
                    o1.Password = this.Password;
                    o1.Port = this.Port;
                    o1.NotificicationService = this.NotificationService;
                    o1.Run();
                });
            thread.Start(subscriber);
            this.SubscriberCollection.Add(subscriber);

            return thread;
        }

        public AppDomain CreateAppDomain(SubscriberMetadata metadata)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var dll = metadata.Type.Assembly.GetName().Name;
            var configurationFile = Path.Combine(baseDirectory, dll) + ".dll.config";
            var currentConfig = Path.Combine(baseDirectory, Assembly.GetEntryAssembly().GetName().Name + ".exe.config");
            var ads = new AppDomainSetup
            {
                ApplicationBase = AppDomain.CurrentDomain.BaseDirectory + @"..\subscribers",
                DisallowBindingRedirects = false,
                DisallowCodeDownload = true,
                ConfigurationFile = File.Exists(configurationFile) ? configurationFile : currentConfig,
                ShadowCopyFiles = "true",
                //ShadowCopyDirectories = "",
                CachePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"..\subscribers", "CachePath"),
                ApplicationName = metadata.FullName
            };

            return AppDomain.CreateDomain(metadata.FullName, null, ads);
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

            // TODO: restart appdomain if IsTerminating
            // StartAppDomain(subscriber);
        }

        private bool m_stopping;
        public async Task Stop()
        {
            if (m_stopping) return;

            m_stopping = true;
            this.SubscriberCollection.ForEach(s => s.Stop());
            this.NotificationService.Write("WAITING to STOP for 5 seconds");
            await Task.Delay(5.Seconds());
            foreach (var appDomain in this.AppDomainCollection)
            {
                this.NotificationService.Write("UNLOADING -> {0}", appDomain.FriendlyName);
                try
                {
                    AppDomain.Unload(appDomain);
                }
                catch (Exception e)
                {
                    this.NotificationService.WriteError(e.ToString());
                }
            }
            this.SubscriberCollection.Clear();
            this.AppDomainCollection.Clear();
            //
            this.NotificationService.Write("STARTING in 2 seconds");
            await Task.Delay(2.Seconds());
            m_stopping = false;


            var discoverer = new Discoverer();
            var metadata = discoverer.Find();
            var otherSubs = discoverer.FindSubscriber();
            this.Start(otherSubs,metadata);
        }
    }
}
