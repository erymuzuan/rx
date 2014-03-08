using System.Web.Mvc;
using Bespoke.Sph.Web.Filters;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class BaseSphController : Controller
    {
        public const string APPLICATION_JAVASCRIPT = "application/javascript";
        public const string TEXT_HTML = "text/html";

        protected internal virtual ViewResult Script(string viewName = null, object model = null, string masterName = null)
        {
            if (model != null)
            {
                ViewData.Model = model;
            }

            return new ScriptResult
            {
                ViewName = viewName,
                MasterName = masterName,
                ViewData = ViewData,
                TempData = TempData,
                ViewEngineCollection = ViewEngineCollection
            };
        }
    }
}