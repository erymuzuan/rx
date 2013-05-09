using System.Configuration;
using System.ServiceProcess;
using Bespoke.Station.SubscribersInfrastructure;

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
            m_program.Subscribers = discoverer.Find();
            m_program.NotificationService = logger;
            m_program.Start();

        }

        protected override void OnStop()
        {
            m_program.Stop();
        }
    }
}
