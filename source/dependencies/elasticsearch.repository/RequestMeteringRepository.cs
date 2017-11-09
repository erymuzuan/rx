using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Bespoke.Sph.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public class RequestMeteringRepository : RepositoryWithNamingStrategy, IMeteringRepository
    {
        private readonly HttpClient m_client;

        public RequestMeteringRepository(string host, string baseIndexName)
        {
            this.BaseIndexName = baseIndexName;
            m_client = new HttpClient { BaseAddress = new Uri(host) };
        }

        public RequestMeteringRepository() : this(EsConfigurationManager.Host, EsConfigurationManager.SystemIndex)
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

        public async Task<LoadOperation<HttpRequestLog>> SearchAsync(Filter[] filters)
        {
            var alias = GetIndexAlias(filters);
            var url = $"{EsConfigurationManager.Index}_logs/{alias}/_search";
            using (var client = new HttpClient { BaseAddress = new Uri(EsConfigurationManager.LogHost) })
            {

                var query = default(Entity).CompileToElasticsearchQueryDsl(new QueryDsl(filters));
                var response = await client.PostAsync(url, new StringContent(query));
                var lo = await response.ReadContentAsLoadOperationAsync<HttpRequestLog>();
                return lo;


            }
        }
    }
}
