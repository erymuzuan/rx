using System;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.ElasticSearchLogger
{
    [Export(typeof(ILogger))]
    public class Logger : ILogger
    {
        public Severity TraceSwitch { get; set; } = Severity.Log;
        private readonly HttpClient m_client;

        public Logger()
        {
            if (null == m_client)
                m_client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticsearchLogHost) };
        }

        public async Task LogAsync(LogEntry entry)
        {
            if ((int)entry.Severity < (int)this.TraceSwitch) return;

            if (string.IsNullOrWhiteSpace(entry.Id))
                entry.Id = Strings.GenerateId();

            var content = GetJsonContent(entry);

            var date = DateTime.Now.ToString(ConfigurationManager.RequestLogIndexPattern);
            var index = $"{ConfigurationManager.ElasticSearchIndex}_logs_{date}";
            var url = $"{index}/log/{entry.Id}";

            var response = await m_client.PutAsync(url, content);
            Debug.WriteLine("{0}=>{1}", url, response.StatusCode);

        }
        public void Log(LogEntry entry)
        {
            this.LogAsync(entry).ContinueWith(_ => { });
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