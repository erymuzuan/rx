using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Xml.Linq;
using Bespoke.Sph.ControlCenter.Helpers;
using Bespoke.Sph.ControlCenter.Model;
using Bespoke.Sph.ControlCenter.Properties;
using GalaSoft.MvvmLight.Command;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class MainViewModel
    {
        private Process m_elasticProcess;
        private Process m_iisServiceProcess;

        public RelayCommand StartElasticSearchCommand { get; set; }
        public RelayCommand StopElasticSearchCommand { get; set; }
        public RelayCommand StartRabbitMqCommand { get; set; }
        public RelayCommand StopRabbitMqCommand { get; set; }
        public RelayCommand StartSqlServiceCommand { get; set; }
        public RelayCommand StopSqlServiceCommand { get; set; }
        public RelayCommand StartIisServiceCommand { get; set; }
        public RelayCommand StopIisServiceCommand { get; set; }
        public RelayCommand StartSphWorkerCommand { get; set; }
        public RelayCommand StopSphWorkerCommand { get; set; }
        public RelayCommand SaveSettingsCommand { get; set; }
        public RelayCommand ExitAppCommand { get; set; }

        public MainViewModel()
        {
            StartIisServiceCommand = new RelayCommand(StartIisService, () => !IisServiceStarted);
            StopIisServiceCommand = new RelayCommand(StopIisService, () => IisServiceStarted);

            StartSqlServiceCommand = new RelayCommand(StartSqlService, () => !SqlServiceStarted);
            StopSqlServiceCommand = new RelayCommand(StopSqlService, () => SqlServiceStarted);

            StartRabbitMqCommand = new RelayCommand(StartRabbitMqService, () => !RabbitMqServiceStarted);
            StopRabbitMqCommand = new RelayCommand(StopRabbitMqService, () => RabbitMqServiceStarted);

            StartElasticSearchCommand = new RelayCommand(StartElasticSearch, () => !ElasticSearchServiceStarted);
            StopElasticSearchCommand = new RelayCommand(StopElasticSearch, () => ElasticSearchServiceStarted);

            StartSphWorkerCommand = new RelayCommand(StartSphWorker, () => !SphWorkerServiceStarted);
            StopSphWorkerCommand = new RelayCommand(StopSphWorker, () => SphWorkerServiceStarted);

            SaveSettingsCommand = new RelayCommand(SaveSettings);
            ExitAppCommand = new RelayCommand(Exit);

            LoadSettings();

        }

        public void LoadSettings()
        {

            ApplicationName = Settings.Default.ApplicationName;
            ProjectDirectory = Settings.Default.ProjectDirectory;
            var file = this.GetSettingFile();
            if (File.Exists(file))
            {
                var settings = File.ReadAllText(file)
                                   .Deserialize<SphSettings>();

                SqlLocalDbName = settings.SqlLocalDbName;
                IisExpressDirectory = settings.IisExpressDirectory;
                RabbitMqDirectory = settings.RabbitMqDirectory;
                RabbitmqUserName = settings.RabbitMqUserName;
                RabbitmqPassword = settings.RabbitMqPassword;
                JavaHome = settings.JavaHome;
                ElasticSearchHome = settings.ElasticSearchHome;
                if (settings.Port == 0)
                {
                    XNamespace x = "http://www.bespoke.com.my/";
                    var xml = XElement.Parse(File.ReadAllText(file));
                    var port = xml.Descendants(x + "Port").First().Value;
                    this.Port = int.Parse(port);
                }
                Port = settings.Port;
            }


            if (string.IsNullOrEmpty(JavaHome))
            {
                Environment.GetEnvironmentVariable("JAVA_HOME", EnvironmentVariableTarget.Machine);
            }

            var rabbitStarted = false;
            if (!string.IsNullOrEmpty(RabbitmqUserName) && !string.IsNullOrEmpty(RabbitmqPassword))
            {
                rabbitStarted = CheckRabbitMqHostConnection(RabbitmqUserName, RabbitmqPassword, ApplicationName);
            }
            RabbitMqServiceStarted = rabbitStarted;
            RabbitMqStatus = rabbitStarted ? "Running" : "Stopped";
        }

        private void StartIisService()
        {
            Log("IIS Service...[INITIATING]");

            try
            {
                var iisConfig = @".\config\applicationhost.config".TranslatePath();
                if (!File.Exists(iisConfig))
                {
                    Console.WriteLine(Resources.CannotFind + iisConfig);
                    return;

                }
                if (!File.Exists(IisExpressDirectory.TranslatePath()))
                {
                    Console.WriteLine(Resources.CannotFind + IisExpressDirectory);
                    return;
                }

                var arg = string.Format("/config:\"{0}\" /site:web.{1} /trace:verbose", iisConfig, ApplicationName);
                var info = new ProcessStartInfo
                {
                    FileName = IisExpressDirectory.TranslatePath(),
                    Arguments = arg,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardInput = true,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                m_iisServiceProcess = Process.Start(info);
                if (null == m_iisServiceProcess) throw new InvalidOperationException("Cannot start IIS");

                m_iisServiceProcess.BeginOutputReadLine();
                m_iisServiceProcess.BeginErrorReadLine();
                m_iisServiceProcess.OutputDataReceived += OnDataReceived;
                m_iisServiceProcess.ErrorDataReceived += OnDataReceived;

                this.IisServiceStarted = true;
                this.IisServiceStatus = "Running";
                Log("IIS Service... [STARTED]");
            }
            catch (Exception ex)
            {
                Console.WriteLine(Resources.ExceptionOccurred, ex.Message, ex.StackTrace);
            }
        }

        private void StopIisService()
        {
            m_iisServiceProcess.Kill();
            m_iisServiceProcess = null;
            Log("IIS Service... [STOPPED]");
            IisServiceStarted = false;
            IisServiceStatus = "Stopped";
        }

        private void StartSqlService()
        {
            if (string.IsNullOrEmpty(SqlLocalDbName))
            {
                MessageBox.Show("Instance name cannot be empty", "SPH Control Panel");
                return;
            }

            Log("SqlLocalDb...[STARTING]");
            try
            {

                var workerInfo = new ProcessStartInfo
                {
                    FileName = "SqlLocalDB.exe",
                    Arguments = string.Format("start \"{0}\"", SqlLocalDbName),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                using (var p = Process.Start(workerInfo))
                {
                    if (null == p) throw new InvalidOperationException("Cannot start sql");
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                    p.OutputDataReceived += OnDataReceived;
                    p.ErrorDataReceived += OnDataReceived;
                    p.WaitForExit();
                }

                SqlServiceStarted = true;
                SqlServiceStatus = "Running";
                Log("SqlLocalDb... [STARTED]");
            }
            catch (Exception ex)
            {
                Console.WriteLine(Resources.ExceptionOccurred, ex.Message, ex.StackTrace.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void StopSqlService()
        {
            Log("SqlLocalDb...[STOPPING]");
            try
            {

                var workerInfo = new ProcessStartInfo
                {
                    FileName = "SqlLocalDB.exe",
                    Arguments = string.Format("stop \"{0}\"", SqlLocalDbName),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                using (var p = Process.Start(workerInfo))
                {
                    if (null == p) throw new InvalidOperationException("Cannot start SQL");
                    p.BeginOutputReadLine();
                    p.BeginErrorReadLine();
                    p.OutputDataReceived += OnDataReceived;
                    p.ErrorDataReceived += OnDataReceived;
                    p.WaitForExit();
                }

                SqlServiceStarted = false;
                SqlServiceStatus = "Stopped";
                Log("SqlLocalDb... [STOPPED]");
            }
            catch (Exception ex)
            {
                Console.WriteLine(Resources.ExceptionOccurred, ex.Message, ex.StackTrace.ToString(CultureInfo.InvariantCulture));
            }
        }

        private bool CheckRabbitMqHostConnection(string username, string password, string host)
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

        private Process m_rabbitMqServer;
        private Process m_sphWorkerProcess;

        private void StartRabbitMqService()
        {
            Log("RabbitMQ...[STARTING]");
            try
            {
                var rabbitMqServerBat = string.Join(@"\", RabbitMqDirectory, "sbin", "rabbitmq-server.bat").TranslatePath();
                if (!File.Exists(rabbitMqServerBat))
                {
                    Console.WriteLine(Resources.CannotFind + rabbitMqServerBat);
                    return;
                }

                var startInfo = new ProcessStartInfo
                {
                    FileName = rabbitMqServerBat,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                m_rabbitMqServer = Process.Start(startInfo);

                if (null == m_rabbitMqServer) throw new InvalidOperationException("Cannot start RabbitMQ");
                m_rabbitMqServer.BeginOutputReadLine();
                m_rabbitMqServer.BeginErrorReadLine();
                m_rabbitMqServer.OutputDataReceived += OnDataReceived;
                m_rabbitMqServer.ErrorDataReceived += OnDataReceived;


                RabbitMqServiceStarted = true;
                RabbitMqStatus = "Started";
                Log("RabbitMQ... [STARTED]");
            }
            catch (Exception ex)
            {
                Console.WriteLine(Resources.ExceptionOccurred, ex.Message, ex.StackTrace);
            }
        }

        private void StopRabbitMqService()
        {
            Log("RabbitMQ...[STOPPING]");
            try
            {
                m_rabbitMqServer.Kill();
                RabbitMqServiceStarted = false;
                RabbitMqStatus = "Stopped";
                Log("RabbitMQ... [STOPPED]");
            }
            catch (Exception ex)
            {
                Console.WriteLine(Resources.ExceptionOccurred, ex.Message, ex.StackTrace.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void StartElasticSearch()
        {
            Log("ElasticSearch...[INITIATING]");

            try
            {
                var elasticSearchBat = string.Join(@"\", ElasticSearchHome, "bin", "elasticsearch.bat").TranslatePath();
                Console.WriteLine(elasticSearchBat);
                if (!File.Exists(elasticSearchBat))
                {
                    Console.WriteLine(Resources.CannotFind + elasticSearchBat);
                    return;
                }
                var info = new ProcessStartInfo
                {
                    FileName = elasticSearchBat,
                    WorkingDirectory = Path.GetDirectoryName(elasticSearchBat) ?? ".",
                    //Arguments = "/c elasticsearch.bat",
                    //Arguments = string.Format(@"/c {0}\{1}", ElasticSearchHome, "elasticsearch.bat"),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    //CreateNoWindow = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                m_elasticProcess = Process.Start(info);
                if (null == m_elasticProcess)
                {
                    Console.Error.WriteLine("Cannot start elastic search");
                    return;
                }
                m_elasticProcess.BeginOutputReadLine();
                m_elasticProcess.BeginErrorReadLine();
                m_elasticProcess.OutputDataReceived += OnDataReceived;
                m_elasticProcess.ErrorDataReceived += OnErrorReceived;

                ElasticSearchServiceStarted = true;
                ElasticSearchStatus = "Running";
                Log("ElasticSearch... [STARTED]");
            }
            catch (Exception ex)
            {
                Console.WriteLine(Resources.ExceptionOccurred, ex.Message, ex.StackTrace.ToString(CultureInfo.InvariantCulture));
            }

        }

        private void StopElasticSearch()
        {
            m_elasticProcess.CloseMainWindow();
            m_elasticProcess.Close();

            m_elasticProcess = null;
            Log("ElasticSearch... [STOPPED]");
            ElasticSearchServiceStarted = false;
            ElasticSearchStatus = "Stopped";
        }

        private void StartSphWorker()
        {
            Log("SPH Worker...[STARTING]");
            var f = string.Join(@"\", ProjectDirectory, "subscribers.host", "workers.console.runner.exe");

            try
            {
                var workerInfo = new ProcessStartInfo
                {
                    FileName = f,
                    Arguments = string.Format("/log:console /v:{0}", ApplicationName),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };


                m_sphWorkerProcess = Process.Start(workerInfo);
                if (null == m_sphWorkerProcess) throw new InvalidOperationException("Cannot start subscriber worker");
                m_sphWorkerProcess.BeginOutputReadLine();
                m_sphWorkerProcess.BeginErrorReadLine();
                m_sphWorkerProcess.OutputDataReceived += OnDataReceived;
                m_sphWorkerProcess.ErrorDataReceived += OnDataReceived;

                SphWorkerServiceStarted = true;
                SphWorkersStatus = "Running";
                Log("SPH Worker... [STARTED]");
            }
            catch (Exception ex)
            {
                Console.WriteLine(Resources.ExceptionOccurred, ex.Message, ex.StackTrace.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void StopSphWorker()
        {
            Log("SPH Worker... [STOPPING]");
            // TODO : send control +  C, or call stop
            if (null != m_sphWorkerProcess)
                m_sphWorkerProcess.Kill();

            Log("SPH Worker... [STOPPED]");
            SphWorkerServiceStarted = false;
            SphWorkersStatus = "Stopped";
        }

        private string GetSettingFile()
        {
            return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "sph." + ApplicationName + ".settings.xml");

        }
        private void SaveSettings()
        {
            var settings = new SphSettings
            {
                SqlLocalDbName = SqlLocalDbName,
                IisExpressDirectory = IisExpressDirectory,
                RabbitMqDirectory = RabbitMqDirectory,
                RabbitMqUserName = RabbitmqUserName,
                RabbitMqPassword = RabbitmqPassword,
                JavaHome = JavaHome,
                ElasticSearchHome = ElasticSearchHome,
                Port = Port
            };

            var path = this.GetSettingFile();
            File.WriteAllText(path, settings.ToXmlString(), Encoding.UTF8);

            Settings.Default.ApplicationName = this.ApplicationName;
            Settings.Default.ProjectDirectory = this.ProjectDirectory;
            Settings.Default.Save();

            MessageBox.Show("SPH settings has been successfully saved", "SPH Control Panel");
        }

        private void Exit()
        {
            Environment.Exit(0);
        }

        private void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            if ((e.Data != null) && (m_writer != null))
                m_writer.WriteLine("[{0:yyyy-MM-dd HH:mm:ss}] {1}", DateTime.Now, e.Data);
        }

        private void OnErrorReceived(object sender, DataReceivedEventArgs e)
        {
            if ((e.Data != null) && (m_writer != null))
            {
                m_writer.WriteLine("[{0:yyyy-MM-dd HH:mm:ss}] {1}", DateTime.Now, e.Data);
            }
        }

        private void Log(string message)
        {
            Console.WriteLine(@"[{0:yyyy-MM-dd HH:mm:ss}] {1}", DateTime.Now, message);
        }
    }
}
