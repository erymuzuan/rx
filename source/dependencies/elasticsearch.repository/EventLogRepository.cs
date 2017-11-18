using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public class EventLogRepository : ILoggerRepository, IDisposable
    {
        private readonly HttpClient m_client;
        public string Index { get; }

        public EventLogRepository() : this(EsConfigurationManager.LogHost, ConfigurationManager.ApplicationName.ToLower() + "_logs")
        {
        }
        public EventLogRepository(string host, string index)
        {
            Index = index;
            m_client = new HttpClient { BaseAddress = new Uri(host) };
        }
        public Task<LogEntry> LoadOneAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<LoadOperation<LogEntry>> SearchAsync(QueryDsl dsl)
        {
            var query = (default(EntityDefinition)).CompileToElasticsearchQueryDsl(dsl);
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
