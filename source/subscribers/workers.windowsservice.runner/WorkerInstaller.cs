using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace workers.windowsservice.runner
{
    [RunInstaller(true)]
    public partial class WorkerInstaller : Installer
    {
        public WorkerInstaller()
        {
            InitializeComponent();


            var serviceProcessInstaller = new ServiceProcessInstaller();
            var serviceInstaller = new ServiceInstaller();

            //# Service Account Information
            serviceProcessInstaller.Account = ServiceAccount.LocalSystem;


            //# Service Information
            serviceInstaller.DisplayName = "SPH subscribers worker";
            serviceInstaller.StartType = ServiceStartMode.Automatic;

            //# This must be identical to the WindowsService.ServiceBase name
            //# set in the constructor of WindowsService.cs
            serviceInstaller.ServiceName = "SphWorkerService";

            this.Installers.Add(serviceProcessInstaller);
            this.Installers.Add(serviceInstaller);
        }
    }
}
