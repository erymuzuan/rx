using System.Configuration;
using System.ServiceProcess;
using Bespoke.Sph.SubscribersInfrastructure;

namespace workers.windowsservice.runner
{
    public partial class StationMsWorkerService : ServiceBase
    {
        public StationMsWorkerService()
        {
            InitializeComponent();
        }

        private Program m_program;
        protected override void OnStart(string[] args)
        {
            var logger = new EventLogNotification();
            m_program = new Program
            {
                HostName = ConfigurationManager.AppSettings["sph:RabbitMqHost"],
                UserName = ConfigurationManager.AppSettings["sph:RabbitMqUserName"],
                Password = ConfigurationManager.AppSettings["sph:RabbitMqPassword"],
                Port = int.Parse(ConfigurationManager.AppSettings["sph:RabbitMqPort"]),
                VirtualHost = ConfigurationManager.AppSettings["sph:RabbitMqVhost"] ?? "sph.009"

            };

            SubscriberMetadata[] metadata;
            using (var discoverer = new Isolated<Discoverer>())
            {
                metadata = discoverer.Value.Find();
            }
            m_program.NotificationService = logger;
            m_program.Start(metadata);

        }

        protected async override void OnStop()
        {
            await m_program.Stop();
        }
    }
}
