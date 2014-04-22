using System;
using System.Configuration;
using System.ServiceProcess;
using Bespoke.Sph.SubscribersInfrastructure;

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
            m_program = new Program
            {
                HostName = ConfigurationManager.AppSettings["sph:RabbitMqHost"] ?? "localhost",
                UserName = ConfigurationManager.AppSettings["sph:RabbitMqUserName"] ?? "guest",
                Password = ConfigurationManager.AppSettings["sph:RabbitMqPassword"] ?? "guest",
                Port = int.Parse(ConfigurationManager.AppSettings["sph:RabbitMqPort"] ?? "5672"),
                VirtualHost = ConfigurationManager.AppSettings["sph:RabbitMqVhost"] ?? "Dev",
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
