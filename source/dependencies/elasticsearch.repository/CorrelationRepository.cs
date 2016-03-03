using System;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;

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
            var url = $"{ConfigurationManager.ElasticSearchIndex}/correlationset/_search";
            var q = new
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
            var esresult = await m_client.PostAsync(url, new StringContent(q.ToJson()));
            var content = esresult.Content as StreamContent;
            if (null == content) throw new InvalidOperationException("StreamContent is null");

            var json2 = await content.ReadAsStringAsync();
            var wid = Newtonsoft.Json.Linq.JObject.Parse(json2).SelectToken("hits.hits[0]._source.wid");
            if (null == wid) return default(T);

            var context = new SphDataContext();
            var instance = await context.LoadOneAsync<Workflow>(x => x.Id == wid.ToString());
            await instance.LoadWorkflowDefinitionAsync();
            return (T)instance;

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
