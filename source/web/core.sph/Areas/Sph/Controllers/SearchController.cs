﻿using System;
using System.Web.Http;
using System.Threading.Tasks;
using System.Linq;
using System.Net.Http.Headers;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;
using Humanizer;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("search")]
    public class SearchController : BaseApiController
    {
        public SearchController()
        {
            this.CacheManager = ObjectBuilder.GetObject<ICacheManager>();
        }

        private ICacheManager CacheManager { get; set; }

        private const string TypesKey = "search:published-types";
        private const string RecordKey = "search:published-records";

        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> Index([FromUri(Name = "q")]string text, [ContentType]MediaTypeHeaderValue contentType)
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
                types = entNames.ToString(",");
                records = en.Distinct().ToString(",");

                CacheManager.Insert(TypesKey, types, 5.Minutes());
                CacheManager.Insert(RecordKey, records, 5.Minutes());
            }

            var provider = contentType?.MediaType ?? "odata";
            var parser = QueryParserFactory.Instance.Get(provider);
            var repos = ObjectBuilder.GetObject<IReadOnlyRepository>();
            var result = await repos.SearchAsync(types.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries), parser.Parse(text, String.Empty));

            var setting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            return Json(result, setting);

        }

        [HttpPost]
        [Route("log")]
        public async Task<IHttpActionResult> SearchLogAsync( [RawBody]string queryText, [ContentType]MediaTypeHeaderValue contentType)
        {
            var parser = QueryParserFactory.Instance.Get(null, contentType?.MediaType ?? "appplication/json+elasticsearch");
            var query = parser.Parse(queryText, String.Empty);

            var repos = ObjectBuilder.GetObject<ILoggerRepository>();
            var result = await repos.SearchAsync(query);

            var setting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            return Json(result, setting);

        }
        [HttpPost]
        [Route("{type}")]
        public async Task<IHttpActionResult> SearchAsync(string type, [RawBody]string queryText, [ContentType]MediaTypeHeaderValue contentType)
        {
            var parser = QueryParserFactory.Instance.Get(null, contentType?.MediaType ?? "appplication/json+elasticsearch");
            var query = parser.Parse(queryText, String.Empty);

            var repos = ObjectBuilder.GetObject<IReadOnlyRepository>();
            var result = await repos.SearchAsync(new[] { type }, query);

            var setting = new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore };
            setting.Converters.Add(new Newtonsoft.Json.Converters.StringEnumConverter());
            return Json(result, setting);

        }

        [HttpPost]
        [Route("workflow/{id}/v{version}")]
        public async Task<IHttpActionResult> Workflow(string id, string version, [RawBody]string json)
        {
            var wfes = $"{id}workflow".Replace("-", "");
            return await SearchAsync(wfes, json, new MediaTypeHeaderValue("odata/V2"));
        }


    }
}
