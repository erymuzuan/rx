using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    public class EditorController : Controller
    {
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
    }

    public class EditorPageViewModel
    {
        public Bespoke.Sph.Domain.Page Page { get; set; }
    }
}