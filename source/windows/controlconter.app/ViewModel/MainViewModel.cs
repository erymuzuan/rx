using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using Bespoke.Sph.ControlCenter.Helpers;
using Bespoke.Sph.ControlCenter.Model;
using GalaSoft.MvvmLight.Command;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class MainViewModel
    {
        private string m_settingsFile;
        private Process m_elasticProcess;

        public RelayCommand StartElasticSearchCommand { get; set; }
        public RelayCommand StopElasticSearchCommand { get; set; }
        public RelayCommand StartRabbitMqCommand { get; set; }
        public RelayCommand StopRabbitMqCommand { get; set; }
        public RelayCommand StartSqlServiceCommand { get; set; }
        public RelayCommand StopSqlServiceCommand { get; set; }
        public RelayCommand StartIisServiceCommand { get; set; }
        public RelayCommand StopIisServiceCommand { get; set; }
        public RelayCommand SaveSettingsCommand { get; set; }
        public RelayCommand ExitAppCommand { get; set; }

        public MainViewModel()
        {
            LoadSettings();
            
            StartIisServiceCommand = new RelayCommand(StartIisService, () => !IisServiceStarted);
            StopIisServiceCommand = new RelayCommand(StopIisService, () => IisServiceStarted);

            StartSqlServiceCommand = new RelayCommand(StartSqlService, () => !SqlServiceStarted);
            StopSqlServiceCommand = new RelayCommand(StopSqlService, () => SqlServiceStarted);

            StartRabbitMqCommand = new RelayCommand(StartRabbitMqService, () => !RabbitMqServiceStarted);
            StopRabbitMqCommand = new RelayCommand(StopRabbitMqService, () => RabbitMqServiceStarted);

            StartElasticSearchCommand = new RelayCommand(StartElasticSearch, () => !ElasticSearchServiceStarted );
            StopElasticSearchCommand = new RelayCommand(StopElasticSearch, () => ElasticSearchServiceStarted);

            SaveSettingsCommand = new RelayCommand(SaveSettings);
            ExitAppCommand = new RelayCommand(Exit);
        }

        private void LoadSettings()
        {
            m_settingsFile = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "sph.settings.xml");
            if (File.Exists(m_settingsFile))
            {
                var settings = File.ReadAllText(m_settingsFile)
                                   .Deserialize<SphSettings>();

                IisExpressDirectory = settings.IisExpressDirectory;
                WebProjectDirectory = settings.SphDirectory;
                RabbitMqDirectory = settings.RabbitMqDirectory;
                JavaHome = settings.JavaHome;
                ElasticSearchHome = settings.ElasticSearchHome;

            }

            if (string.IsNullOrEmpty(JavaHome))
            {
                Environment.GetEnvironmentVariable("JAVA_HOME", EnvironmentVariableTarget.Machine);
            }

            //IisExpressDirectory = @"C:\Program Files (x86)\IIS Express\iisexpress.exe";
            //WebProjectDirectory = @"D:\jlm";
            //RabbitMqDirectory = @"D:\syazwan.app\rabbitmq";
            //JavaHome = Environment.GetEnvironmentVariable("JAVA_HOME", EnvironmentVariableTarget.Machine);
            //ElasticSearchHome = @"C:\elasticsearch-0.90.11";

            Console.WriteLine(@"[Settings loaded]");
        }

        private void StartIisService()
        {
            IisServiceStarted = true;
        }

        private void StopIisService()
        {
            IisServiceStarted = false;
        }

        private void StartSqlService()
        {
            SqlServiceStarted = true;
        }

        private void StopSqlService()
        {
            SqlServiceStarted = false;
        }

        private void StartRabbitMqService()
        {
            RabbitMqServiceStarted = true;
        }

        private void StopRabbitMqService()
        {
            RabbitMqServiceStarted = false;
        }

        private void StartElasticSearch()
        {
            Console.WriteLine(@"ElasticSearch...[INITIATING]");

            try
            {
                var info = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    WorkingDirectory = ElasticSearchHome,
                    Arguments = "/c elasticsearch.bat",
                    //Arguments = string.Format(@"/c {0}\{1}", ElasticSearchHome, "elasticsearch.bat"),
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    //CreateNoWindow = false,
                    RedirectStandardError = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                };
                m_elasticProcess = Process.Start(info);
                m_elasticProcess.BeginOutputReadLine();
                m_elasticProcess.BeginErrorReadLine();
                m_elasticProcess.OutputDataReceived += OnDataReceived;
                m_elasticProcess.ErrorDataReceived += OnErrorReceived;
                
                ElasticSearchServiceStarted = true;
                ElasticSearchStatus = "Running";
                Console.WriteLine(@"ElasticSearch... [STARTED]");
            }
            catch (Exception ex)
            {
                Console.WriteLine(@"Exception Occurred :{0},{1}", ex.Message, ex.StackTrace.ToString(CultureInfo.InvariantCulture));
            }
            
        }

        private void StopElasticSearch()
        {
            m_elasticProcess.CloseMainWindow();
            m_elasticProcess.Close();
            m_elasticProcess = null;
            Console.WriteLine(@"ElasticSearch... [STOPPED]");
            ElasticSearchServiceStarted = false;
            ElasticSearchStatus = "Stopped";
        }

        private void SaveSettings()
        {
            var settings = new SphSettings
            {
                IisExpressDirectory = IisExpressDirectory,
                SphDirectory = WebProjectDirectory,
                RabbitMqDirectory = RabbitMqDirectory,
                JavaHome = JavaHome,
                ElasticSearchHome = ElasticSearchHome
            };

            var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "sph.settings.xml");
            File.WriteAllText(path, settings.ToXmlString(), Encoding.UTF8);

            MessageBox.Show("SPH settings has been successfully saved", "SPH Control Panel");
        }

        private void Exit()
        {
            Application.Current.Shutdown();
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
    }
}
