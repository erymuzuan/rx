using System.Web.Mvc;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("solution")]
    [Authorize(Roles = "developers")]
    public class SolutionController : Controller
    {
        [Route("diagnostics")]
        public ActionResult StartDiagnostics()
        {

            // validate all forms
            // - path
            // - route

            // validate all views
            // - column path
            // - link , is correct route
            // - icon
            // - some suggestion to spelling
            // - route

            // validate all entities
            // - record name
            // - security
            // - icon/image
            // - Operation, name, security, Setter Path



            // triggers
            // -> action
            // - filter
            // - compilation
            // - deployment
            // - active/inactive

            // workflow

            // 

            return Json(true);
        }
    }
}