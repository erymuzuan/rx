using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Controllers
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

        public ActionResult SaveSnippets()
        {
            var snippets = this.GetRequestBody();
            var json = Server.MapPath("~/code.snippets.js");
            System.IO.File.WriteAllText(json,snippets);
            return Json(new {status = "OK", success =true});
        }
    }
}