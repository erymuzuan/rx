using System.IO;
using System.Web.Mvc;
using Bespoke.Sph.Web.ViewModels;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class TestController : Controller
    {
        public ActionResult Index(string id)
        {
            var path = Server.MapPath("~/ScriptTests");
            var files = Directory.GetFiles(path, "*.js");
            var vm = new JsTestViewModel
            {
                Files = files,
                Script = id
            };
            return View(vm);
        }

    }
}
