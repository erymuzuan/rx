using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public class RequestMeteringRepository : RepositoryWithNamingStrategy, IMeteringRepository, IDisposable
    {
        private readonly JObject m_mapping;

        public RequestMeteringRepository(string host, string baseIndexName) : base(host, baseIndexName)
        {
            m_mapping = JObject.Parse(Properties.Resources.RequestLogMapping);
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
            this.Client.PostAsync(index + "/request_log", content)
                .ContinueWith(_ => { })
                .ConfigureAwait(false);

            base.CreateAliasesQueryAsync(DateTime.Today)
                .ContinueWith(_ => { })
                .ConfigureAwait(false);
        }

        public async Task<LoadOperation<HttpRequestLog>> SearchAsync(QueryDsl query)
        {

            var alias = GetIndexAlias(query.Filters.ToArray(), nameof(HttpRequestLog.Time));
            var url = $"{alias}/request_log/_search";

            var dsl = query.CompileToElasticsearchQuery<HttpRequestLog>(m_mapping);
            var response = await this.Client.PostAsync(url, new StringContent(dsl));
            var lo = await response.ReadContentAsLoadOperationAsync<HttpRequestLog>(query);
            return lo;
        }

      
    }
}
