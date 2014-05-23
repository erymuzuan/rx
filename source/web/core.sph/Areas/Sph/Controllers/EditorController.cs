using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.ViewModels;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class EditorController : Controller
    {
        public ActionResult Help()
        {
            return View();
        }
        public ActionResult Ace()
        {
            return View();
        }
        public async Task<ActionResult> Page(int id)
        {
            var context = new SphDataContext();
            var page = await context.LoadOneAsync<Page>(p => p.PageId == id);
            var vm = new EditorPageViewModel { Page = page };
            return View(vm);
        }

        [NoCache]
        public ActionResult Snippets(string id)
        {
            var folder = Server.MapPath("~/App_Data/snippets/" + id);
            var snippets = from f in System.IO.Directory.GetFiles(folder, "*.json")
                           let json = System.IO.File.ReadAllText(f)
                           select json;

            this.Response.ContentType = "application/json";
            return Content("[" + string.Join(",", snippets) + "]");
        }

        public ActionResult SaveSnippet()
        {
            var snippet = this.GetRequestJson<Snippet>();
            var file = Server.MapPath(string.Format("~/App_Data/snippets/{0}/{1}.json", snippet.Lang, snippet.Title));
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
            System.IO.File.WriteAllText(file, JsonConvert.SerializeObject(snippet, Formatting.Indented, settings));
            return Json(new { status = "OK", success = true });
        }
    }
}