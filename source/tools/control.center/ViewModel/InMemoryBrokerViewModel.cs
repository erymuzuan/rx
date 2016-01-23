using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Bespoke.Sph.ControlCenter.Model;
using Bespoke.Sph.ControlCenter.Properties;
using GalaSoft.MvvmLight.Command;
using EventLog = Bespoke.Sph.ControlCenter.Model.EventLog;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class InMemoryBrokerViewModel : IView
    {
        private Process m_elasticProcess;
        private Process m_iisServiceProcess;
        public DispatcherObject View { get; set; }
        public RelayCommand StartElasticSearchCommand { get; set; }
        public RelayCommand StopElasticSearchCommand { get; set; }
        public RelayCommand StartSqlServiceCommand { get; set; }
        public RelayCommand StopSqlServiceCommand { get; set; }
        public RelayCommand StartIisServiceCommand { get; set; }
        public RelayCommand StopIisServiceCommand { get; set; }
        public RelayCommand SaveSettingsCommand { get; set; }
        public RelayCommand ExitAppCommand { get; set; }
        public RelayCommand SetupCommand { get; set; }
        public Logger Logger { get; set; }

        public InMemoryBrokerViewModel()
        {
            StartIisServiceCommand = new RelayCommand(StartIisService, () => !IisServiceStarted  && !IsBusy);
            StopIisServiceCommand = new RelayCommand(StopIisService, () => IisServiceStarted && !IsBusy);

            StartSqlServiceCommand = new RelayCommand(StartSqlService, () => !SqlServiceStarted);
            StopSqlServiceCommand = new RelayCommand(StopSqlService, () => SqlServiceStarted);

        

            StartElasticSearchCommand = new RelayCommand(StartElasticSearch, () => !ElasticSearchServiceStarted );
            StopElasticSearchCommand = new RelayCommand(StopElasticSearch, () => ElasticSearchServiceStarted);
            

            SaveSettingsCommand = new RelayCommand(SaveSettings);
            ExitAppCommand = new RelayCommand(Exit);
            SetupCommand = new RelayCommand(Setup, () => !this.IsSetup);


        }

      


        private void Setup()
        {
            MessageBox.Show("Use powershell to run Setup-SphApp.ps1");
        }

        public async Task LoadAsync()
        {
            this.IsBusy = true;


            this.Settings = SphSettings.Load();
            if (null == this.Settings)
            {
                MessageBox.Show("Cannot load your project.json", "Reactive Developer", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            // TODO : see if the database, elasticsearch index, RabbitMq vhost etc are present
            this.IsSetup = await this.FindOutSetupAsync();


            this.Logger = new Logger
            {
                UserName = this.Settings.RabbitMqUserName,
                Password = this.Settings.RabbitMqPassword,
                Port = this.Settings.RabbitMqPort ?? 5672,
                VirtualHost = this.Settings.ApplicationName,
                Host = this.Settings.RabbitMqHost ?? "localhost"
            };

            if (string.IsNullOrEmpty(this.Settings.JavaHome))
            {
                Environment.GetEnvironmentVariable("JAVA_HOME", EnvironmentVariableTarget.Machine);
            }

          
            this.CheckIisExpress();
            this.CheckElasticsearch();
            this.CheckSqlServer();

            this.IsBusy = false;
        }



        private async Task<bool> FindOutSetupAsync()
        {
            if (string.IsNullOrWhiteSpace(this.Settings.ApplicationName)) return false;
            using (var client = new HttpClient { BaseAddress = new Uri("http://localhost:9200") })
            {
                var url = this.Settings.ApplicationName.ToLowerInvariant() + "_sys/_mapping";

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
            var connectionString = "Data Source=(localdb)\\" + this.Settings.SqlLocalDbName + ";Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False";

            using (var conn = new SqlConnection(connectionString))
            {
                try
                {
                    await conn.OpenAsync();
                    using (var cmd = new SqlCommand("SELECT COUNT(*) FROM sysdatabases WHERE [name] ='" + this.Settings.ApplicationName + "'"))
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

        private void CheckSqlServer()
        {
            try
            {
                using(var conn = new SqlConnection($"Data Source=(localdb)\\{this.Settings.SqlLocalDbName};Initial Catalog={this.Settings.ApplicationName};Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False"))
                using(var cmd = new SqlCommand("SELECT COUNT(*) FROM [Sph].[UserProfile]", conn))
                {
                    conn.Open();
                    var count = (int)cmd.ExecuteScalar();
                    if(count > 0 )
                    {

                        SqlServiceStarted = true;
                        SqlServiceStatus = "Running";
                    }
                }

            }catch(SqlException)
            {

            }
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

        private static int FindProcessByCommandLineArgs(string process, string arg)
        {
            var query = $"select ProcessId, CommandLine from Win32_Process where Name='{process}'";
            var searcher = new ManagementObjectSearcher(query);
            var retObjectCollection = searcher.Get();

            foreach (var o in retObjectCollection)
            {
                var retObject = (ManagementObject)o;
                var commandLine = $"{retObject["CommandLine"]}";
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
                    if (!File.Exists(this.Settings.IisExpressExecutable.TranslatePath()))
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

        public async void StartSqlService()
        {
            if (string.IsNullOrEmpty(this.Settings.SqlLocalDbName))
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
                    Arguments = $"start \"{this.Settings.SqlLocalDbName}\"",
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
                    Arguments = $"stop \"{this.Settings.SqlLocalDbName}\"",
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
        private int m_elasticSearchId;


        public void StartElasticSearch()
        {
            this.IsBusy = true;
            this.QueueUserWorkItem(StartElasticSearchHelper);
        }

        private async void StartElasticSearchHelper()
        {

            Log("ElasticSearch...[INITIATING]");

            var esHome = Path.GetDirectoryName(Path.GetDirectoryName(this.Settings.ElasticSearchJar));
            var version = this.Settings.ElasticSearchJar.RegexSingleValue(@"elasticsearch-(?<version>\d.\d.\d).jar", "version");
            var es = string.Join(@"\", esHome).TranslatePath();
            Log("Elasticsearch Home " + esHome);
            Log("Version :" + version);

            var arg = string.Format(@" -Xms256m -Xmx1g -Xss256k -XX:+UseParNewGC -XX:+UseConcMarkSweepGC -XX:CMSInitiatingOccupancyFraction=75 -XX:+UseCMSInitiatingOccupancyOnly -XX:+HeapDumpOnOutOfMemoryError  -Delasticsearch -Des-foreground=yes -Des.path.home=""{0}""  -cp "";{0}/lib/elasticsearch-{1}.jar;{0}/lib/*;{0}/lib/sigar/*"" ""org.elasticsearch.bootstrap.Elasticsearch""",
                es,
                version);
            var info = new ProcessStartInfo
            {
                FileName = this.Settings.JavaHome + @"\bin\java.exe",
                Arguments = arg,
                WorkingDirectory = this.Settings.Home,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
                RedirectStandardError = true,
                WindowStyle = ProcessWindowStyle.Hidden
            };
            if (!File.Exists(info.FileName))
            {
                this.Post(() =>
                {
                    this.IsBusy = false;
                    MessageBox.Show("Cannot find Java in " + this.Settings.JavaHome, "Reactive Developer",
                        MessageBoxButton.OK, MessageBoxImage.Warning);
                });
                return;
            }

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


            var connected = false;
            // verify that elasticsearch started successfully
            var uri = new Uri($"http://localhost:{this.Settings.ElasticsearchHttpPort}");
            using (var client = new HttpClient() { BaseAddress = uri })
            {
                for (var i = 0; i < 20; i++)
                {
                    try
                    {
                        var ok = client.GetStringAsync("/").Result;
                        connected = ok.Contains("200");
                        break;
                    }
                    catch
                    {
                        // ignored
                        Thread.Sleep(500);
                    }
                }
            }
            if (connected)
            {
                ElasticSearchServiceStarted = true;
                ElasticSearchStatus = "Running";
                Log("ElasticSearch... [STARTED]");
                Log("Started : " + m_elasticProcess.Id);
                m_elasticSearchId = m_elasticProcess.Id;
                this.IsSetup = await this.FindOutSetupAsync();
                IsBusy = false;

            }
            else
            {
                this.Post(() =>
                {
                    this.IsBusy = false;
                    MessageBox.Show("Cannot start your Elasticsearch", "Reactive Developer", MessageBoxButton.OK, MessageBoxImage.Warning);
                });
            }




        }

        public void CheckElasticsearch()
        {
            const string PROCESS_NAME = "java.exe";
            const string ELASTICSEARCH = "elasticsearch-";
            this.QueueUserWorkItem(() =>
            {
                var id = FindProcessByCommandLineArgs(PROCESS_NAME, ELASTICSEARCH);
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

        private void StopElasticSearch()
        {
            try
            {
                var es = Process.GetProcessById(m_elasticSearchId);
                es.Kill();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            m_elasticProcess = null;
            Log("ElasticSearch... [STOPPED]");
            ElasticSearchServiceStarted = false;
            ElasticSearchStatus = "Stopped";
        }

        private void SaveSettings()
        {
            this.Settings.Save();
            MessageBox.Show("SPH settings has been successfully saved", "SPH Control Panel");
        }

        private void Exit()
        {
            Environment.Exit(0);
        }

        private void OnElasticsearchDataReceived(object sender, DataReceivedEventArgs e)
        {
            var message = $"{e.Data}";
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
            var entry = new LogEntry
            {
                Severity = Severity.Error,
                Message = e.Data,
                Time = DateTime.Now,
                Log = EventLog.Elasticsearch,
                Source = "Elasticsearch"
            };
            this.QueueUserWorkItem(this.Logger.Log, entry);
        }

        private void OnIisDataReceived(object sender, DataReceivedEventArgs e)
        {
            var message = $"{e.Data}";
            m_writer?.WriteLine("[{0:yyyy-MM-dd HH:mm:ss}] {1}", DateTime.Now, message);

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



   
        private void OnDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
                m_writer?.WriteLine("*[{0:HH:mm:ss}] {1}", DateTime.Now, e.Data);
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
                   || this.ElasticSearchServiceStarted;

            return !running;

        }

        public void Stop()
        {

        }
    }
}
