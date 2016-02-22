using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.ControlCenter.Properties;
using GalaSoft.MvvmLight.Command;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class MainViewModel 
    {
        private Process m_rabbitMqServer;
        public RelayCommand StartRabbitMqCommand { get; set; }
        public RelayCommand StopRabbitMqCommand { get; set; }

        public void SetupRabbitMqCommand()
        {

            StartRabbitMqCommand = new RelayCommand(StartRabbitMqService, () => !RabbitMqServiceStarted && !RabbitMqServiceStarting);
            StopRabbitMqCommand = new RelayCommand(StopRabbitMqService, () => RabbitMqServiceStarted
                && (!ElasticSearchServiceStarted
                && !SphWorkerServiceStarted
                && !IisServiceStarted));

          


        }

        

        public bool CheckRabbitMqHostConnection(string username, string password, string host)
        {
            var isOpen = false;
            try
            {

                var factory = new ConnectionFactory
                {
                    UserName = username ?? "guest",
                    Password = password ?? "guest",
                    VirtualHost = host ?? "/"
                };
                using (var conn = factory.CreateConnection())
                {
                    isOpen = conn.IsOpen;
                    conn.Close();
                }
                return isOpen;
            }
            catch (BrokerUnreachableException ex)
            {
                Log("Cannot connect to RabbitMQ host" + "\r\n\r\n" + ex.GetBaseException().Message);

            }
            return isOpen;
        }


        public async void StartRabbitMqService()
        {
            this.RabbitMqServiceStarting = true;
            this.IsBusy = true;
            Log("RabbitMQ...[STARTING]");
            try
            {
                var rabbitMqServerBat = string.Join(@"\", this.Settings.RabbitMqDirectory, "sbin", "rabbitmq-server.bat").TranslatePath();
                if (!File.Exists(rabbitMqServerBat))
                {
                    Console.WriteLine(Resources.CannotFind + rabbitMqServerBat);
                    this.IsBusy = false;
                    this.RabbitMqServiceStarting = false;
                    return;
                }

                var startInfo = new ProcessStartInfo
                {
                    FileName = rabbitMqServerBat,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Normal,

                };
                var rabbitMqBase = this.Settings?.RabbitMqBase?.Trim();
                if (!string.IsNullOrWhiteSpace(rabbitMqBase))
                {
                    if (!Directory.Exists(rabbitMqBase))
                        Directory.CreateDirectory(rabbitMqBase);

                    if (!startInfo.EnvironmentVariables.ContainsKey("RABBITMQ_BASE"))
                        startInfo.EnvironmentVariables.Add("RABBITMQ_BASE", rabbitMqBase);
                }

                m_rabbitMqServer = Process.Start(startInfo);

                if (null == m_rabbitMqServer) throw new InvalidOperationException("Cannot start RabbitMQ");
                m_rabbitMqServer.BeginOutputReadLine();
                m_rabbitMqServer.BeginErrorReadLine();
                m_rabbitMqServer.OutputDataReceived += OnDataReceived;
                m_rabbitMqServer.ErrorDataReceived += OnErrorReceived;


                this.IsSetup = await this.FindOutSetupAsync();

                this.QueueUserWorkItem(() =>
                {

                    var tcs = new TaskCompletionSource<bool>();
                    DataReceivedEventHandler started = (o, e) =>
                    {
                        if ($"{e.Data}".Contains("Starting broker"))
                            tcs.SetResult(true);
                        if ($"{e.Data}".Contains("ERROR"))
                        {
                            tcs.SetResult(false);
                        }
                        if ($"{e.Data}".Contains("erl_crash.dump"))
                        {
                            tcs.SetResult(false);
                        }

                    };
                    m_rabbitMqServer.OutputDataReceived += started;
                    m_rabbitMqServer.ErrorDataReceived += started;
                    tcs.Task.ContinueWith(_ =>
                    {
                        this.Post(() =>
                        {
                            RabbitMqServiceStarted = _.Result;
                            this.IsBusy = false;
                            this.RabbitMqServiceStarting = false;
                            RabbitMqStatus = _.Result ? "Started" : "Error";
                            if (_.Result)
                            {
                                Log("RabbitMQ... [STARTED]");
                                WebConsoleServer.Default.StartConsume(this);
                            }
                            else
                            {
                                Log("!Error starting RabbitMq, please check the log in the console", "Error");
                            }
                        });

                    });
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(Resources.ExceptionOccurred, ex.Message, ex.StackTrace);
            }
        }

        private void StopRabbitMqService()
        {
            Log("[STOPPING] web.console.logger");
            WebConsoleServer.Default.StopConsume();
            Log("RabbitMQ...[STOPPING]");

            var rabbitmqctl = string.Join(@"\", this.Settings.RabbitMqDirectory, "sbin", "rabbitmqctl.bat").TranslatePath();
            var startInfo = new ProcessStartInfo
            {
                FileName = rabbitmqctl,
                Arguments = "stop",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Normal
            };
            using (var stop = Process.Start(startInfo))
            {
                stop?.WaitForExit();
            }
            m_rabbitMqServer?.Dispose();

            RabbitMqServiceStarted = false;
            RabbitMqStatus = "Stopped";
            Log("RabbitMQ... [STOPPED]");

        }

  

        
    }
}
