using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Bespoke.Sph.Extensions;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public class ReadonlyRepository : IReadonlyRepository
    {
        private readonly HttpClient m_client;
        public ReadonlyRepository(string host)
        {
            m_client = new HttpClient { BaseAddress = new Uri(host) };
        }

        public ReadonlyRepository() : this(EsConfigurationManager.ElasticSearchHost)
        {

        }
        public async Task TruncateAsync(EntityDefinition ed)
        {
            var message = new HttpRequestMessage(HttpMethod.Delete,
                $"{EsConfigurationManager.ElasticSearchIndex}/{ed.Name.ToLowerInvariant()}/_query")
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

        public async Task CleanAsync(EntityDefinition ed)
        {
            await m_client.DeleteAsync(
                $"{EsConfigurationManager.ElasticSearchIndex}/_mapping/{ed.Name.ToLowerInvariant()}");
        }

        public async Task CleanAsync()
        {

            var response = await m_client.DeleteAsync(ConfigurationManager.ApplicationName);
            Console.WriteLine($@"DELETE {ConfigurationManager.ApplicationName} index : {response.StatusCode}");
            await m_client.PutAsync(ConfigurationManager.ApplicationName, new StringContent(""));
        }

        public async Task<object> SearchAsync(string types, Filter[] filters)
        {

            /*
                var query = @"
                    {
                        ""query"": {
                            ""query_string"": {
                               ""default_field"": ""_all"",
                               ""query"": """ + text + @"""
                            }
                        },
                       ""highlight"": {
                            ""fields"": {
                                " + records + @"
                            }
                        },  
                      ""from"": 0,
                      ""size"": 20
                    }
                ";

                var request = new StringContent(query);
                var url = $"{ConfigurationManager.ElasticSearchIndex}/{types}/_search";

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                    var response = await client.PostAsync(url, request);
                    var content = response.Content as StreamContent;
                    if (null == content) throw new Exception("Cannot execute query on es " + request);
                    var result = await content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                        throw new SearchException("Cannot execute query for :" + text) { Query = query, Result = result };
                    return Json(result);

                }*/
            //--
            /*        /*
        */
            var query = (default(EntityDefinition)).GetFilterDsl(filters);
            var request = new StringContent(query);
            var index = EsConfigurationManager.ElasticSearchIndex;
            var url = $"{index}/{types.ToLowerInvariant()}/_search";
            
            var response = await m_client.PostAsync(url, request);
            var lo = await response.ReadContentAsLoadOperationAsync<Entity>();

            return lo;

        }

        public async Task<int> GetCountAsync(string entity)
        {

            var json = await m_client.GetStringAsync($"{EsConfigurationManager.ElasticSearchIndex}/{entity.ToLowerInvariant()}/_count");
            var jo = JObject.Parse(json);
            return jo.SelectToken("$.count").Value<int>();
        }
    }
}