using System.IO;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
        private string m_applicationName;
        public string ApplicationName
        {
            set
            {
                m_applicationName = value;
                OnPropertyChanged("ApplicationName");
            }
            get { return m_applicationName; }
        }

        private string m_sqlLocalDbName;
        public string SqlLocalDbName
        {
            set
            {
                m_sqlLocalDbName = value;
                OnPropertyChanged("SqlLocalDbName");
            }
            get { return m_sqlLocalDbName; }
        }

        private TextWriter m_writer;
        public TextWriter TextWriter
        {
            set
            {
                m_writer = value;
                OnPropertyChanged("TextWriter");
            }
            get { return m_writer; }
        }

        private string m_javaHome;
        public string JavaHome
        {
            set
            {
                m_javaHome = value;
                OnPropertyChanged("JavaHome");
            }
            get { return m_javaHome; }
        }

        private string m_elasticSearchHome;
        public string ElasticSearchHome
        {
            set
            {
                m_elasticSearchHome = value;
                OnPropertyChanged("ElasticSearchHome");
            }
            get { return m_elasticSearchHome; }
        }

        private string m_iisExpressDirectory;
        public string IisExpressDirectory
        {
            set
            {
                m_iisExpressDirectory = value;
                OnPropertyChanged("IisExpressDirectory");
            }
            get { return m_iisExpressDirectory; }
        }

        private string m_projectDirectory;
        public string ProjectDirectory
        {
            set
            {
                m_projectDirectory = value;
                OnPropertyChanged("ProjectDirectory");
            }
            get { return m_projectDirectory; }
        }

        private string m_rabbitMqDirectory;
        public string RabbitMqDirectory
        {
            set
            {
                m_rabbitMqDirectory = value;
                OnPropertyChanged("RabbitMqDirectory");
            }
            get { return m_rabbitMqDirectory; }
        }

        private bool m_elasticSearchServiceStarted;
        public bool ElasticSearchServiceStarted
        {
            set
            {
                m_elasticSearchServiceStarted = value;
                OnPropertyChanged("ElasticSearchServiceStarted");
            }
            get { return m_elasticSearchServiceStarted; }
        }

        private bool m_rabbitMqServiceStarted;
        public bool RabbitMqServiceStarted
        {
            set
            {
                m_rabbitMqServiceStarted = value;
                OnPropertyChanged("RabbitMqServiceStarted");
            }
            get { return m_rabbitMqServiceStarted; }
        }



        private bool m_iisServiceStarted;
        public bool IisServiceStarted
        {
            set
            {
                m_iisServiceStarted = value;
                OnPropertyChanged("IisServiceStarted");
            }
            get { return m_iisServiceStarted; }
        }
        
        private bool m_sqlServiceStarted;
        public bool SqlServiceStarted
        {
            set
            {
                m_sqlServiceStarted = value;
                OnPropertyChanged("SqlServiceStarted");
            }
            get { return m_sqlServiceStarted; }
        }

        private bool m_sphWorkerServiceStarted;
        public bool SphWorkerServiceStarted
        {
            set
            {
                m_sphWorkerServiceStarted = value;
                OnPropertyChanged("SphWorkerServiceStarted");
            }
            get { return m_sphWorkerServiceStarted; }
        }

        private string m_elasticSearchStatus;
        public string ElasticSearchStatus
        {
            set
            {
                m_elasticSearchStatus = value;
                OnPropertyChanged("ElasticSearchStatus");
            }
            get { return m_elasticSearchStatus; }
        }

        private string m_rabbitMqStatus;
        public string RabbitMqStatus
        {
            set
            {
                m_rabbitMqStatus = value;
                OnPropertyChanged("RabbitMqStatus");
            }
            get { return m_rabbitMqStatus; }
        }

        private string m_iisStatus;
        public string IisStatus
        {
            set
            {
                m_iisStatus = value;
                OnPropertyChanged("IisStatus");
            }
            get { return m_iisStatus; }
        }

        private string m_sqlServiceStatus;
        public string SqlServiceStatus
        {
            set
            {
                m_sqlServiceStatus = value;
                OnPropertyChanged("SqlServiceStatus");
            }
            get { return m_sqlServiceStatus; }
        }

        private string m_iisServiceStatus;
        public string IisServiceStatus
        {
            set
            {
                m_iisServiceStatus = value;
                OnPropertyChanged("IisServiceStatus");
            }
            get { return m_iisServiceStatus; }
        }

        private string m_sphWorkersStatus;
        public string SphWorkersStatus
        {
            set
            {
                m_sphWorkersStatus = value;
                OnPropertyChanged("SphWorkersStatus");
            }
            get { return m_sphWorkersStatus; }
        }
    }
}
