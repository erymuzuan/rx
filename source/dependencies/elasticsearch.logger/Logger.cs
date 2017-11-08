using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.ElasticSearchLogger
{
    [Export(typeof(ILogger))]
    public class Logger : RepositoryWithNamingStrategy, ILogger
    {
        public Severity TraceSwitch { get; set; } = Severity.Info;
        private readonly HttpClient m_client;

        public Logger(string host, string baseIndexName, IndexNamingStrategy namingStrategy)
        {
            this.IndexNamingStrategy = namingStrategy;
            this.BaseIndexName = baseIndexName;
            if (null == m_client)
                m_client = new HttpClient { BaseAddress = new Uri(host) };
        }
        public Logger(string host, string baseIndexName) : this(EsConfigurationManager.LogHost, baseIndexName, IndexNamingStrategy.Daily)
        {
        }
        public Logger() : this(EsConfigurationManager.LogHost, "logs", IndexNamingStrategy.Daily)
        {
        }

        public async Task LogAsync(LogEntry entry)
        {
            if ((int)entry.Severity < (int)this.TraceSwitch) return;

            if (string.IsNullOrWhiteSpace(entry.Id))
                entry.Id = Strings.GenerateId();

            var content = GetJsonContent(entry);
            var index = GetIndexName();
            var url = $"{index}/log/{entry.Id}";

            var response = await m_client.PutAsync(url, content);
            Debug.WriteLine("{0}=>{1}", url, response.StatusCode);

        }
        public void Log(LogEntry entry)
        {
            this.LogAsync(entry).ContinueWith(_ => { }).ConfigureAwait(false);
        }
        private static StringContent GetJsonContent(LogEntry entry)
        {
            var setting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            setting.Converters.Add(new StringEnumConverter());
            setting.Formatting = Formatting.Indented;
            setting.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var json = JsonConvert.SerializeObject(entry, setting);
            var content = new StringContent(json);
            return content;
        }
    }
}