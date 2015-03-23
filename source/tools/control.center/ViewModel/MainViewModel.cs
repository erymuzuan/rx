using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Bespoke.Sph.ControlCenter.Model;
using Bespoke.Sph.ControlCenter.Properties;
using Bespoke.Sph.Domain;
using GalaSoft.MvvmLight.Command;
using NamedPipeWrapper;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;
using EventLog = Bespoke.Sph.ControlCenter.Model.EventLog;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class MainViewModel : IView
    {
        private Process m_elasticProcess;
        private Process m_iisServiceProcess;
        public DispatcherObject View { get; set; }
        public ConsoleNotificationSubscriber ConsoleLogger { get; set; }
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
        public RelayCommand SetupCommand { get; set; }
        public Logger Logger { get; set; }

        public MainViewModel()
        {
            StartIisServiceCommand = new RelayCommand(StartIisService, () => !IisServiceStarted && RabbitMqServiceStarted && !RabbitMqServiceStarting && !IsBusy);
            StopIisServiceCommand = new RelayCommand(StopIisService, () => IisServiceStarted && !IsBusy);

            StartSqlServiceCommand = new RelayCommand(StartSqlService, () => !SqlServiceStarted);
            StopSqlServiceCommand = new RelayCommand(StopSqlService, () => SqlServiceStarted);

            StartRabbitMqCommand = new RelayCommand(StartRabbitMqService, () => !RabbitMqServiceStarted && !RabbitMqServiceStarting);
            StopRabbitMqCommand = new RelayCommand(StopRabbitMqService, () => RabbitMqServiceStarted
                && (!ElasticSearchServiceStarted
                && !SphWorkerServiceStarted
                && !IisServiceStarted));

            StartElasticSearchCommand = new RelayCommand(StartElasticSearch, () => !ElasticSearchServiceStarted && RabbitMqServiceStarted && !RabbitMqServiceStarting);
            StopElasticSearchCommand = new RelayCommand(StopElasticSearch, () => ElasticSearchServiceStarted);

            StartSphWorkerCommand = new RelayCommand(StartSphWorker, () => !SphWorkerServiceStarted && RabbitMqServiceStarted);
            StopSphWorkerCommand = new RelayCommand(StopSphWorker, () => SphWorkerServiceStarted && !IsBusy);

            SaveSettingsCommand = new RelayCommand(SaveSettings);
            ExitAppCommand = new RelayCommand(Exit);
            SetupCommand = new RelayCommand(Setup, () => !this.IsSetup);

            this.Logger = new Logger
            {
                UserName = RabbitmqUserName,
                Password = RabbitmqPassword,
                Port = this.Port,
                VirtualHost = this.ApplicationName,
                Host = "localhost"
            };

        }

        private void Setup()
        {
            MessageBox.Show("Use powershell to run Setup-SphApp.ps1");
        }

        public async Task LoadAsync()
        {
            this.IsBusy = true;
            ApplicationName = Settings.Default.ApplicationName;
            ProjectDirectory = Settings.Default.ProjectDirectory;
            var file = this.GetSettingFile();
            if (File.Exists(file))
            {
                var settings = File.ReadAllText(file)
                                   .DeserializeFromJson<SphSettings>();

                SqlLocalDbName = settings.SqlLocalDbName;
                IisExpressDirectory = settings.IisExpressDirectory;
                RabbitMqDirectory = settings.RabbitMqDirectory;
                RabbitmqUserName = settings.RabbitMqUserName;
                RabbitmqPassword = settings.RabbitMqPassword;
                JavaHome = settings.JavaHome;
                ElasticSearchHome = settings.ElasticSearchHome;
                ElasticSearchVersion = settings.ElasticSearchVersion;
                Port = settings.Port;
                // TODO : see if the database, elasticsearch index, RabbitMq vhost etc are present
                this.IsSetup = await this.FindOutSetupAsync();
            }
            else
            {
                this.IsSetup = false;
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

            this.ConsoleLogger = new ConsoleNotificationSubscriber()
            {
                HostName = RabbitMqHost ?? "localhost",
                UserName = RabbitmqUserName ?? "guest",
                Password = RabbitmqPassword ?? "guest",
                Port = RabbitMqPort ?? 5672,
                VirtualHost = ApplicationName
            };
            Log(this.ConsoleLogger.Start(this.LoggerWebSockerPort ?? 50230)
                ? "Web Console subscriber successfully started"
                : "Fail to start Web Console Logger");

            if (!string.IsNullOrWhiteSpace(this.ApplicationName) && rabbitStarted)
                this.ConsoleLogger.Listen();

            this.CheckWorkers();
            this.CheckIisExpress();
            this.CheckElasticsearch();

            this.IsBusy = false;
        }



        private async Task<bool> FindOutSetupAsync()
        {
            if (string.IsNullOrWhiteSpace(this.ApplicationName)) return false;
            using (var client = new HttpClient { BaseAddress = new Uri("http://localhost:9200") })
            {
                var url = this.ApplicationName.ToLowerInvariant() + "_sys/_mapping";

                try
                {
                    var response = await client.GetAsync(url);
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        return false;
                    var content = response.Content as StreamContent;
                    if (null == content) return false;
                    var json = await content.ReadAsStringAsync();
                    Console.WriteLine(json);
                }
                catch (HttpRequestException)
                {
                    return false;
                }
            }
            // get the rabbitmq vhost
            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(this.RabbitmqUserName, this.RabbitmqPassword)
            };
            using (var client = new HttpClient(handler) { BaseAddress = new Uri("http://localhost:15672") })
            {
                var url = "api/vhosts/" + this.ApplicationName;
                try
                {
                    var response = await client.GetAsync(url);
                    if (response.StatusCode == HttpStatusCode.NotFound)
                        return false;
                    var content = response.Content as StreamContent;
                    if (null == content) return false;
                    var json = await content.ReadAsStringAsync();
                    Console.WriteLine(json);
                }
                catch (HttpRequestException)
                {
                    return false;
                }
            }
            // get the database $
            var connectionString = "Data Source=(localdb)\\" + this.SqlLocalDbName + ";Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";

            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM sysdatabases WHERE [name] ='" + this.ApplicationName + "'"))
                    {
                        var count = await cmd.ExecuteScalarAsync();
                        return (int)count == 1;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        private void CheckIisExpress()
        {
            const string PROCESS_NAME = "iisexpress.exe";
            var web = "/site:web." + ApplicationName;

            var id = FindProcessByCommandLineArgs(PROCESS_NAME, web);
            if (id == 0) return;
            m_iisServiceProcess = Process.GetProcessById(id);
            this.Post(() =>
            {
                this.IsBusy = false;
                this.IisServiceStarted = true;
            });
        }

        private static int FindProcessByCommandLineArgs(string process, string arg)
        {
            var query = string.Format("select ProcessId, CommandLine from Win32_Process where Name='{0}'", process);
            var searcher = new ManagementObjectSearcher(query);
            var retObjectCollection = searcher.Get();

            foreach (var o in retObjectCollection)
            {
                var retObject = (ManagementObject)o;
                var commandLine = string.Format("{0}", retObject["CommandLine"]);
                var id = Convert.ToInt32(retObject["ProcessId"]);
                if (commandLine.Contains(arg))
                {
                    return id;
                }
            }
            return 0;
        }

        private void StartIisService()
        {
            Log("IIS Service...[INITIATING]");
            this.IsBusy = true;
            this.QueueUserWorkItem(() =>
            {
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

        private async void StartSqlService()
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
                this.IsSetup = await this.FindOutSetupAsync();
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
        private int m_elasticSearchId;
        private NamedPipeClient<string, string> m_namedPipeClient;

        private async void StartRabbitMqService()
        {
            this.RabbitMqServiceStarting = true;
            this.IsBusy = true;
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
                    WindowStyle = ProcessWindowStyle.Normal
                };
                m_rabbitMqServer = Process.Start(startInfo);

                if (null == m_rabbitMqServer) throw new InvalidOperationException("Cannot start RabbitMQ");
                m_rabbitMqServer.BeginOutputReadLine();
                m_rabbitMqServer.BeginErrorReadLine();
                m_rabbitMqServer.OutputDataReceived += OnDataReceived;
                m_rabbitMqServer.ErrorDataReceived += OnErrorReceived;


                this.IsSetup = await this.FindOutSetupAsync();

                this.QueueUserWorkItem(() =>
                {
                    var flag = new ManualResetEvent(false);
                    DataReceivedEventHandler started = (o, e) =>
                    {
                        if (string.Format("{0}", e.Data).Contains("Starting broker"))
                            flag.Set();
                    };
                    m_rabbitMqServer.OutputDataReceived += started;
                    flag.WaitOne(15000);
                    Task.Delay(1000).Wait();
                    this.Post(() =>
                    {
                        this.ConsoleLogger.Listen();
                        RabbitMqServiceStarted = true;
                        this.IsBusy = false;
                        this.RabbitMqServiceStarting = false;
                        RabbitMqStatus = "Started";
                        Log("RabbitMQ... [STARTED]");
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
            Log("RabbitMQ...[STOPPING]");

            var rabbitmqctl = string.Join(@"\", RabbitMqDirectory, "sbin", "rabbitmqctl.bat").TranslatePath();
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

        private async void StartElasticSearch()
        {
            Log("ElasticSearch...[INITIATING]");

            try
            {
                var elasticSearchBat = string.Join(@"\", ElasticSearchHome, "bin", "elasticsearch.bat").TranslatePath();
                var es = string.Join(@"\", ElasticSearchHome).TranslatePath();
                Console.WriteLine(elasticSearchBat);
                if (!File.Exists(elasticSearchBat))
                {
                    MessageBox.Show(Resources.CannotFind + elasticSearchBat);
                    return;
                }
                var arg = string.Format(@" -Xms256m -Xmx1g -Xss256k -XX:+UseParNewGC -XX:+UseConcMarkSweepGC -XX:CMSInitiatingOccupancyFraction=75 -XX:+UseCMSInitiatingOccupancyOnly -XX:+HeapDumpOnOutOfMemoryError  -Delasticsearch -Des-foreground=yes -Des.path.home=""{0}""  -cp "";{0}/lib/elasticsearch-{1}.jar;{0}/lib/*;{0}/lib/sigar/*"" ""org.elasticsearch.bootstrap.Elasticsearch""", es, ElasticSearchVersion);
                var info = new ProcessStartInfo
                {
                    FileName = JavaHome + @"\bin\java.exe",
                    Arguments = arg,
                    WorkingDirectory = Path.GetDirectoryName(elasticSearchBat) ?? ".",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
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
                m_elasticProcess.OutputDataReceived += OnElasticsearchDataReceived;
                m_elasticProcess.ErrorDataReceived += OnElasticsearchErroReceived;

                ElasticSearchServiceStarted = true;
                ElasticSearchStatus = "Running";
                Log("ElasticSearch... [STARTED]");
                Log("Started : " + m_elasticProcess.Id);
                m_elasticSearchId = m_elasticProcess.Id;
                this.IsSetup = await this.FindOutSetupAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine(Resources.ExceptionOccurred, ex.Message, ex.StackTrace.ToString(CultureInfo.InvariantCulture));
            }

        }

        private void CheckElasticsearch()
        {
            const string PROCESS_NAME = "java.exe";
            var web = "elasticsearch-" + this.ElasticSearchVersion;
            this.QueueUserWorkItem(() =>
            {
                var id = FindProcessByCommandLineArgs(PROCESS_NAME, web);
                if (id == 0) return;
                this.Post(() =>
                {
                    m_elasticSearchId = id;
                    m_elasticProcess = Process.GetProcessById(id);
                    this.IsBusy = false;
                    this.ElasticSearchServiceStarted = true;
                });
            });

        }
        private void CheckWorkers()
        {
            const string PROCESS_NAME = "workers.console.runner.exe";
            var web = "/v:" + this.ApplicationName;
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
        private void StopElasticSearch()
        {
            try
            {
                var es = Process.GetProcessById(m_elasticSearchId);
                es.Kill();
                m_elasticProcess = null;
                Log("ElasticSearch... [STOPPED]");
                ElasticSearchServiceStarted = false;
                ElasticSearchStatus = "Stopped";
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private void StartSphWorker()
        {
            this.IsBusy = true;
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
                m_sphWorkerProcess.OutputDataReceived += OnWorkerDataReceived;
                m_sphWorkerProcess.ErrorDataReceived += OnWorkerErrorReceived;

            }
            catch (Exception ex)
            {
                Console.WriteLine(Resources.ExceptionOccurred, ex.Message, ex.StackTrace.ToString(CultureInfo.InvariantCulture));
            }
        }

        private void StopSphWorker()
        {
            this.IsBusy = true;
            this.QueueUserWorkItem(() =>
            {
                PushMessageToWorker("stop app");
                Task.Delay(5000).Wait();
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

        private void StartWorkerNamePipeClient()
        {
            m_namedPipeClient = new NamedPipeClient<string, string>("RxDevConsole." + ApplicationName);
            m_namedPipeClient.ServerMessage += delegate (NamedPipeConnection<string, string> conn, string message)
            {
                Log(string.Format("Server says: {0}", message));
            };

            m_namedPipeClient.Start();
        }

        private void PushMessageToWorker(string msg)
        {
            m_namedPipeClient.PushMessage(msg);
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
                ElasticSearchVersion = ElasticSearchVersion,
                Port = Port
            };

            var path = this.GetSettingFile();
            File.WriteAllText(path, settings.ToJsonString(true), Encoding.UTF8);

            Settings.Default.ApplicationName = this.ApplicationName;
            Settings.Default.ProjectDirectory = this.ProjectDirectory;
            Settings.Default.Save();

            MessageBox.Show("SPH settings has been successfully saved", "SPH Control Panel");
        }

        private void Exit()
        {
            Environment.Exit(0);
        }

        private void OnElasticsearchDataReceived(object sender, DataReceivedEventArgs e)
        {
            var message = string.Format("{0}", e.Data);
            m_writer?.WriteLine("[{0:yyyy-MM-dd HH:mm:ss}] {1}", DateTime.Now, message);
            var severity = message.Contains("HTTP status 500") ? Severity.Error : Severity.Verbose;
            this.Logger.Log(new LogEntry
            {
                Severity = severity,
                Message = e.Data,
                Time = DateTime.Now,
                Log = EventLog.Elasticsearch,
                Source = "Elasticsearch"
            });

            if (message.Contains("detected_master"))
            {
                MessageBox.Show("Elasticsearch is running in a cluster, Are you sure this what you intended ?", "Rx Developer",
                    MessageBoxButton.OK, MessageBoxImage.Question);
            }
        }

        private void OnElasticsearchErroReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                m_writer?.WriteLine("[{0:yyyy-MM-dd HH:mm:ss}] {1}", DateTime.Now, e.Data);
            }
            this.Logger.Log(new LogEntry
            {
                Severity = Severity.Error,
                Message = e.Data,
                Time = DateTime.Now,
                Log = EventLog.Elasticsearch,
                Source = "Elasticsearch"
            });
        }

        private void OnIisDataReceived(object sender, DataReceivedEventArgs e)
        {
            var message = string.Format("{0}", e.Data);
            m_writer?.WriteLine("[{0:yyyy-MM-dd HH:mm:ss}] {1}", DateTime.Now, message);

            var severity = message.Contains("HTTP status 500") ? Severity.Error : Severity.Verbose;
            this.Logger.Log(new LogEntry
            {
                Severity = severity,
                Message = e.Data,
                Time = DateTime.Now,
                Log = EventLog.WebServer,
                Source = "IIS Express"
            });
            if (message.Contains("IIS Express stopped"))
                this.StopIisService();

            if (message.Contains("IIS Express is running"))
            {
                this.Post(() =>
                {
                    this.IsBusy = false;
                    this.IisServiceStarted = true;
                    this.IisServiceStatus = "Running";
                    Log("IIS Service... [STARTED]");

                });
            }
        }

        private void OnIisErrorReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                m_writer?.WriteLine("![{0:HH:mm:ss}] {1}", DateTime.Now, e.Data);
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

        private void OnWorkerDataReceived(object sender, DataReceivedEventArgs e)
        {
            var message = string.Format("{0}", e.Data);
            m_writer?.WriteLine("*[{0:HH:mm:ss}] {1}", DateTime.Now, message);

            if (message.Contains("Welcome to [SPH] Type ctrl + c to quit at any time"))
            {
                this.IsBusy = false;
                SphWorkerServiceStarted = true;
                SphWorkersStatus = "Running";
                Log("SPH Worker... [STARTED]");

                StartWorkerNamePipeClient();

            }
        }

        private void OnWorkerErrorReceived(object sender, DataReceivedEventArgs e)
        {
            var message = string.Format("{0}", e.Data);
            m_writer.WriteLine("![{0:HH:mm:ss}] {1}", DateTime.Now, message);
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
        private void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
                m_writer?.WriteLine("*[{0:HH:mm:ss}] {1}", DateTime.Now, e.Data);
        }

        private void OnErrorReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                m_writer?.WriteLine("![{0:HH:mm:ss}] {1}", DateTime.Now, e.Data);
            }
        }

        private void Log(string message)
        {
            this.Post(m =>
            {
                Console.WriteLine(@"@[{0:HH:mm:ss}] {1}", DateTime.Now, m);
            }, message);
        }

        public bool CanExit()
        {
            var running = this.IisServiceStarted
                   || this.IsBusy
                   || this.RabbitMqServiceStarting
                   || this.RabbitMqServiceStarted
                   || this.ElasticSearchServiceStarted
                   || this.SphWorkerServiceStarted;

            return !running;

        }
    }
}
