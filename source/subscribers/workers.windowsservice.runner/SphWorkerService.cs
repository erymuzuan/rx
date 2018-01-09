using System;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;
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
            ILogger logger = new EventLogNotification();
            var configName = ConfigurationManager.AppSettings["sph:WorkersConfig"];
            var env = ConfigurationManager.GetEnvironmentVariable("Environment");
            var configFile = $"{ConfigurationManager.SphSourceDirectory}\\{nameof(WorkersConfig)}\\{env}.{configName}.json";
            if (!File.Exists(configFile))
            {
                logger.WriteWarning($@"Cannot find subscribers config in '{configFile}'");
                var controller = ServiceController.GetServices(this.ServiceName).First();
                controller.Stop();
                controller.WaitForStatus(ServiceControllerStatus.Stopped);

                return;

            }
            var options = configFile.DeserializeFromJsonFile<WorkersConfig>();

            m_program = new Program(options.SubscriberConfigs.ToArray())
            {
                NotificationService = logger

            };

            try
            {
                SubscriberMetadata[] metadata;
                using (var discoverer = new Isolated<Discoverer>())
                {
                    metadata = discoverer.Value.Find();
                }
                m_program.StartAsync(metadata).Wait();
            }
            catch (Exception e)
            {
                logger.WriteError(e, "Exception starting the subscribers");
            }

        }

        protected override void OnStop()
        {
            m_program.Stop();
        }
    }
}
