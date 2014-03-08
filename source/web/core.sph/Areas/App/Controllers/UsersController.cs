using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Web.Filters;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class UsersController : BaseAppController
    {
        [RazorScriptFilter]
        public ActionResult Js()
        {
            this.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            return View("Script");
        }

    
        public ActionResult Html()
        {
            return View();
        }
    }
}