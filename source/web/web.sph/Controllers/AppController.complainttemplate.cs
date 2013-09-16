using System.Web.Mvc;

namespace Bespoke.Sph.Web.Controllers
{
    public partial class AppController
    {
        [Authorize]
        public ActionResult ComplaintTemplateListHtml()
        {
            return View();
        }

        [Authorize]
        public ActionResult ComplaintTemplateListJs()
        {
            return View();
        }
    }
}
