using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Linq;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Controllers
{
    [NoCache]
    [RoutePrefix("search")]
    public class SearchController : Controller
    {
        [HttpGet]
        [Route("{text}")]
        public async Task<ActionResult> Index(string text)
        {
            var context = new SphDataContext();
            var en = (await context.GetListAsync<EntityDefinition, string>(e => e.IsPublished == true, e => e.RecordName))
                .Select(a => $"\"{a}\":{{}}");
            var entNames = (await context.GetListAsync<EntityDefinition, string>(e => e.IsPublished == true, e => e.Name))
                .Select(a => a.ToLowerInvariant())
                .ToArray();
            var types = string.Join(",", entNames);

            var records = string.Join(",", en.Distinct().ToArray());
            var json = @"
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

            var request = new StringContent(json);
            var url = $"{ConfigurationManager.ApplicationName.ToLowerInvariant()}/{types}/_search";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);

                var response = await client.PostAsync(url, request);
                var content = response.Content as StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es " + request);
                this.Response.ContentType = "application/json; charset=utf-8";
                var json2 = await content.ReadAsStringAsync();
                return Content(json2);

            }
        }

        public async Task<ActionResult> Es(string type, string json, bool sys = true)
        {
            var request = new StringContent(json);
            var url = $"{ConfigurationManager.ElasticSearchIndex}{(sys ? "_sys" : "")}/{type}/_search";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(ConfigurationManager.ElasticSearchHost);

                var response = await client.PostAsync(url, request);
                var content = response.Content as StreamContent;
                if (null == content) throw new Exception("Cannot execute query on es " + request);
                this.Response.ContentType = "application/json; charset=utf-8";
                return Content(await content.ReadAsStringAsync());

            }
        }

        [HttpPost]
        [Route("workflow/{id}/v{version}")]
        public async Task<ActionResult> Workflow(string id, string version, [RawRequestBody]string json)
        {
            var wfes = $"{id}workflow".Replace("-", "");
            return await Es(wfes, json, false);
        }

        [Route("activity")]
        public async Task<ActionResult> Activity()
        {
            return await Es("activity", this.GetRequestBody(), false);
        }

        [Route("log")]
        public async Task<ActionResult> Log([RawRequestBody]string body)
        {
            return await Es("log", body);
        }

        public async Task<ActionResult> WorkflowDefinition()
        {
            return await Es(typeof(WorkflowDefinition).Name.ToLowerInvariant(), this.GetRequestBody());
        }
        public async Task<ActionResult> EntityDefinition()
        {
            return await Es(typeof(EntityDefinition).Name.ToLowerInvariant(), this.GetRequestBody());
        }
        [HttpPost]
        [Route("entityview")]
        public async Task<ActionResult> EntityView()
        {
            return await Es(typeof(EntityView).Name.ToLowerInvariant(), this.GetRequestBody());
        }
        public async Task<ActionResult> EntityForm()
        {
            return await Es(typeof(EntityForm).Name.ToLowerInvariant(), this.GetRequestBody());
        }
        public async Task<ActionResult> Designation()
        {
            return await Es(typeof(Designation).Name.ToLowerInvariant(), this.GetRequestBody());
        }

        public async Task<ActionResult> ReportDefinition()
        {
            return await Es(typeof(ReportDefinition).Name.ToLowerInvariant(), this.GetRequestBody());
        }
        public async Task<ActionResult> ReportDelivery()
        {
            return await Es(typeof(ReportDelivery).Name.ToLowerInvariant(), this.GetRequestBody());
        }
        public async Task<ActionResult> Message()
        {
            return await Es(typeof(Message).Name.ToLowerInvariant(), this.GetRequestBody());
        }
        public async Task<ActionResult> Page()
        {
            return await Es(typeof(Page).Name.ToLowerInvariant(), this.GetRequestBody());
        }


    }
}
