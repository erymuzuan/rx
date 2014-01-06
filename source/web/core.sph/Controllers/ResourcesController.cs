using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Bespoke.Sph.Web.Controllers
{
    public class ResourcesController : Controller
    {
        private ActionResult GetResource(string id, string folder)
        {
            var contentType = MimeMapping.GetMimeMapping(Path.GetExtension(id) ?? ".txt");

            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = string.Format("Bespoke.Sph.Web.{0}.{1}", folder, id);

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
            return Content("[DAMNIT] why can't i get the " + id);



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


            return GetResource(id, "Content");



        }
        public ActionResult Fonts(string id)
        {
            return GetResource(id, "fonts");

        }
    }
}