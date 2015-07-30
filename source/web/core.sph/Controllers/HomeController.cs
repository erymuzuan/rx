using System.Web.Mvc;

namespace Bespoke.Sph.Web.Controllers
{
    public class HomeController : Controller
    {
        [OutputCache(CacheProfile = "home.index")]
        public ActionResult Index()
        {
            return View();
        }

    }
}