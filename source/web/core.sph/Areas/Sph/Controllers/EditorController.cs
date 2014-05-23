using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.ViewModels;
using Newtonsoft.Json;

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
        public ActionResult Snippets()
        {
            var file = Server.MapPath("~/code.snippets.json");
            var json = System.IO.File.ReadAllText(file);
            this.Response.ContentType = "application/json";
            return Content(json);
        }

        public ActionResult SaveSnippets()
        {
            var snippets = JsonConvert.DeserializeObject(this.GetRequestBody());
            var file = Server.MapPath("~/code.snippets.json");

            System.IO.File.WriteAllText(file, JsonConvert.SerializeObject(snippets, Formatting.Indented));
            return Json(new {status = "OK", success =true});
        }
    }
}