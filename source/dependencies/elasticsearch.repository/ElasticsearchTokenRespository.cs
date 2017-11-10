using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Extensions;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public class ElasticsearchTokenRespository : ITokenRepository
    {
        private readonly ITokenRepository[] m_failOverRepositories;
        private readonly HttpClient m_client;
        public ElasticsearchTokenRespository(string host, ITokenRepository[] failOverRepositories = null)
        {
            m_failOverRepositories = failOverRepositories ?? Array.Empty<ITokenRepository>();
            m_client = new HttpClient { BaseAddress = new Uri(host) };
        }
        public ElasticsearchTokenRespository(ITokenRepository[] failOverRepositories) : this(EsConfigurationManager.Host, failOverRepositories)
        {
        }

        public ElasticsearchTokenRespository() : this(EsConfigurationManager.Host)
        {
        }


        public async Task SaveAsync(AccessToken token)
        {
            var content = new StringContent(token.ToJson());
            var index = EsConfigurationManager.SystemIndex;
            var url = $"{index}/access_token/{token.WebId}";

            var response = await m_client.PostAsync(url, content);
            await AddToFailOversAsync(token);
            response.EnsureSuccessStatusCode();
        }

        public Task<LoadOperation<AccessToken>> LoadAsync(string user, DateTime expiry, int page = 1, int size = 20)
        {
            var unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var seconds = Math.Round((expiry.ToUniversalTime() - unixEpoch).TotalSeconds);
            var query = $@"
{{
    ""query"": {{
        ""bool"": {{
            ""must"": [
               {{
                  ""term"": {{
                     ""user"": {{
                        ""value"": ""{user}""
                     }}
                  }}
               }},
               {{
                   ""range"": {{
                      ""exp"": {{
                         ""from"": {seconds}
                      }}
                   }}
               }}
            ]
        }}
    }},
    ""from"": {(page - 1) * size},
    ""size"": {size}
}}
";

            return ExecuteSearchAsync(query);
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

            var lo = await ExecuteSearchAsync(query).ConfigureAwait(false);
            lo.PageSize = size;
            lo.CurrentPage = page;
            return lo;
        }


        public async Task<LoadOperation<AccessToken>> SearchAsync(string query, int page = 1, int size = 20)
        {
            var equery = $@"
                {{
                   ""query"": {{
                          ""query_string"": {{
                             ""default_field"": ""_all"",
                             ""query"": ""{query}""
                          }}
                       }}, 
                  ""from"": {(page - 1) * size},
                  ""size"": {size}
                }}
            ";
            var lo = await ExecuteSearchAsync(equery).ConfigureAwait(false);
            lo.PageSize = size;
            lo.CurrentPage = page;
            return lo;
        }

        private async Task<LoadOperation<AccessToken>> ExecuteSearchAsync(string query)
        {
            var request = new StringContent(query);
            var url = $"{EsConfigurationManager.SystemIndex}/access_token/_search";

            var pr = await Policy.Handle<HttpRequestException>()
                            .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(Math.Pow(2, x) * 500))
                            .ExecuteAndCaptureAsync(async () => await m_client.PostAsync(url, request));

            if (null != pr.FinalException)
                throw pr.FinalException;

            var response = pr.Result;
            response.EnsureSuccessStatusCode();

            if (!(response.Content is StreamContent content)) throw new Exception("Cannot execute query on es " + request);
            var json = await content.ReadAsStringAsync();

            var jo = JObject.Parse(json);
            var items = from hit in jo.SelectToken("$.hits.hits")
                        let source = hit.SelectToken("_source")
                        let token = JsonConvert.DeserializeObject<AccessToken>(source.ToString())
                        select token;

            var lo = new LoadOperation<AccessToken>();
            lo.ItemCollection.AddRange(items);
            lo.TotalRows = jo.SelectToken("$.hits.total").Value<int>();
            return lo;
        }

        public async Task<AccessToken> LoadOneAsync(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentNullException(nameof(subject));

            var url = $"{EsConfigurationManager.SystemIndex}/access_token/{subject}";

            var pr = await Policy.Handle<HttpRequestException>()
                            .WaitAndRetryAsync(5, x => TimeSpan.FromMilliseconds(Math.Pow(2, x) * 500))
                            .ExecuteAndCaptureAsync(async () => await m_client.GetAsync(url));
            if (null != pr.FinalException)
                return await LoadFromFailOversAsync(subject);

            var response = pr.Result;
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return await LoadFromFailOversAsync(subject);
            }

            var jo = await response.ReadContentAsJsonAsync();
            var token = jo.SelectToken("$._source").ToString().DeserializeFromJson<AccessToken>();

            return token;
        }

        private async Task<AccessToken> LoadFromFailOversAsync(string subject)
        {
            var tasks = from fo in m_failOverRepositories
                        select fo.LoadOneAsync(subject);
            var winning = await Task.WhenAny(tasks);

            return await winning;

        }
        private async Task RemoveFromFailOversAsync(string subject)
        {
            var tasks = from fo in m_failOverRepositories
                        select fo.RemoveAsync(subject);
            await Task.WhenAll(tasks);

        }
        private async Task AddToFailOversAsync(AccessToken token)
        {
            var tasks = from fo in m_failOverRepositories
                        select fo.SaveAsync(token);
            await Task.WhenAll(tasks);
        }

        public async Task RemoveAsync(string subject)
        {
            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentNullException(nameof(subject));

            var url = $"{EsConfigurationManager.SystemIndex}/access_token/{subject}";
            var pr = await Policy.Handle<HttpRequestException>()
                            .WaitAndRetryAsync(10, x => TimeSpan.FromMilliseconds(Math.Pow(2, x) * 500))
                            .ExecuteAndCaptureAsync(async () => await m_client.DeleteAsync(url));
            if (null != pr.FinalException)
                throw pr.FinalException;

            var response = pr.Result;
            await RemoveFromFailOversAsync(subject);
            response.EnsureSuccessStatusCode();

        }

    }
}
