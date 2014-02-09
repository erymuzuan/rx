using System;
using System.Net;
using System.Net.Http;
using System.Security;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class ManagementController : Controller
    {
        public async Task<ActionResult> Index(string text)
        {
            var provider = ObjectBuilder.GetObject<ISearchProvider>();
            var results = await provider.SearchAsync(text);
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(JsonConvert.SerializeObject(results));
        }

        public async Task<ActionResult> Api(string id)
        {
            var url = "api/" + id;
            dynamic broker = ObjectBuilder.GetObject("IBrokerConnection");
            using (var handler = new HttpClientHandler { Credentials = new NetworkCredential(broker.UserName, broker.Password)})
            using (var client = new HttpClient(handler))
            {
                client.BaseAddress = new Uri("http://localhost:15672/");

                var response = await client.GetStringAsync(url);
                this.Response.ContentType = "application/json; charset=utf-8";
                return Content(response);

            }
        }


    }
}