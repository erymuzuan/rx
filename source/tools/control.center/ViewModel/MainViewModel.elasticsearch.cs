using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Windows;
using Bespoke.Sph.ControlCenter.Model;
using GalaSoft.MvvmLight.Command;
using EventLog = Bespoke.Sph.ControlCenter.Model.EventLog;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class MainViewModel
    {
        private Process m_elasticProcess;
        private int m_elasticSearchId;
        public RelayCommand StartElasticSearchCommand { get; set; }
        public RelayCommand StopElasticSearchCommand { get; set; }

        public void SetupElasticsearch()
        {

            StartElasticSearchCommand = new RelayCommand(StartElasticsearch, () => !ElasticSearchServiceStarted && RabbitMqServiceStarted && !RabbitMqServiceStarting);
            StopElasticSearchCommand = new RelayCommand(StopElasticSearch, () => ElasticSearchServiceStarted);



        }
        
        public void StartElasticsearch()
        {
            this.IsBusy = true;
            this.QueueUserWorkItem(StartElasticsearchHelper);
        }

        private async void StartElasticsearchHelper()
        {

            Log("ElasticSearch...[INITIATING]");

            var esHome = Path.GetDirectoryName(Path.GetDirectoryName(this.Settings.ElasticSearchJar));
            var version = this.Settings.ElasticSearchJar.RegexSingleValue(@"elasticsearch-(?<version>\d.\d.\d).jar", "version");
            var es = string.Join(@"\", esHome).TranslatePath();
            Log("Elasticsearch Home " + esHome);
            Log("Version :" + version);

            const string JAVA_OPTS = "-Xms256m -Xmx1g -Xss256k -XX:+UseParNewGC -XX:+UseConcMarkSweepGC -XX:CMSInitiatingOccupancyFraction=75 -XX:+UseCMSInitiatingOccupancyOnly -XX:+HeapDumpOnOutOfMemoryError";
            // ReSharper disable InconsistentNaming


            //NOTE : V2 CLASS_PATH is a little different than V1
            string CLASS_PATH = $"\"{es}/lib/elasticsearch-{version}.jar;{es}/lib/*\"";
            string ES_PARAMS = $@"-Delasticsearch -Des-foreground=yes -Des.path.home=""{es}""";
            // ReSharper restore InconsistentNaming

            // NOTE : V2 got to add "start" at the end
            var arg = $@" {JAVA_OPTS} {ES_PARAMS} -cp {CLASS_PATH} ""org.elasticsearch.bootstrap.Elasticsearch"" start ";
            if (version.StartsWith("1."))
            {
                CLASS_PATH = $"\";{es}/lib/elasticsearch-{version}.jar;{es}/lib/*;{es}/lib/sigar/*\"";
                arg = $"{JAVA_OPTS} {ES_PARAMS} -cp {CLASS_PATH}  \"org.elasticsearch.bootstrap.Elasticsearch\"";
            }
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

            Log(info.FileName);
            Log(arg);
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
                        connected = ok.Contains("tagline");
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

     
    }
}
