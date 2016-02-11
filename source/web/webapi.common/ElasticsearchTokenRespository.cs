using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.WebApi
{
    public class ElasticsearchTokenRespository : ITokenRepository
    {
        public async Task SaveAsync(AccessToken token)
        {
            var content = new StringContent(token.ToJson());
            var index = ConfigurationManager.ElasticSearchSystemIndex;
            var url = $"{ConfigurationManager.ElasticSearchHost}/{index}/access_token/{token.WebId}";

            using (var client = new HttpClient())
            {
                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
            }
        }

        public Task<LoadOperation<AccessToken>> LoadAsync(string user, DateTime expiry, int page = 1, int size = 20)
        {
            throw new NotImplementedException();
        }

        public async Task<LoadOperation<AccessToken>> LoadAsync(DateTime expiry, int page = 1, int size = 20)
        {

            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var seconds = Math.Round((expiry.ToUniversalTime() - unixEpoch).TotalSeconds);
            var query = $@"
                {{
                    ""query"": {{
                        ""range"": {{
                           ""exp"": {{ 
                                ""from"":{seconds}
                            }}
                        }}
                    }}, 
                  ""from"": {(page - 1) * size},
                  ""size"": {size}
                }}
            ";

            var request = new StringContent(query);
            var url = $"{ConfigurationManager.ElasticSearchSystemIndex}/access_token/_search";
            string json;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.PostAsync(url, request);
                response.EnsureSuccessStatusCode();


                var content = response.Content as StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es " + request);
                json = await content.ReadAsStringAsync();

            }

            var jo = JObject.Parse(json);
            var items = from hit in jo.SelectToken("$.hits.hits")
                        let source = hit.SelectToken("_source")
                        let token = JsonConvert.DeserializeObject<AccessToken>(source.ToString())
                        select token;

            var lo = new LoadOperation<AccessToken>();
            lo.ItemCollection.AddRange(items);
            return lo;
        }

        public async Task<AccessToken> LoadOneAsync(string id)
        {
            var url = $"{ConfigurationManager.ElasticSearchSystemIndex}/access_token/{id}";
            string json;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.GetAsync(url);
                if (!response.IsSuccessStatusCode) return null;


                var content = response.Content as StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es ");
                json = await content.ReadAsStringAsync();

            }

            var jo = JObject.Parse(json);
            var token = jo.SelectToken("$._source").ToString().DeserializeFromJson<AccessToken>();

            return token;
        }

        public async Task RemoveAsync(string id)
        {
            var url = $"{ConfigurationManager.ElasticSearchSystemIndex}/access_token/{id}";
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);
                var response = await client.DeleteAsync(url);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}