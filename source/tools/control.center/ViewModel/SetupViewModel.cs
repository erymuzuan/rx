using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Threading;
using System.Windows;
using Bespoke.Sph.ControlCenter.Model;
using GalaSoft.MvvmLight.Command;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class SetupViewModel : ViewModelBase, IView
    {
        public RelayCommand<string> NextCommand { get; set; }
        public RelayCommand<string> PreviousCommand { get; set; }
        public RelayCommand FinishCommand { get; set; }

        public SetupViewModel()
        {
            this.NextCommand = new RelayCommand<string>(key =>
            {
                this.CurrentTab = key;
            }, key => !string.IsNullOrWhiteSpace(this.Settings?.ApplicationName) && !this.IsBusy);
            this.PreviousCommand = new RelayCommand<string>(key =>
            {
                this.CurrentTab = key;
            }, key => !this.IsBusy);
            this.FinishCommand = new RelayCommand(Setup, () =>
            {
                if (string.IsNullOrWhiteSpace(this.Settings?.ApplicationName))
                    return false;
                return !this.IsBusy;
            });

            this.GeneralVisible = Visibility.Visible;
            this.RabbitMqVisible = Visibility.Collapsed;
            this.ElasticsearchVisible = Visibility.Collapsed;
            this.SqlServerVisible = Visibility.Collapsed;
            this.SetupVisible = Visibility.Collapsed;
            this.Settings = new SphSettings
            {
                RabbitMqDirectory = ".\\rabbitmq_server",
                RabbitMqManagementPort = 15672,
                RabbitMqPort = 5672,
                RabbitMqHost = "localhost",
                RabbitMqPassword = "guest",
                RabbitMqUserName = "guest",

                ElasticSearchJar = ".jar",
                ElasticsearchClusterName = "",
                ElasticsearchHttpPort = 9200,
                ElasticsearchNodeName = "",
                ElasticsearchindexNumberOfReplicas = 0,
                ElasticsearchindexNumberOfShards = 1,
                LoggerWebSocketPort = 50238,
                WebsitePort = 50230,
                ProjectDirectory = "",
                ApplicationName = "",
                IisExpressExecutable = ".\\IIS Express\\iisexpress.exe",
                JavaHome = Environment.GetEnvironmentVariable("JAVA_HOME"),
                SqlLocalDbName = "Projects"

            };
            this.Settings.PropertyChanged += Settings_PropertyChanged;
        }

        public void Load()
        {
            var root = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName);
            var eslib = Path.Combine(root.FullName, "elasticsearch\\lib\\");
            var jar = Directory.GetFiles(eslib, "elasticsearch-*.jar").Single();
            this.Settings.ElasticSearchJar = jar;
        }

        private void Settings_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ApplicationName")
            {
                if (string.IsNullOrWhiteSpace(this.Settings.ElasticsearchClusterName))
                    this.Settings.ElasticsearchClusterName = $"cluster_{Environment.MachineName}_{this.Settings.ApplicationName}";

                if (string.IsNullOrWhiteSpace(this.Settings.ElasticsearchNodeName))
                    this.Settings.ElasticsearchNodeName = string.Format("node_{0}_{1}", Environment.MachineName,
                        this.Settings.ApplicationName);
            }
            this.NextCommand.RaiseCanExecuteChanged();
            this.FinishCommand.RaiseCanExecuteChanged();
        }

        protected override void RaisePropertyChanged(string propertyName)
        {
            if (propertyName == "CurrentTab")
            {
                this.GeneralVisible = this.CurrentTab == "General" ? Visibility.Visible : Visibility.Collapsed;
                this.RabbitMqVisible = this.CurrentTab == "RabbitMq" ? Visibility.Visible : Visibility.Collapsed;
                this.ElasticsearchVisible = this.CurrentTab == "Elasticsearch" ? Visibility.Visible : Visibility.Collapsed;
                this.SqlServerVisible = this.CurrentTab == "SqlServer" ? Visibility.Visible : Visibility.Collapsed;
                this.SetupVisible = this.CurrentTab == "Setup" ? Visibility.Visible : Visibility.Collapsed;
            }
        }

        public void Setup()
        {
            this.IsBusy = true;
            this.Message = "Please wait.. while we run the setup for you";
            this.QueueUserWorkItem(RunSetup, this.Settings);
        }

        void UpdateProgress(double step = 0.2d, string message = ". ")
        {
            this.Post(() =>
            {
                if (this.Progress < 100)
                    this.Progress += step;
                this.Message += message;
            });
        }
        private void RunSetup(SphSettings settings)
        {
            var path = "Setup-SphApp.ps1".TranslatePath();
            var wc = ".".TranslatePath();
            if (!File.Exists(path))
            {
                this.Post(() =>
                {
                    this.IsBusy = false;
                    this.Status = "fail";
                    MessageBox.Show($"Cannot find {path}", "Reactive developer", MessageBoxButton.OK, MessageBoxImage.Error);
                });
                return;
            }
            var main = new MainViewModel
            {
                Settings = settings,
                View = this.View,
                ConsoleLogger = new ConsoleNotificationSubscriber(settings),
                Logger = new Logger()
            };
            this.Message += "\r\nStarting RabbitMq\r\n";
            main.StartRabbitMqService();
            this.Message += "Starting SQL Server\r\n";
            main.StartSqlService();
            this.Message += "Starting Elasticsearch\r\n";
            main.StartElasticSearch();

            var flag = new ManualResetEvent(false);
            var starting = true;
            this.QueueUserWorkItem(() =>
            {
                while (!main.RabbitMqServiceStarted && starting)
                {
                    Thread.Sleep(200);
                    UpdateProgress();
                }
                this.Message += "\r\nRabbitMq started ...\r\n";
                while (!main.SqlServiceStarted && starting)
                {
                    Thread.Sleep(200);
                    UpdateProgress();
                }
                this.Message += "\r\nSql Server started ...\r\n";
                while (!main.ElasticSearchServiceStarted && starting)
                {
                    Thread.Sleep(200);
                    UpdateProgress();
                }
                this.Message += "\r\nElasticsearch started ...\r\n";
                flag.Set();
            });

            flag.WaitOne(TimeSpan.FromSeconds(60));
            var showStartFailure = new Action<string>(m =>
            {
                starting = false;
                this.Post(() =>
                {
                    this.IsBusy = false;
                    MessageBox.Show(m);
                });

            });
            if (!main.RabbitMqServiceStarted)
            {
                showStartFailure("Fail to start RabbitMq");
                return;
            }
            if (!main.SqlServiceStarted)
            {
                showStartFailure("Fail to start SQL Server");
                return;
            }
            if (!main.ElasticSearchServiceStarted)
            {
                showStartFailure("Fail to start Elasticsearch");
                return;
            }


            this.Message += $"\r\nRunning ps1 in {wc}\r\n";
            using (var ps = PowerShell.Create())
            {
                var ps1 = File.ReadAllText(path);
                ps.AddScript(ps1);
                ps.AddParameter("WorkingCopy", wc);
                ps.AddParameter("ApplicationName", settings.ApplicationName);
                ps.AddParameter("Port", settings.WebsitePort ?? 50230);
                ps.AddParameter("SqlServer", settings.SqlLocalDbName);
                ps.AddParameter("RabbitMqUserName", settings.RabbitMqUserName);
                ps.AddParameter("RabbitMqPassword", settings.RabbitMqPassword);
                ps.AddParameter("ElasticSearchHost", "http://localhost:" + settings.ElasticsearchHttpPort);

                var outputCollection = new PSDataCollection<PSObject>();
                outputCollection.DataAdded += outputCollection_DataAdded;
                ps.Streams.Error.DataAdded += Error_DataAdded;
                ps.Streams.Verbose.DataAdded += Verbose_DataAdded;
                ps.Streams.Warning.DataAdded += Warning_DataAdded;
                ps.Streams.Debug.DataAdded += Debug_DataAdded;
                ps.Streams.Progress.DataAdded += Progress_DataAdded;
                ps.InvocationStateChanged += Ps_InvocationStateChanged;


                var result = ps.BeginInvoke<PSObject, PSObject>(null, outputCollection);


                while (result.IsCompleted == false)
                {
                    Console.WriteLine("Waiting for pipeline to finish...");
                    UpdateProgress();

                    Thread.Sleep(100);
                }

                this.Message += "Error : Execution has stopped. The pipeline state " + ps.InvocationStateInfo.State;

                foreach (var outputItem in outputCollection)
                {
                    //TODO: handle/process the output items if required
                    this.Message += outputItem.BaseObject + "\r\n";
                }
                var status = ps.HadErrors ? "fail" : "success";
                this.Post(() =>
                {
                    this.Progress = 100;
                    if (status == "success")
                        MessageBox.Show("Congratulations.. you now can start building your app", "Reactive Developer", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Unfortunately. there are errors", "Reactive Developer", MessageBoxButton.OK, MessageBoxImage.Error);

                    this.Status = status;
                    this.IsBusy = false;
                });
            }
        }

        private void Progress_DataAdded(object sender, DataAddedEventArgs e)
        {
            var message = "";
            var errors = sender as PSDataCollection<ProgressRecord>;
            if (null != errors)
            {
                var er = errors[e.Index];
                message = er + "\r\n";
            }
            this.Post(a =>
            {
                this.Message += message;
            }, e);
        }

        private void Debug_DataAdded(object sender, DataAddedEventArgs e)
        {
            var message = "";
            var errors = sender as PSDataCollection<DebugRecord>;
            if (null != errors)
            {
                var er = errors[e.Index];
                message = er + "\r\n";
            }
            this.Post(a =>
            {
                this.Message += message;
            }, e);
        }

        private void Warning_DataAdded(object sender, DataAddedEventArgs e)
        {
            var message = "";
            var errors = sender as PSDataCollection<WarningRecord>;
            if (null != errors)
            {
                var er = errors[e.Index];
                message = er + "\r\n";
            }
            this.Post(a =>
            {
                this.Message += message;
            }, e);
        }

        private void Verbose_DataAdded(object sender, DataAddedEventArgs e)
        {
            var message = "";
            var streams = sender as PSDataCollection<VerboseRecord>;
            if (null != streams)
            {
                var vb = streams[e.Index];
                message = vb.Message + "\r\n";
            }
            this.Post(a =>
            {
                this.Message += message;
            }, e);
        }

        private void Ps_InvocationStateChanged(object sender, PSInvocationStateChangedEventArgs e)
        {
            this.Message += "\r\nInvocationStateChanged : reason -> " + e.InvocationStateInfo.Reason + ", state -> " + e.InvocationStateInfo.State + "\r\n";
        }

        private void outputCollection_DataAdded(object sender, DataAddedEventArgs e)
        {
            var psobjects = sender as PSDataCollection<PSObject>;
            var message = "";
            if (null != psobjects)
            {
                var ps = psobjects[e.Index];
                message = ps.ToString();
            }
            this.Post(a =>
            {
                this.Message += a + "\r\n";
            }, message);
        }

        private void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            var message = "";
            var errors = sender as PSDataCollection<ErrorRecord>;
            if (null != errors)
            {
                var er = errors[e.Index];
                message = er + "\r\n";
            }
            this.Post(a =>
            {
                this.Message += message;
            }, e);
        }

    }
}
