using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public class RequestMeteringRepository : RepositoryWithNamingStrategy, IMeteringRepository, IDisposable
    {
        private readonly HttpClient m_client;

        public RequestMeteringRepository(string host, string baseIndexName)
        {
            this.BaseIndexName = baseIndexName;
            m_client = new HttpClient { BaseAddress = new Uri(host) };
        }

        public RequestMeteringRepository() : this(EsConfigurationManager.Host, ConfigurationManager.ApplicationName.ToLower() + "_logs")
        {

        }

        public void Log(HttpRequestLog request)
        {
            var setting = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var json = JsonConvert.SerializeObject(request, setting);
            var content = new StringContent(json);
            var index = GetIndexName();
            m_client.PostAsync(index + "/request_log", content)
                .ContinueWith(_ => { })
                .ConfigureAwait(false);
        }

        public async Task<LoadOperation<HttpRequestLog>> SearchAsync(QueryDsl query)
        {
            var alias = GetIndexAlias(query.Filters.ToArray());
            var url = $"{alias}/request_log/_search";
            
            var dsl = default(Entity).CompileToElasticsearchQueryDsl(query);
            var response = await m_client.PostAsync(url, new StringContent(dsl));
            var lo = await response.ReadContentAsLoadOperationAsync<HttpRequestLog>(query);
            return lo;

        }

        public void Dispose()
        {
            m_client?.Dispose();
        }
    }
}
