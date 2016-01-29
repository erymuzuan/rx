using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("resource-bundle")]
    public class ResourceBundleController : Controller
    {
        [Route("css")]
        public ActionResult Css()
        {
            var css = new StringBuilder();
            css.AppendLine(GetScript("Content.__css.min.css"));

            foreach (var file in Directory.GetFiles(Server.MapPath("~/Content"), "*.css"))
            {
                css.AppendLine(System.IO.File.ReadAllText(file));
            }

            this.Response.ContentType = "text/css";
            this.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(ConfigurationManager.StaticFileCache));
            this.Response.Cache.SetCacheability(HttpCacheability.Public);

            return Content(css.ToString());
        }

        static string GetScript(string key)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("Bespoke.Sph.Web." + key);
            if (null == stream) return null;
            stream.Position = 0;
            using (var reader = new StreamReader(stream, Encoding.UTF8))
            {
                return reader.ReadToEnd();
            }
        }
    }
}