using System;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;
using Humanizer;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("search")]
    public class SearchController : BaseApiController
    {
        public SearchController()
        {
            this.CacheManager = ObjectBuilder.GetObject<ICacheManager>();
        }

        public ICacheManager CacheManager { get; set; }

        private const string TypesKey = "search:published-types";
        private const string RecordKey = "search:published-records";

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Index([FromUri(Name = "q")]string text)
        {
            var types = this.CacheManager.Get<string>(TypesKey);
            var records = this.CacheManager.Get<string>(RecordKey);
            if (null == types || null == records)
            {
                var context = new SphDataContext();
                var en = (await context.GetListAsync<EntityDefinition, string>(e => e.IsPublished, e => e.RecordName))
                    .Select(a => $"\"{a}\":{{}}");
                var entNames = (await context.GetListAsync<EntityDefinition, string>(e => e.IsPublished, e => e.Name))
                    .Select(a => a.ToLowerInvariant())
                    .ToArray();
                types = string.Join(",", entNames);
                records = string.Join(",", en.Distinct().ToArray());

                CacheManager.Insert(TypesKey, types, 5.Minutes());
                CacheManager.Insert(RecordKey, records, 5.Minutes());
            }



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

            }
        }

        [HttpPost]
        [Route("{type}")]
        public async Task<IHttpActionResult> Es(string type, [RawBody]string query, [FromUri]bool sys = true)
        {
            var request = new StringContent(query);
            var log = type == "log" || type == "request_log";
            var index = sys ? ConfigurationManager.ElasticSearchSystemIndex : ConfigurationManager.ElasticSearchIndex;
            if (log) index = $"{ConfigurationManager.ElasticSearchIndex}_logs";
            var url = $"{index}/{type.ToLowerInvariant()}/_search";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(log ? ConfigurationManager.ElasticsearchLogHost : ConfigurationManager.ElasticSearchHost);

                var response = await client.PostAsync(url, request);
                var content = response.Content as StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es " + request);

                var result = await content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                    throw new SearchException("Cannot execute query for : " + type) { Query = query, Result = result };

                return Json(result);

            }
        }

        [HttpPost]
        [Route("workflow/{id}/v{version}")]
        public async Task<IHttpActionResult> Workflow(string id, string version, [RawBody]string json)
        {
            var wfes = $"{id}workflow".Replace("-", "");
            return await Es(wfes, json, false);
        }


    }

    public class SearchException : Exception
    {
        public string Query { get; set; }
        public string Result { get; set; }
        public SearchException(string message) : base(message) { }
        public SearchException(string message, Exception exception) : base(message, exception)
        {
        }

        public override string ToString()
        {
            var ex = new StringBuilder();
            ex.AppendLine(this.Message);
            ex.AppendLine("Query " + this.Query);
            ex.AppendLine("Result " + this.Result);
            return ex.ToString();
        }
    }
}
