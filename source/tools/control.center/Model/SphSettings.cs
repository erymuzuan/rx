using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using Bespoke.Sph.ControlCenter.Helpers;

namespace Bespoke.Sph.ControlCenter.Model
{
    [XmlType("SphSettings", Namespace = Strings.DEFAULT_NAMESPACE)]
    public class SphSettings : DomainObject
    {
        private string m_sqlLocalDbName;
        private string m_iisExpressDirectory;
        private string m_rabbitMqDirectory;
        private string m_elasticSearchHome;
        private string m_javaHome;
        private string m_rabbitMqPassword;
        private string m_rabbitMqUserName;
        private int? m_websitePort;
        private string m_rabbitMqHost;
        private int? m_rabbitMqPort;
        private int? m_rabbitMqManagementPort;
        private int? m_loggerWebSocketPort;
        private string m_applicationName;
        private string m_projectDirectory;

        public string ApplicationName
        {
            get { return m_applicationName; }
            set
            {
                m_applicationName = value;
                OnPropertyChanged();
            }
        }


        public string SqlLocalDbName
        {
            get
            {
                return string.IsNullOrWhiteSpace(m_sqlLocalDbName) ?
                            @"Projects" :
                            m_sqlLocalDbName;
            }
            set
            {
                m_sqlLocalDbName = value;
                OnPropertyChanged();
            }
        }

        public string IisExpressExecutable
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
                    ".\\rabbitmq_server"
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

        public string ElasticSearchJar
        {
            get
            {
                return string.IsNullOrWhiteSpace(m_elasticSearchHome) ?
                    @".\elasticsearch" :
                    m_elasticSearchHome;
            }
            set { m_elasticSearchHome = value; }
        }

        public int? WebsitePort
        {
            get { return m_websitePort; }
            set
            {
                m_websitePort = value;
                OnPropertyChanged();
            }
        }



        public string RabbitMqHost
        {
            get { return m_rabbitMqHost; }
            set
            {
                m_rabbitMqHost = value;
                OnPropertyChanged();
            }
        }

        public int? RabbitMqPort
        {
            get { return m_rabbitMqPort; }
            set
            {
                m_rabbitMqPort = value;
                OnPropertyChanged();
            }
        }

        public int? RabbitMqManagementPort
        {
            get { return m_rabbitMqManagementPort; }
            set
            {
                m_rabbitMqManagementPort = value;
                OnPropertyChanged();
            }
        }

        public int? LoggerWebSocketPort
        {
            get { return m_loggerWebSocketPort; }
            set
            {
                m_loggerWebSocketPort = value;
                OnPropertyChanged();
            }
        }

        public string ProjectDirectory
        {
            get
            {
                if (string.IsNullOrWhiteSpace(m_projectDirectory)
                    || !Directory.Exists(m_projectDirectory))
                    m_projectDirectory = Path.Combine(Directory.GetCurrentDirectory(), "..\\");
                return m_projectDirectory;
            }
            set
            {
                m_projectDirectory = value;

                OnPropertyChanged();
            }
        }

        public void LoadDefault()
        {

            this.ProjectDirectory = ".".TranslatePath();
            this.WebsitePort = 4436;
            this.RabbitMqManagementPort = 15672;
            this.RabbitMqHost = "localhost";
            this.RabbitMqPort = 5672;
            this.RabbitMqDirectory = ".\\rabbitmq_server";
            this.RabbitMqPassword = "guest";
            this.RabbitMqUserName = "guest";
            this.LoggerWebSocketPort = 50230;
            this.ApplicationName = "";
            this.IisExpressExecutable = ".\\IIS Express\\iisexpress.exe";

            if (!File.Exists(this.IisExpressExecutable.TranslatePath()))
                this.IisExpressExecutable = null;

            if (Directory.Exists(".\\elasticsearch".TranslatePath()))
            {
                var es = Directory.GetFiles(".\\elasticsearch\\lib\\".TranslatePath(), "elasticsearch-*.jar")
                    .SingleOrDefault();
                this.ElasticSearchJar = es;
            }


        }
    }
}
