using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Web.Filters;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class UsersController : BaseAppController
    {
        [RazorScriptFilter]
        [NoCache]
        public ActionResult Js()
        {
            return View("Script");
        }

    
        public ActionResult Html()
        {
            return View();
        }
    }
}