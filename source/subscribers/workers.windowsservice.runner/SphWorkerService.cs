using System;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using Bespoke.Sph.Domain;
using Bespoke.Sph.SubscribersInfrastructure;
using INotificationService = Bespoke.Sph.SubscribersInfrastructure.INotificationService;

namespace workers.windowsservice.runner
{
    public partial class SphWorkerService : ServiceBase
    {
        public SphWorkerService()
        {
            InitializeComponent();
        }

        private Program m_program;
        protected override void OnStart(string[] args)
        {
            INotificationService logger = new EventLogNotification();
            var configName = ConfigurationManager.AppSettings["sph:WorkersConfig"];
            var env = ConfigurationManager.GetEnvironmentVariable("Environment");
            var configFile = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(WorkersConfig)}\\{env}.{configName}.json";
            if (!File.Exists(configFile))
            {
                logger.WriteError($@"Cannot find subscribers config in '{configFile}'");
                var controller = ServiceController.GetServices(this.ServiceName).First();
                controller.Stop();
                controller.WaitForStatus(ServiceControllerStatus.Stopped);

                return;

            }
            var options = configFile.DeserializeFromJsonFile<WorkersConfig>();

            m_program = new Program(options.SubscriberConfigs.ToArray())
            {
                HostName = ConfigurationManager.RabbitMqHost,
                UserName = ConfigurationManager.RabbitMqUserName,
                Password = ConfigurationManager.RabbitMqPassword,
                Port = ConfigurationManager.RabbitMqPort,
                VirtualHost = ConfigurationManager.RabbitMqVirtualHost,
                NotificationService = logger

            };

            try
            {
                SubscriberMetadata[] metadata;
                using (var discoverer = new Isolated<Discoverer>())
                {
                    metadata = discoverer.Value.Find();
                }
                m_program.Start(metadata);
            }
            catch (Exception e)
            {
                logger.Write(e.ToString());
                if (null != e.InnerException)
                    logger.Write(e.InnerException.ToString());
                var aeg = e as AggregateException;
                if (null != aeg)
                {
                    foreach (var exc in aeg.InnerExceptions)
                    {
                        logger.Write(exc.ToString());
                    }

                }
            }

        }

        protected override void OnStop()
        {
            m_program.Stop();
        }
    }
}
