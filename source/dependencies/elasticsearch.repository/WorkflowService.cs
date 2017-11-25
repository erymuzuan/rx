using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.ElasticsearchRepository.Extensions;
using Newtonsoft.Json.Linq;

namespace Bespoke.Sph.ElasticsearchRepository
{
    public class WorkflowService : IWorkflowService
    {
        private readonly HttpClient m_client;
        private readonly JObject m_mapping;
        public WorkflowService(string host)
        {
            m_client = new HttpClient { BaseAddress = new Uri(host) };
            // TODO : mapping for workflow
            m_mapping = JObject.Parse(Properties.Resources.LogMapping);
        }
        public WorkflowService() : this(EsConfigurationManager.Host)
        {

        }

        public WorkflowService(HttpMessageHandler httpMessageHandler, bool disposeHandler)
        {
            m_client = new HttpClient(httpMessageHandler, disposeHandler) { BaseAddress = new Uri(EsConfigurationManager.Host) };
        }
        public WorkflowService(string host, HttpMessageHandler httpMessageHandler, bool disposeHandler)
        {
            m_client = new HttpClient(httpMessageHandler, disposeHandler) { BaseAddress = new Uri(host) };
        }


        public async Task<T> GetInstanceAsync<T>(WorkflowDefinition wd, string correlationName, string correlationValue) where T : Workflow, new()
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
            if (null == wid)
                return default;

            var context = new SphDataContext();
            var instance = await context.LoadOneAsync<Workflow>(x => x.Id == wid.ToString());
            await instance.LoadWorkflowDefinitionAsync();
            return (T)instance;

        }

        private async Task<string> ExecuteElasticsearchQueryAsync(string query)
        {
            var url = $"{EsConfigurationManager.Index}/correlationset/_search";
            var esresult = await m_client.PostAsync(url, new StringContent(query));
            if (!(esresult.Content is StreamContent content)) throw new InvalidOperationException("StreamContent is null");

            var json2 = await content.ReadAsStringAsync();
            var wid = JObject.Parse(json2).SelectToken("hits.hits[0]._source.wid");
            return wid?.ToString();
        }

        public async Task SaveInstanceAsync(Correlation corr)
        {
            var json = corr.ToJson();
            var url = $"{EsConfigurationManager.Index}/correlationset/{corr.Id}";
            using (var client = new HttpClient { BaseAddress = new Uri(EsConfigurationManager.Host) })
            {
                var response = await client.PutAsync(url, new StringContent(json)).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
            }
        }

        public async Task<LoadOperation<WorkflowPresentation>> GetPendingWorkflowsAsync<T>(string activityId, string[] fields,
            IEnumerable<Filter> predicates, int from = 0, int size = 20) where T : Workflow, new()
        {
            var wf = new T();
            var wdid = wf.WorkflowDefinitionId;
            var list = new List<string>();

            var url = $"{EsConfigurationManager.Index}/pendingtask/_search";

            const int TASK_PAGE_SIZE = 50;
            var total = TASK_PAGE_SIZE;
            var taskFrom = 0;
            while (taskFrom <= total)
            {
                var query = $@"{{
                       ""query"": {{
                          ""bool"": {{
                             ""must"": [
                                {{
                                   ""term"": {{
                                      ""WorkflowDefinitionId"": {{
                                         ""value"": ""{wdid}""
                                      }}
                                   }}
                                }},
                                {{
                                   ""term"": {{
                                      ""ActivityWebId"": {{
                                         ""value"": ""{activityId}""
                                      }}
                                   }}
                                }}
                             ]
                          }}
                       }},
                       ""_source"": [
                          ""WorkflowId""
                       ],
                       ""from"": {taskFrom},
                       ""size"":{TASK_PAGE_SIZE}
                    }}
";
                var request = new StringContent(query);
                var response = await m_client.PostAsync(url, request);

                if (!(response.Content is StreamContent content)) throw new Exception("Cannot execute query on es " + request);
                var json = await content.ReadAsStringAsync();
                var jo = JObject.Parse(json);
                total = jo.SelectToken("$.hits.total").Value<int>();
                var hits = jo.SelectToken("$.hits.hits").Select(x => x.SelectToken("$._source.WorkflowId").Value<string>());
                list.AddRange(hits);
                taskFrom += TASK_PAGE_SIZE;
            }

            // now look for specific workflow with that Id
            return await GetFilteredHitsAsync<T>(list.ToArray(), predicates, fields, from, size);

        }

        private async Task<LoadOperation<WorkflowPresentation>> GetFilteredHitsAsync<T>(IEnumerable<string> hits,
            IEnumerable<Filter> predicates,
            IEnumerable<string> fields,
            int from = 0,
            int size = 20) where T : Workflow, new()
        {
            var wf = new T();
            var terms = predicates.Select(x =>x.ToElasticsearchFilter(m_mapping)).Select(x => x.CompileToTermLevelQuery<T>());
            // TODO : Make the id field in mapping  as not-analyzed
            var ids = hits.Select(x => x.Remove(0, x.LastIndexOf("-", StringComparison.Ordinal) + 1))
                .Select(x => $@"""{x}""")
                .ToList();

            // TODO : breaks the id into a chunck of 1024 , and do the request in a while loop
            var query = $@"{{
                       ""query"": {{
                          ""bool"": {{
                             ""must"": [
                                {{
                                    ""terms"":{{
                                            ""Id"" : [{string.Join(",", ids.Take(1024))}]
                                        }}
                                }},
                                {string.Join(",", terms)}
                             ]
                          }}
                       }},
                       ""_source"": [{string.Join(",", fields.Select(x => $"\"{x}\""))}],
                       ""from"": {from},
                       ""size"":{size}
                    }}";
            var request = new StringContent(query);
            var url = $"{EsConfigurationManager.Index}/{typeof(T).Name.ToLowerInvariant()}/_search?version";

            var response = await m_client.PostAsync(url, request);

            if (!(response.Content is StreamContent content)) throw new Exception("Cannot execute query on es " + request);
            var json = await content.ReadAsStringAsync();
            var jo = JObject.Parse(json);
            var hits2 = jo.SelectToken("$.hits.hits")
                .Select(x => new
                {
                    version = x.SelectToken("$._version").ToString(),
                    id = x.SelectToken("$._id").Value<string>(),
                    _source = x.SelectToken("$._source").ToString()
                })
                .Select(x => new WorkflowPresentation(x.id, x.version, x._source)
                {
                    WorkflowDefinitionId = wf.WorkflowDefinitionId,
                    WorkflowDefinitionVersion = wf.Version
                })
                .ToArray();

            var low = new LoadOperation<WorkflowPresentation>
            {
                TotalRows = ids.Count,
                CurrentPage = @from / size + 1,
                PageSize = size
            };
            low.ItemCollection.AddRange(hits2);

            return low;

        }

        public async Task<T> GetOneAsync<T>(string id) where T : Workflow, new()
        {
            var url = $"{ EsConfigurationManager.Index}/{ typeof(T).Name.ToLowerInvariant()}/{  id}";
            var response = await m_client.GetAsync(url);
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                return null;

            if (!(response.Content is StreamContent content)) return null;

            var json = await content.ReadAsStringAsync();
            var jo = JObject.Parse(json);
            var source = jo.SelectToken("$._source");
            //TODO : cache settings
            return source.ToString().DeserializeFromJson<T>();
        }

        public async Task<IEnumerable<T>> SearchAsync<T>(IEnumerable<Filter> predicates) where T : Workflow, new()
        {
            // TODO : the mapping is unique to each workflow
            var terms = predicates.Select(x => x.ToElasticsearchFilter(m_mapping)).Select(x => x.CompileToTermLevelQuery<T>());
            var query = $@"{{
                       ""query"": {{
                          ""bool"": {{
                             ""must"": [
                                {string.Join(",", terms)}
                             ]
                          }}
                       }}
                    }}";
            var request = new StringContent(query);
            var url = $"{EsConfigurationManager.Index}/{typeof(T).Name.ToLowerInvariant()}/_search";

            var response = await m_client.PostAsync(url, request);

            if (!(response.Content is StreamContent content)) throw new Exception("Cannot execute query on es " + request);
            var json = await content.ReadAsStringAsync();
            var jo = JObject.Parse(json);
            var workflows = from source in jo.SelectToken("$.hits.hits")
                            select source.SelectToken("$._source").ToString().DeserializeFromJson<T>();
            return workflows;

        }
    }
}
