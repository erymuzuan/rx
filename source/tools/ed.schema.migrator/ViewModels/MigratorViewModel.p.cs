namespace ed.schema.migrator.ViewModels
{
    public partial class MigratorViewModel
    {
        private string m_elasticSearchHost;
        private int m_elasticSearchPort;
        private string m_applicationName;
        private string m_sqlServerName;
        private bool m_isTrustedConnection;
        private string m_sqlServerUserName;
        private string m_sqlServerPassword;
        private bool m_isBusy;
        private string m_message;

        public string Message
        {
            get { return m_message; }
            set
            {
                m_message = value;
                RaisePropertyChanged();
            }
        }

        public bool IsBusy
        {
            get { return m_isBusy; }
            set
            {
                m_isBusy = value;
                RaisePropertyChanged();
            }
        }






        public string SqlServerPassword
        {
            get { return m_sqlServerPassword; }
            set
            {
                m_sqlServerPassword = value;
                RaisePropertyChanged();
            }
        }

        public string SqlServerUserName
        {
            get { return m_sqlServerUserName; }
            set
            {
                m_sqlServerUserName = value;
                RaisePropertyChanged();
            }
        }

        public bool IsTrustedConnection
        {
            get { return m_isTrustedConnection; }
            set
            {
                m_isTrustedConnection = value;
                RaisePropertyChanged();
            }
        }

        public string SqlServerName
        {
            get { return m_sqlServerName; }
            set
            {
                m_sqlServerName = value;
                RaisePropertyChanged();
            }
        }

        public string ApplicationName
        {
            get { return m_applicationName; }
            set
            {
                m_applicationName = value;
                RaisePropertyChanged();
            }
        }

        public int ElasticSearchPort
        {
            get { return m_elasticSearchPort; }
            set
            {
                m_elasticSearchPort = value;
                RaisePropertyChanged();
            }
        }

        public string ElasticSearchHost
        {
            get { return m_elasticSearchHost; }
            set
            {
                m_elasticSearchHost = value;
                RaisePropertyChanged();
            }
        }
         
    }
}