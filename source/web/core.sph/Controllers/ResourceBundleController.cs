using System.IO;
using System.Reflection;
using System.Text;
using System.Web.Mvc;

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

            foreach (var file in System.IO.Directory.GetFiles(Server.MapPath("~/Content"), "*.css"))
            {
                css.AppendLine(System.IO.File.ReadAllText(file));
            }

            this.Response.ContentType = "text/css";
            return Content(css.ToString());
        }

        static string GetScript(string key)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("Bespoke.Sph.Web." + key);
            if (null != stream)
            {
                stream.Position = 0;
                using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
                {
                    return reader.ReadToEnd();
                }
            }

            return null;

        }
    }
}