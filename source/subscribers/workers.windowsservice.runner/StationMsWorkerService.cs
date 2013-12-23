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
                HostName = ConfigurationManager.AppSettings["RabbitMqHost"],
                UserName = ConfigurationManager.AppSettings["RabbitMqUsername"],
                Password = ConfigurationManager.AppSettings["RabbitMqPassword"],
                Port = int.Parse(ConfigurationManager.AppSettings["RabbitMqPort"])

            };

            var discoverer = new Discoverer();
            m_program.SubscribersMetadata = discoverer.Find();
            m_program.NotificationService = logger;
            m_program.Start();

        }

        protected async override void OnStop()
        {
            await m_program.Stop();
        }
    }
}
