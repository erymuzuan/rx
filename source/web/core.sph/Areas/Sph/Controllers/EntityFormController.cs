using System.Web.Mvc;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class EntityFormController : Controller
    {
        public ActionResult Save()
        {
            return Json(new { success = true, status = "OK", id = 0 });
        }
    }
}