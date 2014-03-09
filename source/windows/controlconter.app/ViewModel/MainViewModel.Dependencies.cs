using System.IO;

namespace Bespoke.Sph.ControlCenter.ViewModel
{
    public partial class MainViewModel : ViewModelBase
    {
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

        private string m_webProjectDirectory;
        public string WebProjectDirectory
        {
            set
            {
                m_webProjectDirectory = value;
                OnPropertyChanged("WebProjectDirectory");
            }
            get { return m_webProjectDirectory; }
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
    }
}
