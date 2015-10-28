using System.IO;
using System.Web.Mvc;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("i18n")]
    public class InternationalizationController : Controller
    {
        [HttpGet]
        [Route("{language}/{resource}")]
        public ActionResult GetResourceAsync(string resource, string language = "en")
        {
            var folder = Server.MapPath("~/App_Data/i18n/");
            var file = $"{resource}.{language}.json";
            this.Response.ContentType = "application/json";
            if (!System.IO.File.Exists(Path.Combine(folder, file)))
                return Content("{}");

            var text = System.IO.File.ReadAllText(Path.Combine(folder, file));

            return Content(text);

        }

        [HttpPost]
        [Authorize]
        [Route("{language}/{resource}")]
        public ActionResult SaveAsync(string resource, string language,[RawRequestBody] string json)
        {
            var file = Server.MapPath(string.Format("~/App_Data/i18n/{1}.{0}.json", language, resource));

            System.IO.File.WriteAllText(file, json);
            return Json(new { status = "OK", success = true });
        }

        [HttpGet]
        [Authorize]
        [Route("options")]
        public ActionResult GetOptions()
        {
            var folder = Server.MapPath("~/App_Data/i18n/");
            const string OPTIONS_JSON = "options.json";
            this.Response.ContentType = "application/json";
            if (!System.IO.File.Exists(Path.Combine(folder, OPTIONS_JSON)))
                return Content("{}");

            var text = System.IO.File.ReadAllText(Path.Combine(folder, OPTIONS_JSON));

            return Content(text);
        }
    }
}