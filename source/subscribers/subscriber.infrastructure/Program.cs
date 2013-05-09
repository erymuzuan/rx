using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Bespoke.Sph.SubscribersInfrastructure
{
    public class Program
    {
        private INotificationService m_notificationService;
        private static INotificationService m_notificationService2;
        public SubscriberMetadata[] Subscribers { set; get; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string HostName { get; set; }
        public int Port { get; set; }
        public string VirtualHost { get; set; }

        public INotificationService NotificationService
        {
            get { return m_notificationService; }
            set
            {
                m_notificationService = value;
                m_notificationService2 = value;
            }
        }



        public void Start()
        {
            this.NotificationService.Write("config {0}:{1}:{2}", this.HostName, this.UserName, this.Password);
            this.NotificationService.Write("Starts...");
            var threads = new List<Thread>();
            foreach (var subscriber in Subscribers)
            {
                this.NotificationService.Write("Starts..." + subscriber.FullName);
                try
                {
                    var worker = StartAppDomain(subscriber);
                    threads.Add(worker);
                }
                catch (Exception e)
                {
                    this.NotificationService.Write(e.ToString());
                }
            }
            threads.ForEach(t => t.Join());

        }

        private Thread StartAppDomain(SubscriberMetadata startInfo)
        {
            var appdomain = this.CreateAppDomain(startInfo);
            appdomain.UnhandledException += AppdomainUnhandledException;
            var subscriber = appdomain.CreateInstanceAndUnwrap(startInfo.Assembly, startInfo.FullName);
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

            return thread;
        }

        public AppDomain CreateAppDomain(SubscriberMetadata subscriber)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var dll = subscriber.Type.Assembly.GetName().Name;
            var configurationFile = Path.Combine(baseDirectory, dll) + ".dll.config";
            var currentConfig = Path.Combine(baseDirectory, Assembly.GetEntryAssembly().GetName().Name + ".exe.config");
            var ads = new AppDomainSetup
            {
                ApplicationBase = baseDirectory,
                DisallowBindingRedirects = false,
                DisallowCodeDownload = true,
                ConfigurationFile = File.Exists(configurationFile) ? configurationFile : currentConfig
            };
            return AppDomain.CreateDomain(subscriber.FullName, null, ads);
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

        public void Stop()
        {

        }
    }
}
