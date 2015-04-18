using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
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
        public ObjectCollection<AppDomain> AppDomainCollection { get; } = new ObjectCollection<AppDomain>();

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

        private FileSystemWatcher m_fsw;


        public void Start(SubscriberMetadata[] subscribersMetadata)
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
                    if (null == worker)
                    {
                        this.NotificationService.WriteError("Cannot start {0}", metadata.FullName);
                        continue;
                    }
                    threads.Add(worker);
                }
                catch (Exception e)
                {
                    this.NotificationService.Write(e.ToString());
                }
            }
            if (null != m_fsw)
            {
                m_fsw.Changed -= FswChanged;
                m_fsw.Deleted -= FswChanged;
                m_fsw.Created -= FswChanged;
                m_fsw.Dispose();
            }

            m_fsw = new FileSystemWatcher(AppDomain.CurrentDomain.BaseDirectory + @"..\subscribers")
            {
                EnableRaisingEvents = true
            };
            m_fsw.Changed += FswChanged;
            m_fsw.Deleted += FswChanged;
            m_fsw.Created += FswChanged;

            m_stopping = false;

            threads.ForEach(t => t.Join());

        }

        void FswChanged(object sender, FileSystemEventArgs e)
        {
            if (e.Name == "CachePath")
            {
                this.NotificationService.Write("Changes in CachePath will be ignored");
                return;
            }
            this.NotificationService.Write("Detected changes in FileSystem initiating stop\r\n{0} has {1}", e.Name, e.ChangeType);
            this.Stop();
            this.NotificationService.Write("Restarting in 2 seconds");
            Thread.Sleep(2.Seconds());
            using (var work = new Isolated<Discoverer>())
            {
                var metadata = work.Value.Find();
                this.Start(metadata);
            }

            this.NotificationService.Write("Your workers has been succesfully restarted.");
        }

        private Thread StartAppDomain(SubscriberMetadata metadata)
        {
            var dll = Path.GetFileNameWithoutExtension(metadata.Assembly);
            if (string.IsNullOrWhiteSpace(dll)) return null;
            var appdomain = this.CreateAppDomain(metadata);
            appdomain.UnhandledException += AppdomainUnhandledException;
            var subscriber = appdomain.CreateInstanceAndUnwrap(dll, metadata.FullName) as Subscriber;
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

        //Assembly appdomain_AssemblyResolve(object sender, ResolveEventArgs args)
        //{
        //    var name = new AssemblyName(args.Name);
        //    var dll = Path.Combine(ConfigurationManager.WorkflowCompilerOutputPath, name + ".dll");
        //    if (File.Exists(dll))
        //        return Assembly.LoadFile(dll);
        //    return null;
        //}



        public AppDomain CreateAppDomain(SubscriberMetadata metadata)
        {
            var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var configurationFile = metadata.Assembly + ".config";
            var currentConfig = Path.Combine(baseDirectory, Assembly.GetEntryAssembly().GetName().Name + ".exe.config");
            var ads = new AppDomainSetup
            {
                ApplicationBase = ConfigurationManager.SubscriberPath,
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
        public void Stop()
        {
            if (m_stopping) return;

            m_stopping = true;
            if (null != m_fsw)
            {
                m_fsw.Changed -= FswChanged;
                m_fsw.Dispose();
            }

            this.SubscriberCollection.ForEach(s => s.Stop());

            foreach (var appDomain in this.AppDomainCollection)
            {
                this.NotificationService.Write("UNLOADING -> {0}", appDomain.FriendlyName);
                try
                {
                    AppDomain.Unload(appDomain);
                }
                catch (Exception e)
                {
                    this.NotificationService.WriteError(e,"Fail to unload " + appDomain.FriendlyName);
                }
            }
            this.SubscriberCollection.Clear();
            this.AppDomainCollection.Clear();


        }
    }
}
