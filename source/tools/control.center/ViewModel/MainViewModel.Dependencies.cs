using System;
using System.IO;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private string m_applicationName;
        private bool m_isSetup;

        public bool IsSetup
        {
            get { return m_isSetup; }
            set
            {
                m_isSetup = value;
                OnPropertyChanged();
                this.StartIisServiceCommand.RaiseCanExecuteChanged();
                this.StartSphWorkerCommand.RaiseCanExecuteChanged();
            }
        }
        public string ApplicationName
        {
            set
            {
                m_applicationName = value;
                OnPropertyChanged();
            }
            get { return m_applicationName; }
        }

        private string m_sqlLocalDbName;
        public string SqlLocalDbName
        {
            set
            {
                m_sqlLocalDbName = value;
                OnPropertyChanged();
            }
            get
            {
                return string.IsNullOrWhiteSpace(m_sqlLocalDbName) ?
                            @"(localdb)\Projects" :
                            m_sqlLocalDbName;
            }
        }

        private string m_rabbitmqUserName;
        public string RabbitmqUserName
        {
            set
            {
                m_rabbitmqUserName = value;
                OnPropertyChanged();
            }
            get
            {
                return string.IsNullOrWhiteSpace(m_rabbitmqUserName) ?
                    "guest" :
                    m_rabbitmqUserName;
            }
        }

        private string m_rabbitmqPassword;
        public string RabbitmqPassword
        {
            set
            {
                m_rabbitmqPassword = value;
                OnPropertyChanged();
            }
            get
            {
                return string.IsNullOrWhiteSpace(m_rabbitmqPassword) ?
                    "guest" :
                    m_rabbitmqPassword;
            }
        }

        private TextWriter m_writer;
        public TextWriter TextWriter
        {
            set
            {
                m_writer = value;
                OnPropertyChanged();
            }
            get { return m_writer; }
        }

        private string m_javaHome;
        public string JavaHome
        {
            set
            {
                m_javaHome = value;
                OnPropertyChanged();
            }
            get
            {
                return string.IsNullOrWhiteSpace(m_javaHome) ?
                    Environment.GetEnvironmentVariable("JAVA_HOME")
                    : m_javaHome;
            }
        }

        private string m_elasticSearchHome;
        public string ElasticSearchHome
        {
            set
            {
                m_elasticSearchHome = value;
                OnPropertyChanged();
            }
            get
            {
                return string.IsNullOrWhiteSpace(m_elasticSearchHome) ?
                    "elasticsearch" :
                    m_elasticSearchHome;
            }
        }

        private string m_elasticSearchVersion;
        public string ElasticSearchVersion
        {
            get { return m_elasticSearchVersion; }
            set
            {
                m_elasticSearchVersion = value;
                OnPropertyChanged();
            }
        }

        private string m_iisExpressDirectory;
        public string IisExpressDirectory
        {
            set
            {
                m_iisExpressDirectory = value;
                OnPropertyChanged();
            }
            get
            {
                return string.IsNullOrWhiteSpace(m_iisExpressDirectory) ?
                    @"IIS Express\iisexpress.exe" :
                    m_iisExpressDirectory;
            }
        }

        private string m_projectDirectory;
        public string ProjectDirectory
        {
            set
            {
                m_projectDirectory = value;
                OnPropertyChanged();
            }
            get { return m_projectDirectory; }
        }

        private string m_rabbitMqDirectory;
        public string RabbitMqDirectory
        {
            set
            {
                m_rabbitMqDirectory = value;
                OnPropertyChanged();
            }
            get
            {
                return string.IsNullOrWhiteSpace(m_rabbitMqDirectory) ?
                    "rabbitmq_server"
                    : m_rabbitMqDirectory;
            }
        }

        private bool m_elasticSearchServiceStarted;
        public bool ElasticSearchServiceStarted
        {
            set
            {
                m_elasticSearchServiceStarted = value;
                OnPropertyChanged();
            }
            get { return m_elasticSearchServiceStarted; }
        }

        private bool m_rabbitMqServiceStarted;
        public bool RabbitMqServiceStarted
        {
            set
            {
                m_rabbitMqServiceStarted = value;
                OnPropertyChanged();
            }
            get { return m_rabbitMqServiceStarted; }
        }



        private bool m_iisServiceStarted;
        public bool IisServiceStarted
        {
            set
            {
                m_iisServiceStarted = value;
                OnPropertyChanged();
            }
            get { return m_iisServiceStarted; }
        }

        private bool m_sqlServiceStarted;
        public bool SqlServiceStarted
        {
            set
            {
                m_sqlServiceStarted = value;
                OnPropertyChanged();
            }
            get { return m_sqlServiceStarted; }
        }

        private bool m_sphWorkerServiceStarted;
        public bool SphWorkerServiceStarted
        {
            set
            {
                m_sphWorkerServiceStarted = value;
                OnPropertyChanged();
            }
            get { return m_sphWorkerServiceStarted; }
        }

        private string m_elasticSearchStatus;
        public string ElasticSearchStatus
        {
            set
            {
                m_elasticSearchStatus = value;
                OnPropertyChanged();
            }
            get { return m_elasticSearchStatus; }
        }

        private string m_rabbitMqStatus;
        public string RabbitMqStatus
        {
            set
            {
                m_rabbitMqStatus = value;
                OnPropertyChanged();
            }
            get { return m_rabbitMqStatus; }
        }

        private string m_iisStatus;
        public string IisStatus
        {
            set
            {
                m_iisStatus = value;
                OnPropertyChanged();
            }
            get { return m_iisStatus; }
        }

        private string m_sqlServiceStatus;
        public string SqlServiceStatus
        {
            set
            {
                m_sqlServiceStatus = value;
                OnPropertyChanged();
            }
            get { return m_sqlServiceStatus; }
        }

        private string m_iisServiceStatus;
        public string IisServiceStatus
        {
            set
            {
                m_iisServiceStatus = value;
                OnPropertyChanged();
            }
            get { return m_iisServiceStatus; }
        }

        private string m_sphWorkersStatus;
        private int m_port;

        public string SphWorkersStatus
        {
            set
            {
                m_sphWorkersStatus = value;
                OnPropertyChanged();
            }
            get { return m_sphWorkersStatus; }
        }

        public int Port
        {
            get { return m_port; }
            set
            {
                m_port = value;
                OnPropertyChanged();
            }
        }
    }
}
