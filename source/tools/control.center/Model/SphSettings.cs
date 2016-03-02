using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Bespoke.Sph.ControlCenter.Helpers;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.ControlCenter.Model
{
    [XmlType("SphSettings", Namespace = Strings.DEFAULT_NAMESPACE)]
    public class SphSettings : DomainObject
    {
        private string m_applicationName;
        private string m_elasticsearchClusterName;
        private string m_elasticsearchNodeName;
        private int m_elasticsearchIndexNumberOfShards;
        private int m_elasticsearchIndexNumberOfReplicas;
        private string m_updateUri;


        public override string ToString()
        {
            return this.ToNormalizedJsonString();
        }

        public void Save()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../project.json");
            File.WriteAllText(path, this.ToNormalizedJsonString(), Encoding.UTF8);
            // save the environment variable

            var root = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName);
            var eslib = Path.Combine(root.FullName, "elasticsearch\\lib\\");
            var jar = Directory.GetFiles(eslib, "elasticsearch-*.jar").Single();
            SetEnvironmentVariable(nameof(ElasticSearchJar), jar);

            SetEnvironmentVariable("HOME", this.Home);
            SetEnvironmentVariable(nameof(ElasticSearchJar), this.ElasticSearchJar);
            SetEnvironmentVariable(nameof(ElasticsearchClusterName), this.ElasticsearchClusterName);
            SetEnvironmentVariable(nameof(ElasticsearchNodeName), this.ElasticsearchNodeName);
            SetEnvironmentVariable(nameof(ElasticsearchIndexNumberOfReplicas), this.ElasticsearchIndexNumberOfReplicas);
            SetEnvironmentVariable(nameof(ElasticsearchIndexNumberOfShards), this.ElasticsearchIndexNumberOfShards);

            SetEnvironmentVariable(nameof(ElasticsearchHttpPort), this.ElasticsearchHttpPort,9200);
            SetEnvironmentVariable(nameof(LoggerWebSocketPort), this.LoggerWebSocketPort, 50238);
            SetEnvironmentVariable(nameof(WebsitePort), this.WebsitePort, 50230);
            SetEnvironmentVariable(nameof(RabbitMqBase), this.RabbitMqBase);
            SetEnvironmentVariable(nameof(RabbitMqDirectory), this.RabbitMqDirectory);
            SetEnvironmentVariable(nameof(RabbitMqManagementPort), this.RabbitMqManagementPort, 15672);
            SetEnvironmentVariable(nameof(SqlLocalDbName), this.SqlLocalDbName, "Projects");
            SetEnvironmentVariable(nameof(IisExpressExecutable), this.IisExpressExecutable);
            SetEnvironmentVariable(nameof(RabbitMqUserName), this.RabbitMqUserName, "guest");
            SetEnvironmentVariable(nameof(RabbitMqPassword), this.RabbitMqPassword, "guest");
            SetEnvironmentVariable(nameof(RabbitMqHost), this.RabbitMqHost, "localhost");

            var sb = new StringBuilder();
            var variables = Environment.GetEnvironmentVariables();
            foreach (var key in variables.Keys)
            {
                if (!$"{key}".StartsWith("RX_")) continue;
                sb.AppendLine($"SET {key}={variables[key]}");
            }

            var bat = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "../env.bat");
            File.WriteAllText(bat, sb.ToString(), Encoding.UTF8);

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

        [JsonIgnore]
        public int ElasticsearchHttpPort
        {
            get { return GetEnvironmentVariableInt32("ElasticsearchHttpPort") ?? 9200; }
            set
            {
                SetEnvironmentVariable("ElasticsearchHttpPort", value.ToString());
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
                return GetEnvironmentVariable(nameof(SqlLocalDbName)) ?? "Projects";
            }
            set
            {
                SetEnvironmentVariable(nameof(SqlLocalDbName), value);
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public string IisExpressExecutable
        {
            get
            {

                var path = GetEnvironmentVariable(nameof(IisExpressExecutable)) ?? Home + @"\IIS Express\iisexpress.exe";
                return path.TranslatePath();
            }
            set
            {
                SetEnvironmentVariable(nameof(IisExpressExecutable), value);
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public string RabbitMqDirectory
        {
            get
            {
                var path = GetEnvironmentVariable(nameof(RabbitMqDirectory)) ?? ".\\rabbitmq_server";
                return path.TranslatePath();
            }
            set
            {
                SetEnvironmentVariable(nameof(RabbitMqDirectory), value);
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public string RabbitMqUserName
        {
            get
            {
                return GetEnvironmentVariable(nameof(RabbitMqUserName)) ?? "guest";
            }
            set
            {
                SetEnvironmentVariable(nameof(RabbitMqUserName), value, "guest");
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public string RabbitMqPassword
        {
            get
            {
                return GetEnvironmentVariable(nameof(RabbitMqPassword)) ?? "guest";
            }
            set
            {
                SetEnvironmentVariable(nameof(RabbitMqPassword), value, "guest");
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public string JavaHome
        {
            get { return Environment.GetEnvironmentVariable("JAVA_HOME"); }
            set { Environment.SetEnvironmentVariable("JAVA_HOME", value); }
        }

        [JsonIgnore]
        public string ElasticSearchJar
        {
            get
            {
                var root = Directory.GetParent(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).FullName);
                var eslib = Path.Combine(root.FullName, "elasticsearch\\lib\\");
                return Directory.GetFiles(eslib, "elasticsearch-*.jar").Single();
            }
            set { SetEnvironmentVariable(nameof(ElasticSearchJar), value); }
        }

        [JsonIgnore]
        public int? WebsitePort
        {
            get { return GetEnvironmentVariableInt32(nameof(WebsitePort)) ?? 50230; }
            set
            {
                SetEnvironmentVariable(nameof(WebsitePort), value ?? 50230, 50230);
                OnPropertyChanged();
            }
        }


        [JsonIgnore]
        public string RabbitMqHost
        {
            get { return GetEnvironmentVariable(nameof(RabbitMqHost)) ?? "localhost"; }
            set
            {
                SetEnvironmentVariable(nameof(RabbitMqHost), value, "localhost");
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public int? RabbitMqPort
        {
            get { return GetEnvironmentVariableInt32(nameof(RabbitMqPort)) ?? 5672; }
            set
            {
                SetEnvironmentVariable(nameof(RabbitMqPort), value.ToString());
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public int? RabbitMqManagementPort
        {
            get { return GetEnvironmentVariableInt32(nameof(RabbitMqManagementPort)) ?? 15672; }
            set
            {
                SetEnvironmentVariable(nameof(RabbitMqManagementPort), value.ToString());
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public int? LoggerWebSocketPort
        {
            get { return GetEnvironmentVariableInt32(nameof(LoggerWebSocketPort)) ?? 50238; }
            set
            {
                SetEnvironmentVariable(nameof(LoggerWebSocketPort), value.ToString());
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public string Home
        {
            get
            {
                var home = GetEnvironmentVariable("HOME");
                if (string.IsNullOrWhiteSpace(ApplicationName))
                    throw new InvalidOperationException("Cannot get HOME for empty ApplicationName");

                if (string.IsNullOrWhiteSpace(home))
                {
                    home = Directory.GetCurrentDirectory();
                    SetEnvironmentVariable("HOME", home);

                }
                return home;
            }
        }

        public string UpdateUri
        {
            get { return m_updateUri; }
            set
            {
                m_updateUri = value;
                OnPropertyChanged();
            }
        }

        [JsonIgnore]
        public string RabbitMqBase
        {
            get { return GetEnvironmentVariable(nameof(RabbitMqBase)) ?? "rabbitmq_base"; }
            set
            {
                SetEnvironmentVariable(nameof(RabbitMqBase), value);
                OnPropertyChanged();
            }
        }

        public void LoadDefault()
        {

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

        private string ApplicationNameToUpper => $"{this.ApplicationName}".ToUpper();

        public string GetEnvironmentVariable(string setting)
        {
            var process = Environment.GetEnvironmentVariable($"RX_{ApplicationNameToUpper}_{setting}", EnvironmentVariableTarget.Process);
            if (!string.IsNullOrWhiteSpace(process)) return process;

            var user = Environment.GetEnvironmentVariable($"RX_{ApplicationNameToUpper}_{setting}", EnvironmentVariableTarget.User);
            if (!string.IsNullOrWhiteSpace(user)) return user;

            return Environment.GetEnvironmentVariable($"RX_{ApplicationNameToUpper}_{setting}", EnvironmentVariableTarget.Machine);
        }
        private void SetEnvironmentVariable(string setting, object value, object defaultValue = null)
        {
            if ($"{value}".Contains("control.center"))
                throw new InvalidOperationException($"You cannot set \"RX_{ApplicationNameToUpper}_{setting}\" variable to \"{value}\"");

            if (null != defaultValue && defaultValue == value) return;

            Environment.SetEnvironmentVariable($"RX_{ApplicationNameToUpper}_{setting}", $"{value}", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable($"RX_{ApplicationNameToUpper}_{setting}", $"{value}", EnvironmentVariableTarget.User);
        }
        private int? GetEnvironmentVariableInt32(string setting)
        {
            var val = GetEnvironmentVariable(setting);
            int intValue;
            if (int.TryParse(val, out intValue)) return intValue;
            return null;
        }
    }
}
