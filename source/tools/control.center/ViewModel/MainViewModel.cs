using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using Bespoke.Sph.ControlCenter.Model;
using GalaSoft.MvvmLight.Command;
using Newtonsoft.Json;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class MainViewModel : IView
    {
        public DispatcherObject View { get; set; }
        public RelayCommand SaveSettingsCommand { get; set; }
        public RelayCommand ExitAppCommand { get; set; }
        public RelayCommand SetupCommand { get; set; }
        public Logger Logger { get; set; }
        public Action<string> LogCallback { get; set; }

        public MainViewModel()
        {

            this.SetupIisCommand();
            this.SetupSqlLocaldbCommand();
            this.SetupRabbitMqCommand();
            this.SetupWebConsoleCommand();
            this.SetupElasticsearch();
            this.SetupWorkersCommand();

            SaveSettingsCommand = new RelayCommand(SaveSettings);
            ExitAppCommand = new RelayCommand(Exit);
            SetupCommand = new RelayCommand(Setup, () => !this.IsSetup);


        }

        private static void Setup()
        {
            MessageBox.Show("Use powershell to run Setup-SphApp.ps1");
        }

        public async Task LoadAsync()
        {
            this.IsBusy = true;
            this.BusyMessage = "Please wait while we take care few things....";


            this.Settings = SphSettings.Load();
            if (null == this.Settings)
            {
                MessageBox.Show("Cannot load your project.json", "Reactive Developer", MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
            // NOTE : see if the database, elasticsearch index, RabbitMq vhost etc are present
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

            var rabbitStarted = false;
            if (!string.IsNullOrEmpty(this.Settings.RabbitMqUserName) && !string.IsNullOrEmpty(this.Settings.RabbitMqPassword))
            {
                rabbitStarted = CheckRabbitMqHostConnection(this.Settings.RabbitMqUserName, this.Settings.RabbitMqPassword, this.Settings.ApplicationName);

            }
            RabbitMqServiceStarted = rabbitStarted;
            RabbitMqStatus = rabbitStarted ? "Running" : "Stopped";
            this.StartWebConsole();


            this.CheckWorkers();
            this.CheckIisExpress();
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
            // get the rabbitmq vhost
            var handler = new HttpClientHandler
            {
                Credentials = new NetworkCredential(this.Settings.RabbitMqUserName, this.Settings.RabbitMqPassword)
            };
            using (var client = new HttpClient(handler) { BaseAddress = new Uri("http://localhost:15672") })
            {
                var url = "api/vhosts/" + this.Settings.ApplicationName;
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

        private void SaveSettings()
        {
            this.Settings.Save();
            MessageBox.Show("SPH settings has been successfully saved", "SPH Control Panel");
        }

        private void Exit()
        {
            Environment.Exit(0);
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

        private void Log(string message, string severity = "Info")
        {
            this.Post(m =>
            {
                var log = $"@[{DateTime.Now:HH:mm:ss}] {m}";
                if (null != this.LogCallback)
                    LogCallback(log);
                Console.WriteLine(log);
            }, message);
            var json = JsonConvert.SerializeObject(new { time = DateTime.Now, message, severity });
            WebConsoleServer.Default.SendMessage(json);
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

        public void Stop()
        {
            this.StopWebConsole();
        }
    }
}
