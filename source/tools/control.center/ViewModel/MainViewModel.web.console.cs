using System.Diagnostics;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class MainViewModel
    {
        public RelayCommand StartWebConsoleCommand { get; set; }
        public RelayCommand StopWebConsoleCommand { get; set; }
        public RelayCommand DeployOutputCommand { get; set; }

        private void SetupWebConsoleCommand()
        {
            StartWebConsoleCommand = new RelayCommand(StartWebConsole, () => !WebConsoleStarted);
            StopWebConsoleCommand = new RelayCommand(StopWebConsole, () => WebConsoleStarted);
            DeployOutputCommand = new RelayCommand(DeployOutput, () => WebConsoleServer.Default.CreatedFileCollection.Count > 1);
            WebConsoleServer.Default.CreatedFileCollection.CollectionChanged += delegate
            {
                DeployOutputCommand.RaiseCanExecuteChanged();
            };
        }

        private async void DeployOutput()
        {
            try
            {
                this.IsBusy = true;
                await DeployOutputHelper();
            }
            finally
            {
                this.IsBusy = false;
            }
        }
        private async Task DeployOutputHelper()
        {     // stop the workers
            this.Log("Stoping worker and give it a 2500 ms break");
            try
            {
                this.StopSphWorker();
                await Task.Delay(2500);
            }
            catch
            {
                //ignore
            }
            Debug.Assert(this.SphWorkerServiceStarted == false, "Worker should be stopped");
            foreach (var f in WebConsoleServer.Default.CreatedFileCollection)
            {
                this.Log($"Deploying {f} ...");
                WebConsoleServer.Default.DeployOutput(f);
            }
            WebConsoleServer.Default.CreatedFileCollection.Clear();
            // restart the workers
            this.Log("Done...");
            while (!this.StartSphWorkerCommand.CanExecute(null))
            {
                await Task.Delay(500);
            }
            this.Log("Stating worker");
            this.StartSphWorkerCommand.Execute(null);
        }


        private void StopWebConsole()
        {
            WebConsoleServer.Default.Stop();
            this.WebConsoleStarted = false;
        }

        private void StartWebConsole()
        {
            this.QueueUserWorkItem(() =>
            {
                var port = (this.Settings.LoggerWebSocketPort - 1) ?? 50237;
                var loggerStarted = false;
                while (!loggerStarted)
                {
                    port++;
                    Log($"Trying to start web socket console on port {port}");
                    loggerStarted = WebConsoleServer.Default.Start(this, port);
                }
                this.WebConsoleStarted = true;
                this.Settings.LoggerWebSocketPort = port;
                var message = "Web console subscriber successfully started on port " + port;
                Log(message);
                if (RabbitMqServiceStarted)
                {
                    WebConsoleServer.Default.StartConsume(this);
                }

                this.Post(() =>
                {
                    StartWebConsoleCommand.RaiseCanExecuteChanged();
                });
            });
        }

    }
}
