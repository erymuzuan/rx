using System.Web.Mvc;

namespace Bespoke.Sph.WebSph.Controllers
{
    [Authorize(Roles = "administrators")]
    [RoutePrefix("management-console")]
    public class ManagementConsoleController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}