using System;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight.Command;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class MainViewModel 
    {
        public RelayCommand StartSphWorkerCommand { get; set; }
        public RelayCommand StopSphWorkerCommand { get; set; }

        public void SetupWorkersCommand()
        {
            

            StartSphWorkerCommand = new RelayCommand(StartSphWorker, () => !SphWorkerServiceStarted && RabbitMqServiceStarted && SqlServiceStarted);
            StopSphWorkerCommand = new RelayCommand(StopSphWorker, () => SphWorkerServiceStarted && !IsBusy);

        }

        private Process m_sphWorkerProcess;

        public void CheckWorkers()
        {
            const string PROCESS_NAME = "workers.console.runner.exe";
            var web = "/v:" + this.Settings.ApplicationName;
            this.QueueUserWorkItem(() =>
            {
                var id = FindProcessByCommandLineArgs(PROCESS_NAME, web);
                if (id == 0) return;
                this.Post(() =>
                {
                    m_sphWorkerProcess = Process.GetProcessById(id);
                    this.IsBusy = false;
                    this.SphWorkerServiceStarted = true;
                });
            });

        }

        private void StartSphWorker()
        {
            this.IsBusy = true;
            Log("SPH Worker...[STARTING]");
            var f = string.Join(@"\", this.Settings.Home, "subscribers.host", "workers.console.runner.exe");

            try
            {
                var workerInfo = new ProcessStartInfo
                {
                    FileName = f,
                    Arguments = $"/log:console /v:{this.Settings.ApplicationName}",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    WindowStyle = ProcessWindowStyle.Hidden,

                };



                m_sphWorkerProcess = Process.Start(workerInfo);
                if (null == m_sphWorkerProcess) throw new InvalidOperationException("Cannot start subscriber worker");
                m_sphWorkerProcess.StandardInput.AutoFlush = true;
                m_sphWorkerProcess.BeginOutputReadLine();
                m_sphWorkerProcess.BeginErrorReadLine();
                m_sphWorkerProcess.OutputDataReceived += OnWorkerDataReceived;
                m_sphWorkerProcess.ErrorDataReceived += OnWorkerErrorReceived;
                this.SphWorkerServiceStarted = true;

            }
            catch (Exception ex)
            {
                this.IsBusy = false;
                var message = ex.Message + "\r\n" + ex.StackTrace.ToString(CultureInfo.InvariantCulture);
                Log(message);
                this.Post(() =>
                {

                    MessageBox.Show(message, "Reactive Developer", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern bool AttachConsole(uint dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool FreeConsole();

        [DllImport("kernel32.dll")]
        // ReSharper disable InconsistentNaming
        static extern bool SetConsoleCtrlHandler(ConsoleCtrlDelegate HandlerRoutine, bool Add);

        // Delegate type to be used as the Handler Routine for SCCH
        delegate Boolean ConsoleCtrlDelegate(CtrlTypes CtrlType);
        // ReSharper restore InconsistentNaming
        [DllImport("kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GenerateConsoleCtrlEvent(CtrlTypes dwCtrlEvent, uint dwProcessGroupId);
        // Enumerated type for the control messages sent to the handler routine
        enum CtrlTypes : uint
        {
            // ReSharper disable InconsistentNaming
            CTRL_C_EVENT = 0,
            // ReSharper disable UnusedMember.Local
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
            // ReSharper restore UnusedMember.Local
            // ReSharper restore InconsistentNaming
        }

        private void StopSphWorker()
        {
            this.IsBusy = true;
            this.QueueUserWorkItem(() =>
            {    //This does not require the console window to be visible.
                if (AttachConsole((uint)m_sphWorkerProcess.Id))
                {
                    Log("SPH worker .. attached");
                    //Disable Ctrl-C handling for our program
                    SetConsoleCtrlHandler(null, true);
                    Log("SPH Worker .. sending Ctrl C");
                    GenerateConsoleCtrlEvent(CtrlTypes.CTRL_C_EVENT, 0);

                    //Must wait here. If we don't and re-enable Ctrl-C handling below too fast, we might terminate ourselves.
                    m_sphWorkerProcess.WaitForExit();

                    FreeConsole();

                    //Re-enable Ctrl-C handling or any subsequently started programs will inherit the disabled state.
                    SetConsoleCtrlHandler(null, false);
                }
                Task.Delay(1000).Wait();
                if (!m_sphWorkerProcess.HasExited)
                {
                    Log("SPH Worker... [STOPPING]");
                    m_sphWorkerProcess.Kill();
                }
                Log("SPH Worker... [STOPPED]");

                this.Post(() =>
                {
                    m_sphWorkerProcess = null;
                    SphWorkerServiceStarted = false;
                    SphWorkersStatus = "Stopped";
                    this.IsBusy = false;
                });
            });
        }


        private void OnWorkerDataReceived(object sender, DataReceivedEventArgs e)
        {
            var message = $"{e.Data}";

            if (message.Contains("Welcome to [SPH] Type ctrl + c to quit at any time"))
            {
                this.IsBusy = false;
                SphWorkerServiceStarted = true;
                SphWorkersStatus = "Running";
                Log("SPH Worker... [STARTED]");
            }
            Log(message);

        }

        private void OnWorkerErrorReceived(object sender, DataReceivedEventArgs e)
        {
            var message = $"{e.Data}";
            m_writer?.WriteLine("![{0:HH:mm:ss}] {1}", DateTime.Now, message);
            if (message.Contains("Unhandled Exception"))
            {
                this.IsBusy = false;
                SphWorkerServiceStarted = false;
                SphWorkersStatus = "Error";
                this.QueueUserWorkItem(() =>
                {
                    Task.Delay(500).Wait();
                    this.Post(() =>
                    {
                        MessageBox.Show("There's an error starting your subscriber worker \r\n" + message, "Reactive Developer",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    });
                });
            }
        }
    }
}
