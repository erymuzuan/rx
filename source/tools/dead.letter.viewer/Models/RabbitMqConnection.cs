namespace Bespoke.Station.Windows.RabbitMqDeadLetter.Models
{
    public class RabbitMqConnection : ModelBase
    {

        private int m_port;
        private string m_password;
        private string m_userName;
        private string m_virtualHost;
        private string m_hostName;

        public int Port
        {
            get { return m_port; }
            set
            {
                m_port = value;
                RaisePropertyChanged();
            }
        }

        public string Password
        {
            get { return m_password; }
            set
            {
                m_password = value;
                RaisePropertyChanged();
            }
        }

        public string UserName
        {
            get { return m_userName; }
            set
            {
                m_userName = value;
                RaisePropertyChanged();
            }
        }


        public string VirtualHost
        {
            get { return m_virtualHost; }
            set
            {
                m_virtualHost = value;
                RaisePropertyChanged();
            }
        }

        private int m_apiPort;
        public int ApiPort
        {
            get { return m_apiPort; }
            set
            {
                m_apiPort = value;
                RaisePropertyChanged();
            }
        }
        public string HostName
        {
            get { return m_hostName; }
            set
            {
                m_hostName = value;
                RaisePropertyChanged();
            }
        }

    }
}