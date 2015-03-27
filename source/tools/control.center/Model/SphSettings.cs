using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Bespoke.Sph.ControlCenter.Helpers;
using Bespoke.Sph.Domain;

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
        private string m_elasticsearchClusterName;
        private string m_elasticsearchNodeName;
        private int m_elasticsearchIndexNumberOfShards;
        private int m_elasticsearchIndexNumberOfReplicas;
        private int m_elasticsearchHttpPort;


        public override string ToString()
        {
            return this.ToNormalizedJsonString();
        }

        public void Save()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../project.json");
            File.WriteAllText(path, this.ToNormalizedJsonString(), Encoding.UTF8);
        }

        public static SphSettings Load()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../project.json");
            if (!File.Exists(path))
                return null;

            try
            {

                var settings = File.ReadAllText(path).DeserializeFromJson<SphSettings>();
                return settings;
            }
            catch
            {
                return null;
            }
        }

        public int ElasticsearchHttpPort
        {
            get { return m_elasticsearchHttpPort; }
            set
            {
                m_elasticsearchHttpPort = value;
                OnPropertyChanged();
            }
        }

        public int ElasticsearchIndexNumberOfReplicas
        {
            get { return m_elasticsearchIndexNumberOfReplicas; }
            set
            {
                m_elasticsearchIndexNumberOfReplicas = value;
                OnPropertyChanged();
            }
        }

        public int ElasticsearchIndexNumberOfShards
        {
            get { return m_elasticsearchIndexNumberOfShards; }
            set
            {
                m_elasticsearchIndexNumberOfShards = value;
                OnPropertyChanged();
            }
        }

        public string ElasticsearchNodeName
        {
            get { return m_elasticsearchNodeName; }
            set
            {
                m_elasticsearchNodeName = value;
                OnPropertyChanged();
            }
        }

        public string ElasticsearchClusterName
        {
            get { return m_elasticsearchClusterName; }
            set
            {
                m_elasticsearchClusterName = value;
                OnPropertyChanged();
            }
        }

        [Required]
        [MinLength(3, ErrorMessage = "You'll need at least 3 chars")]
        [RegularExpression("[a-zA-z]{3,9}", ErrorMessage = "must be alpahnumeric only")]
        public string ApplicationName
        {
            get { return m_applicationName; }
            set
            {
                m_applicationName = value;
                OnPropertyChanged();
            }
        }

        [Required]
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
                    m_projectDirectory = Directory.GetCurrentDirectory();
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

        public void SetElasticsearchConfig()
        {
            if (Directory.Exists(".\\elasticsearch".TranslatePath()))
            {
                var es = ".\\elasticsearch\\config\\elasticsearch.yml".TranslatePath();
                var config = File.ReadAllText(es);

                config = config
                    .Replace("cluster.name: ElasticsearchClusterName", "cluster.name: " + ElasticsearchClusterName)
                    .Replace("node.name: \"ElasticsearchNodeName\"", "node.name: \"" + ElasticsearchNodeName + "\"")
                    .Replace("index.number_of_replicas: ElasticsearchIndexNumberOfReplicas", "index.number_of_replicas: " + ElasticsearchIndexNumberOfReplicas)
                    .Replace("index.number_of_shards: ElasticsearchIndexNumberOfShards", "index.number_of_shards: " + ElasticsearchIndexNumberOfShards)
                    .Replace("http.port: ElasticsearchHttpPort", "http.port: " + ElasticsearchHttpPort)
                    ;

                File.WriteAllText(es, config);
            }
        }
    }
}
