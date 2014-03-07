using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class SearchController : Controller
    {
        public async Task<ActionResult> Index(string text)
        {
            var provider = ObjectBuilder.GetObject<ISearchProvider>();
            var results = await provider.SearchAsync(text);
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(JsonConvert.SerializeObject(results));
        }

        public async Task<ActionResult> Es(string type, string json)
        {
            var request = new StringContent(json);
            var url = string.Format("{0}/{1}/_search", ConfigurationManager.ElasticSearchIndex, type);

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
        public async Task<ActionResult> Activity()
        {
            return await Es("activity", this.GetRequestBody());
        }

        public async Task<ActionResult> Log()
        {
            return await Es("log", this.GetRequestBody());
        }

        public async Task<ActionResult> WorkflowDefinition()
        {
            return await Es(typeof(WorkflowDefinition).Name.ToLowerInvariant(), this.GetRequestBody());
        }
        public async Task<ActionResult> EntityDefinition()
        {
            return await Es(typeof(EntityDefinition).Name.ToLowerInvariant(), this.GetRequestBody());
        }
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
