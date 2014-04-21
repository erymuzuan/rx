using Bespoke.Sph.Domain;

namespace Bespoke.Sph.RabbitMqPublisher
{
    public class DefaultBrokerConnection : IBrokerConnection
    {
        private int m_managementPort;
        private int m_port;
        private string m_virtualHost;
        private string m_password;
        private string m_userName;
        private string m_host;

        public string Host
        {
            get
            {
                return string.IsNullOrWhiteSpace(m_host) ? "localhost" : m_host;
            }
            set { m_host = value; }
        }

        public string UserName
        {
            get
            {
                return string.IsNullOrWhiteSpace(m_userName) ? "guest" : m_userName;
            }
            set { m_userName = value; }
        }

        public string Password
        {
            get
            {
                return string.IsNullOrWhiteSpace(m_password) ? "guest" : m_password;
            }
            set { m_password = value; }
        }

        public int Port
        {
            get
            {
                return m_port == 0 ? 5672 : m_port;
            }
            set { m_port = value; }
        }

        public int ManagementPort
        {
            get
            {
                return m_managementPort == 0 ? 15672 : m_managementPort;
            }
            set { m_managementPort = value; }
        }

        public string VirtualHost
        {
            get
            {
                return string.IsNullOrWhiteSpace(m_virtualHost) ? ConfigurationManager.ApplicationName : m_virtualHost;
            }
            set { m_virtualHost = value; }
        }
    }
}