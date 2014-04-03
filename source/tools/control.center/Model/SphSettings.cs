using System;
using System.Xml.Serialization;
using Bespoke.Sph.ControlCenter.Helpers;

namespace Bespoke.Sph.ControlCenter.Model
{
    [XmlType("SphSettings", Namespace = Strings.DefaultNamespace)]
    public class SphSettings : DomainObject
    {
        private string m_sqlLocalDbName;
        private string m_iisExpressDirectory;
        private string m_rabbitMqDirectory;
        private string m_elasticSearchHome;
        private string m_javaHome;
        private string m_rabbitMqPassword;
        private string m_rabbitMqUserName;
        private int m_port;


        public string SqlLocalDbName
        {
            get
            {
                return string.IsNullOrWhiteSpace(m_sqlLocalDbName) ?
                            @"(localdb)\Projects" :
                            m_sqlLocalDbName;
            }
            set
            {
                m_sqlLocalDbName = value;
                OnPropertyChanged();
            }
        }

        public string IisExpressDirectory
        {
            get
            {
                return string.IsNullOrWhiteSpace(m_iisExpressDirectory) ?
                    @".\IIS Express\iisexpress.exe" :
                    m_iisExpressDirectory;
            }
            set
            {
                m_iisExpressDirectory = value;
                OnPropertyChanged();
            }
        }


        public string RabbitMqDirectory
        {
            get
            {
                return string.IsNullOrWhiteSpace(m_rabbitMqDirectory) ?
                    ".\rabbitmq_server"
                    : m_rabbitMqDirectory;
            }
            set
            {
                m_rabbitMqDirectory = value;
                OnPropertyChanged();
            }
        }

        public string RabbitMqUserName
        {
            get
            {
                return string.IsNullOrWhiteSpace(m_rabbitMqUserName) ?
                    "guest" :
                    m_rabbitMqUserName;
            }
            set
            {
                m_rabbitMqUserName = value;
                OnPropertyChanged();
            }
        }

        public string RabbitMqPassword
        {
            get
            {
                return string.IsNullOrWhiteSpace(m_rabbitMqPassword) ?
                    "guest" :
                    m_rabbitMqPassword;
            }
            set
            {
                m_rabbitMqPassword = value;
                OnPropertyChanged();
            }
        }

        public string JavaHome
        {
            get
            {
                return string.IsNullOrWhiteSpace(m_javaHome) ?
                    Environment.GetEnvironmentVariable("JAVA_HOME")
                    : m_javaHome;
            }
            set { m_javaHome = value; }
        }

        public string ElasticSearchHome
        {
            get
            {
                return string.IsNullOrWhiteSpace(m_elasticSearchHome) ?
                    @".\elasticsearch" :
                    m_elasticSearchHome;
            }
            set { m_elasticSearchHome = value; }
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
