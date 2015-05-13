using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Bespoke.Sph.Web.Controllers
{
    public class ResourcesController : Controller
    {
        [OutputCache(VaryByParam = "id;folder", Duration = 300)]
        private ActionResult GetResource(string id, string folder)
        {

            var contentType = MimeMapping.GetMimeMapping(Path.GetExtension(id) ?? ".txt");

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = $"Bespoke.Sph.Web.{folder}.{id}";

            var raw = this.Request.RawUrl;
            if (raw.StartsWith("/~"))
                raw = raw.Remove(0, 1);
            else if (raw.StartsWith("/"))
                raw = "~" + raw;
            if (raw.Contains("?"))
                raw = raw.Remove(raw.IndexOf("?", StringComparison.InvariantCultureIgnoreCase));

            var file = Server.MapPath(raw);
            if (System.IO.File.Exists(file))
            {
                return File(System.IO.File.ReadAllBytes(file), contentType);
            }

            var stream = assembly.GetManifestResourceStream(resourceName);
            if (null != stream)
            {
                return File(stream, contentType);
            }


            // processed, do not cache
            this.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            if (id.StartsWith("viewmodels") && id.EndsWith(".js"))
            {
                var controller = id.Replace("viewmodels.", "")
                 .Replace(".js", "")
                 .Replace(".", "");

                return RedirectToAction("Js", controller, new { area = "App" });


            }
            if (id.StartsWith("views.") && id.EndsWith(".html"))
            {
                var controller = id.Replace("views.", "")
                    .Replace(".html", "")
                    .Replace(".", "");

                return RedirectToAction("Html", controller, new { area = "App" });
            }
            return new HttpNotFoundResult("Cannot find " + id + " in core.sph.dll embedded resources");

        }

        public ActionResult SphApp(string id)
        {
            return GetResource(id, "SphApp");
        }

        public ActionResult Scripts(string id)
        {
            return GetResource(id, "Scripts");

        }

        public ActionResult Kendo(string id)
        {
            return GetResource(id, "kendo");
        }

        public ActionResult Images(string id)
        {
            return GetResource(id, "Images");
        }

        [ActionName("Content")]
        public ActionResult Css(string id)
        {

            if (id == "jsplumb.js")
                id = "jsplumb.jsplumb.css";
            if (id == "workflow.triggers.css.map")
                id = "theme.matyie.workflow.triggers.css.map";

            return GetResource(id, "Content");



        }
        public ActionResult Fonts(string id)
        {
            return GetResource(id, "fonts");

        }
    }
}