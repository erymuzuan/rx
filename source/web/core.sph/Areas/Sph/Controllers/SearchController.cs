using System;
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
                types = entNames.ToString(",");
                records = en.Distinct().ToString(",");

                CacheManager.Insert(TypesKey, types, 5.Minutes());
                CacheManager.Insert(RecordKey, records, 5.Minutes());
            }

            var repos = ObjectBuilder.GetObject<IReadonlyRepository>();
            var result = await repos.SearchAsync(types.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries), Filter.Parse(text));

            return Json(result);

        }

        [HttpPost]
        [Route("{type}")]
        public async Task<IHttpActionResult> Es(string type, [RawBody]string query, [FromUri]bool sys = true)
        {

            var repos = ObjectBuilder.GetObject<IReadonlyRepository>();
            var result = await repos.SearchAsync(new[] { type }, Filter.Parse(query));

            return Json(result);

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
