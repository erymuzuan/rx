using System.Web;
using System.Web.Mvc;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class UsersController : BaseAppController
    {
        public ActionResult Js()
        {
            this.Response.ContentType = APPLICATION_JAVASCRIPT;
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);

            var script = this.RenderScript("Script");
            return Content(script);
        }

        public ActionResult Script()
        {
            return View();
        }
        public ActionResult Html()
        {
            return View();
        }
    }
}