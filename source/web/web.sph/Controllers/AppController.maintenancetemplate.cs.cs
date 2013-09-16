using System.Web.Mvc;

namespace Bespoke.Sph.Web.Controllers
{
    public partial class AppController
    {

        [Authorize]
        public ActionResult MaintenanceTemplateListHtml()
        {
            return View();
        }

        [Authorize]
        public ActionResult MaintenanceTemplateListJs()
        {
            return View();
        }

    }
}
