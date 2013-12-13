using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    public class SearchController : Controller
    {
        public async Task<ActionResult> Index(string text)
        {
            var provider = ObjectBuilder.GetObject<ISearchProvider>();
            var results = await provider.SearchAsync(text);
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await JsonConvert.SerializeObjectAsync(results));
        }

        [HttpPost]
        public async Task<ActionResult> Building()
        {
            return await this.Es("building");
        }

        [HttpPost]
        public async Task<ActionResult> Space()
        {
            return await this.Es("space");
        }
        [HttpPost]
        public async Task<ActionResult> Land()
        {
            return await this.Es("land");
        }

        [HttpPost]
        public async Task<ActionResult> Contract()
        {
            return await this.Es(typeof(Contract).Name.ToLowerInvariant());
        }

        [HttpPost]
        public async Task<ActionResult> Complaint()
        {
            return await this.Es(typeof(Complaint).Name.ToLowerInvariant());
        }

        [HttpPost]
        public async Task<ActionResult> Maintenance()
        {
            return await this.Es(typeof(Maintenance).Name.ToLowerInvariant());
        }

        [HttpPost]
        public async Task<ActionResult> Payment()
        {
            return await this.Es(typeof(Payment).Name.ToLowerInvariant());
        }


        [HttpPost]
        public async Task<ActionResult> Rebate()
        {
            return await this.Es(typeof(Rebate).Name.ToLowerInvariant());
        }

        [HttpPost]
        public async Task<ActionResult> RentalApplication()
        {
            return await this.Es(typeof(RentalApplication).Name.ToLowerInvariant());
        }

        [HttpPost]
        public async Task<ActionResult> Invoice()
        {
            return await this.Es(typeof(Invoice).Name.ToLowerInvariant());
        }


        public async Task<ActionResult> Es(string id)
        {
            var json = this.GetRequestBody();
            var type = id;
            var request = new StringContent(json);
            var url = string.Format("{0}/{1}/{2}/_search", ConfigurationManager.ElasticSearchHost,
                ConfigurationManager.ElasticSearchIndex, type);

            var client = new HttpClient();
            var response = await client.PostAsync(url, request);
            var content = response.Content as StreamContent;
            if (null == content) throw new Exception("Cannot execute query on es " + request);
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await content.ReadAsStringAsync());
        }


    }
}
