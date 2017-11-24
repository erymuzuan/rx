using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public class EventLogRepository : ILoggerRepository, IDisposable
    {
        private readonly HttpClient m_client;
        private string Index { get; }
        private readonly JObject m_mapping ;

        public EventLogRepository() : this(EsConfigurationManager.LogHost, ConfigurationManager.ApplicationName.ToLower() + "_logs")
        {
        }
        public EventLogRepository(string host, string index)
        {
            Index = index;
            m_client = new HttpClient { BaseAddress = new Uri(host) };
            m_mapping = JObject.Parse(Properties.Resources.LogMapping);
        }
        public Task<LogEntry> LoadOneAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<LoadOperation<LogEntry>> SearchAsync(QueryDsl dsl)
        {
            var query = dsl.CompileToElasticsearchQuery<LogEntry>(m_mapping);
            var request = new StringContent(query);
            var url = $"{Index}/log/_search";

            var response = await m_client.PostAsync(url, request);
            var lo = await response.ReadContentAsLoadOperationAsync<LogEntry>(dsl);

            return lo;
        }

        public void Dispose()
        {
            m_client?.Dispose();
        }
    }
}
