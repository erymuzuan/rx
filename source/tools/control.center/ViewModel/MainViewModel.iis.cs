using System;
using System.Diagnostics;
using System.IO;
using Bespoke.Sph.ControlCenter.Model;
using Bespoke.Sph.ControlCenter.Properties;
using GalaSoft.MvvmLight.Command;
using EventLog = Bespoke.Sph.ControlCenter.Model.EventLog;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class MainViewModel 
    {
        private Process m_iisServiceProcess;
        public RelayCommand StartIisServiceCommand { get; set; }
        public RelayCommand StopIisServiceCommand { get; set; }

        private void SetupIisCommand()
        {
            StartIisServiceCommand = new RelayCommand(StartIisService, () => !IisServiceStarted && RabbitMqServiceStarted && !RabbitMqServiceStarting && !IsBusy);
            StopIisServiceCommand = new RelayCommand(StopIisService, () => IisServiceStarted && !IsBusy);
            
        }
        

        private void CheckIisExpress()
        {
            const string PROCESS_NAME = "iisexpress.exe";
            var web = "/site:web." + this.Settings.ApplicationName;

            var id = FindProcessByCommandLineArgs(PROCESS_NAME, web);
            if (id == 0) return;
            m_iisServiceProcess = Process.GetProcessById(id);
            this.Post(() =>
            {
                this.IsBusy = false;
                this.IisServiceStarted = true;
            });
        }
        

        private void StartIisService()
        {
            Log("IIS Service...[INITIATING]");
            this.IsBusy = true;
            this.QueueUserWorkItem(() =>
            {
                try
                {
                    var iisConfig = $@"{this.Settings.Home}\config\applicationhost.config".TranslatePath();
                    if (!File.Exists(iisConfig))
                    {
                        Log(Resources.CannotFind + iisConfig);
                        return;

                    }
                    if (!File.Exists(this.Settings.IisExpressExecutable))
                    {
                        Console.WriteLine(Resources.CannotFind + this.Settings.IisExpressExecutable);
                        return;
                    }

                    var arg = $"/config:\"{iisConfig}\" /site:web.{this.Settings.ApplicationName} /trace:error";
                    var info = new ProcessStartInfo
                    {
                        FileName = this.Settings.IisExpressExecutable.TranslatePath(),
                        Arguments = arg,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardInput = true,
                        CreateNoWindow = true,
                        RedirectStandardError = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    };
                    Log($"Looking for IIS express in {info.FileName}");
                    Log($"Starting IIS express with : {arg}");
                    if (!File.Exists(info.FileName))
                        throw new FileNotFoundException("Cannot find IIS express ", info.FileName);
                    m_iisServiceProcess = Process.Start(info);
                    if (null == m_iisServiceProcess) throw new InvalidOperationException("Cannot start IIS");

                    m_iisServiceProcess.BeginOutputReadLine();
                    m_iisServiceProcess.BeginErrorReadLine();
                    m_iisServiceProcess.OutputDataReceived += OnIisDataReceived;
                    m_iisServiceProcess.ErrorDataReceived += OnIisErrorReceived;
                    m_iisServiceProcess.Exited += (o, e) => { StopIisService(); };

                }
                catch (Exception ex)
                {
                    Console.WriteLine(Resources.ExceptionOccurred, ex.Message, ex.StackTrace);
                }




            });
        }


        private void StopIisService()
        {
            if (!m_iisServiceProcess.HasExited)
                m_iisServiceProcess.Kill();

            m_iisServiceProcess = null;
            Log("IIS Service... [STOPPED]");
            IisServiceStarted = false;
            IisServiceStatus = "Stopped";
        }




        private void OnIisDataReceived(object sender, DataReceivedEventArgs e)
        {

            var message = $"{e.Data}";
            if (message.Contains("IIS Express stopped"))
                this.StopIisService();

            if (message.Contains("IIS Express is running") || message.Contains("Successfully registered URL"))
            {
                this.Post(() =>
                {
                    this.IsBusy = false;
                    this.IisServiceStarted = true;
                    this.IisServiceStatus = "Running";
                    Log("IIS Service... [STARTED]");

                });
            }


            m_writer?.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}");

            var severity = message.Contains("HTTP status 500") ? Severity.Error : Severity.Verbose;
            var entry = new LogEntry
            {
                Severity = severity,
                Message = e.Data,
                Time = DateTime.Now,
                Log = EventLog.WebServer,
                Source = "IIS Express"
            };
            this.QueueUserWorkItem(this.Logger.Log, entry);
        }

        private void OnIisErrorReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                m_writer?.WriteLine($"![{DateTime.Now:HH:mm:ss}] {e.Data}");
            }
            this.Logger.Log(new LogEntry
            {
                Severity = Severity.Error,
                Message = e.Data,
                Time = DateTime.Now,
                Log = EventLog.WebServer,
                Source = "IIS Express"
            });

        }

    

      
    }
}
