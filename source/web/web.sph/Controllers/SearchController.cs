using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
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


    }
}
