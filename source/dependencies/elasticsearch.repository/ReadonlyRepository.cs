using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public class ReadOnlyRepository : IReadOnlyRepository
    {
        private readonly HttpClient m_client;
        public ReadOnlyRepository(string host)
        {
            m_client = new HttpClient { BaseAddress = new Uri(host) };
        }

        public ReadOnlyRepository() : this(EsConfigurationManager.Host)
        {
        }
        
        public async Task TruncateAsync(string entity)
        {
            var message = new HttpRequestMessage(HttpMethod.Delete,
                $"{EsConfigurationManager.Index}/{entity.ToLowerInvariant()}/_query")
            {
                Content = new StringContent(
@"{
       ""query"": {
          ""match_all"": {}
       }
    }")
            };
            await m_client.SendAsync(message);



        }

        public async Task CleanAsync(string entity)
        {
            await m_client.DeleteAsync(
                $"{EsConfigurationManager.Index}/_mapping/{entity.ToLowerInvariant()}");
        }

        public async Task CleanAsync()
        {

            var response = await m_client.DeleteAsync(ConfigurationManager.ApplicationName);
            Console.WriteLine($@"DELETE {ConfigurationManager.ApplicationName} index : {response.StatusCode}");
            await m_client.PutAsync(ConfigurationManager.ApplicationName, new StringContent(""));
        }

        public async Task<LoadOperation<Entity>> SearchAsync(string[] entities, Filter[] filters = null, Sort[] sorts = null, int skip = 0, int size = 20)
        {
            var dsl = new QueryDsl(filters, sorts)
            {
                Skip = skip,
                Size = size
            };
            var types = entities.ToString(",", x => x.ToLowerInvariant());
            var query = (default(EntityDefinition)).CompileToElasticsearchQueryDsl(dsl);
            var request = new StringContent(query);
            var index = EsConfigurationManager.Index;
            var url = $"{index}/{types}/_search";

            var response = await m_client.PostAsync(url, request);
            var lo = await response.ReadContentAsLoadOperationAsync<Entity>();

            return lo;

        }

        public async Task<int> GetCountAsync(string entity)
        {
            var json = await m_client.GetStringAsync($"{EsConfigurationManager.Index}/{entity.ToLowerInvariant()}/_count");
            var jo = JObject.Parse(json);
            return jo.SelectToken("$.count").Value<int>();
        }
    }
}