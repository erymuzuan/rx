using System;
using System.IO;
using System.Linq;
using System.Management.Automation;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Bespoke.Sph.ControlCenter.Model;
using GalaSoft.MvvmLight.Command;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public class SetupViewModel : ViewModelBase, IView
    {
        public RelayCommand<string> NextCommand { get; set; }
        public RelayCommand<string> PreviousCommand { get; set; }
        public RelayCommand FinishCommand { get; set; }

        public SetupViewModel()
        {
            this.NextCommand = new RelayCommand<string>(key =>
            {
                this.CurrentTab = key;
            }, key => !string.IsNullOrWhiteSpace(this.Settings?.ApplicationName));
            this.PreviousCommand = new RelayCommand<string>(key =>
            {
                this.CurrentTab = key;
            }, key => true);
            this.FinishCommand = new RelayCommand(Setup, () =>
            {
                if (string.IsNullOrWhiteSpace(this.Settings?.ApplicationName))
                    return false;
                if (this.IsBusy) return false;
                return true;
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
                JavaHome = "",
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

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ApplicationName")
            {
                if (string.IsNullOrWhiteSpace(this.Settings.ElasticsearchClusterName))
                    this.Settings.ElasticsearchClusterName = string.Format("cluster_{0}_{1}", Environment.MachineName,
                        this.Settings.ApplicationName);
                if (string.IsNullOrWhiteSpace(this.Settings.ElasticsearchNodeName))
                    this.Settings.ElasticsearchNodeName = string.Format("node_{0}_{1}", Environment.MachineName,
                        this.Settings.ApplicationName);
            }
            this.NextCommand.RaiseCanExecuteChanged();
            this.FinishCommand.RaiseCanExecuteChanged();
        }

        protected override void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            if (propertyName == "CurrentTab")
            {
                this.GeneralVisible = this.CurrentTab == "General" ? Visibility.Visible : Visibility.Collapsed;
                this.RabbitMqVisible = this.CurrentTab == "RabbitMq" ? Visibility.Visible : Visibility.Collapsed;
                this.ElasticsearchVisible = this.CurrentTab == "Elasticsearch" ? Visibility.Visible : Visibility.Collapsed;
                this.SqlServerVisible = this.CurrentTab == "SqlServer" ? Visibility.Visible : Visibility.Collapsed;
                this.SetupVisible = this.CurrentTab == "Setup" ? Visibility.Visible : Visibility.Collapsed;
            }
            base.OnPropertyChanged(propertyName);
        }

        private SphSettings m_settings;
        private Visibility m_generalVisible;
        private Visibility m_sqlServerVisible;
        private Visibility m_elasticsearchVisible;
        private Visibility m_rabbitMqVisible;
        private string m_currentTab;
        private Visibility m_setupVisible;
        private bool m_isBusy;

        public int Progress
        {
            get { return m_progress; }
            set
            {
                m_progress = value;
                OnPropertyChanged();
            }
        }

        public bool IsBusy
        {
            get { return m_isBusy; }
            set
            {
                m_isBusy = value;
                OnPropertyChanged();
                this.FinishCommand.RaiseCanExecuteChanged();
            }
        }

        public Visibility SetupVisible
        {
            get { return m_setupVisible; }
            set
            {
                m_setupVisible = value;
                OnPropertyChanged();
            }
        }

        public string CurrentTab
        {
            get { return m_currentTab; }
            set
            {
                m_currentTab = value;
                OnPropertyChanged();
            }
        }

        public Visibility RabbitMqVisible
        {
            get { return m_rabbitMqVisible; }
            set
            {
                m_rabbitMqVisible = value;
                OnPropertyChanged();
            }
        }

        public Visibility ElasticsearchVisible
        {
            get { return m_elasticsearchVisible; }
            set
            {
                m_elasticsearchVisible = value;
                OnPropertyChanged();
            }
        }

        public Visibility SqlServerVisible
        {
            get { return m_sqlServerVisible; }
            set
            {
                m_sqlServerVisible = value;
                OnPropertyChanged();
            }
        }

        public Visibility GeneralVisible
        {
            get { return m_generalVisible; }
            set
            {
                m_generalVisible = value;
                OnPropertyChanged();
            }
        }
        public SphSettings Settings
        {
            get { return m_settings; }
            set
            {
                m_settings = value;
                OnPropertyChanged();
            }
        }

        public void Setup()
        {
            this.IsBusy = true;
            this.Message = "Please wait.. while we run the setup for you";
            this.QueueUserWorkItem(RunSetup, this.Settings);
        }

        private void RunSetup(SphSettings settings)
        {
            using (var ps = PowerShell.Create())
            {
                // this script has a sleep in it to simulate a long running script
                ps.AddScript("$s1 = 'test1'; $s2 = 'test2'; $s1; write-error 'some error';start-sleep -s 7; $s2");

                // prepare a new collection to store output stream objects
                var outputCollection = new PSDataCollection<PSObject>();
                outputCollection.DataAdded += outputCollection_DataAdded;

                // the streams (Error, Debug, Progress, etc) are available on the PowerShell instance.
                // we can review them during or after execution.
                // we can also be notified when a new item is written to the stream (like this):
                ps.Streams.Error.DataAdded += Error_DataAdded;

                // begin invoke execution on the pipeline
                // use this overload to specify an output stream buffer
                var result = ps.BeginInvoke<PSObject, PSObject>(null, outputCollection);

                // do something else until execution has completed.
                // this could be sleep/wait, or perhaps some other work
                while (result.IsCompleted == false)
                {
                    Console.WriteLine("Waiting for pipeline to finish...");
                    if (this.Progress < 100)
                        this.Progress += 1;
                    else
                        this.Progress = 0;

                    Thread.Sleep(100);

                    // might want to place a timeout here...
                }

                Console.WriteLine("Execution has stopped. The pipeline state: " + ps.InvocationStateInfo.State);

                foreach (var outputItem in outputCollection)
                {
                    //TODO: handle/process the output items if required
                    this.Message += outputItem.BaseObject + "\r\n";
                }
                this.Post(() =>
                {
                    this.IsBusy = false;
                    this.Progress = 100;
                    MessageBox.Show("Congratulations.. you now can start building your app", "ReactiveDeveloper", MessageBoxButton.OK, MessageBoxImage.Information);
                });
            }
        }

        private void outputCollection_DataAdded(object sender, DataAddedEventArgs e)
        {
            this.Post(a =>
            {
                this.Message += a + "\r\n";
            }, e);
        }

        private void Error_DataAdded(object sender, DataAddedEventArgs e)
        {
            this.Post(a =>
            {
                this.Message += a + "\r\n";
            }, e);
        }

        private string m_message;
        private int m_progress;

        public string Message
        {
            get { return m_message; }
            set
            {
                m_message = value;
                OnPropertyChanged();
            }
        }

        public DispatcherObject View { get; set; }
    }
}
