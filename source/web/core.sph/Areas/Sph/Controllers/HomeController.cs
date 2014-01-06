using System.Web.Mvc;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }


    }
}
