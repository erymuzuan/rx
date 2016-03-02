using GalaSoft.MvvmLight.Command;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class MainViewModel 
    {
        public RelayCommand StartWebConsoleCommand { get; set; }
        public RelayCommand StopWebConsoleCommand { get; set; }

        private void SetupWebConsoleCommand()
        {
            StartWebConsoleCommand = new RelayCommand(StartWebConsole, () => !WebConsoleStarted);
            StopWebConsoleCommand = new RelayCommand(StopWebConsole, () => WebConsoleStarted);
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
