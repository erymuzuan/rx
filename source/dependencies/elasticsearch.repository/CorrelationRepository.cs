using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public class CorrelationRepository : ICorrelationRepository
    {

        private readonly HttpClient m_client;

        public CorrelationRepository()
        {
            if (null == m_client)
                m_client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) };
        }

        public CorrelationRepository(HttpMessageHandler httpMessageHandler, bool disposeHandler)
        {
            if (null == m_client)
                m_client = new HttpClient(httpMessageHandler, disposeHandler) { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) };
        }
        public async Task<T> GetInstanceAsync<T>(WorkflowDefinition wd, string correlationName, string correlationValue) where T : Workflow
        {
            var q16 = new
            {
                query = new
                {
                    filtered = new
                    {
                        @bool = new
                        {
                            must = new object[]
                        {
                                new
                                {
                                    term =new
                                    {
                                        wdid = wd.Id
                                    }
                                },
                                new {
                                    term =new
                                    {
                                        name = correlationName
                                    }
                                },
                                new {
                                    term =new
                                    {
                                        value = correlationValue
                                    }
                                }
                        }
                        }
                    }
                }
            };

            var q17 = new
            {
                filter = new
                {
                    @bool = new
                    {
                        must = new object[]
                            {
                                new
                                {
                                    term =new
                                    {
                                        wdid = wd.Id
                                    }
                                },
                                new {
                                    term =new
                                    {
                                        name = correlationName
                                    }
                                },
                                new {
                                    term =new
                                    {
                                        value = correlationValue
                                    }
                                }
                            }
                    }
                }

            };
            // NOTE: different version of elasticsearch does require different query, so we have 1.7.5 is the default and 1.6 is supported version
            var wid = await ExecuteElasticsearchQueryAsync(q17.ToJson()) ??
                      await ExecuteElasticsearchQueryAsync(q16.ToJson());

            var context = new SphDataContext();
            var instance = await context.LoadOneAsync<Workflow>(x => x.Id == wid.ToString());
            await instance.LoadWorkflowDefinitionAsync();
            return (T)instance;

        }

        private async Task<string> ExecuteElasticsearchQueryAsync(string query)
        {
            var url = $"{ConfigurationManager.ElasticSearchIndex}/correlationset/_search";
            var esresult = await m_client.PostAsync(url, new StringContent(query));
            var content = esresult.Content as StreamContent;
            if (null == content) throw new InvalidOperationException("StreamContent is null");

            var json2 = await content.ReadAsStringAsync();
            var wid = JObject.Parse(json2).SelectToken("hits.hits[0]._source.wid");
            return wid?.ToString();
        }

        public async Task SaveInstance(Correlation corr)
        {
            var json = corr.ToJson();
            var url = $"{ConfigurationManager.ElasticSearchIndex}/correlationset/{corr.Id}";
            using (var client = new HttpClient { BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost) })
            {
                var response = await client.PutAsync(url, new StringContent(json)).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
        }
    }
}
