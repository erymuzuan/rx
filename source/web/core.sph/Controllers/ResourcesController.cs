using System;
using System.IO;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Bespoke.Sph.Web.Controllers
{
    public class ResourcesController : Controller
    {
        public ActionResult SphApp(string id)
        {
            Console.WriteLine("-*-*-*-*-");
            Console.WriteLine(id);
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Bespoke.Sph.Web.SphApp." + id;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (null == stream)
                {
                    Console.WriteLine(this.Request.Path);
                    Console.WriteLine(this.Request.RawUrl);
                    var raw = this.Request.RawUrl;
                    if (raw.StartsWith("/~"))
                        raw = raw.Remove(0, 1);
                    else if (raw.StartsWith("/"))
                        raw = "~" + raw;

                    var file = Server.MapPath(raw);
                    if (System.IO.File.Exists(file))
                    {
                        var extension = Path.GetExtension(file) ?? ".js";
                        return File(System.IO.File.ReadAllBytes(file), MimeMapping.GetMimeMapping(extension));
                    }
                    if (id.StartsWith("viewmodels"))
                    {
                        var controller = id.Replace("views.", "")
                         .Replace(".html", "")
                         .Replace(".", "");
                        return RedirectToAction("Html", controller, new { area = "App" });
                    
                        
                    }
                    if (id.StartsWith("views.") && id.EndsWith(".html"))
                    {
                        var controller = id.Replace("views.", "")
                            .Replace(".html", "")
                            .Replace(".", "");
                        return RedirectToAction("Html", controller, new {area = "App"});
                    }
                    return Content("[DAMNIT] why can't i get the " + id);
                }
                using (var reader = new StreamReader(stream))
                {
                    string result = reader.ReadToEnd();
                    this.Response.ContentType = MimeMapping.GetMimeMapping(Path.GetExtension(id));
                    return Content(result);
                }
            }
        }

        public ActionResult Scripts(string id)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Bespoke.Sph.Web.Scripts." + id;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                this.Response.ContentType = "application/javascript";
                return Content(result);
            }
        }

        public ActionResult Kendo(string id)
        {
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Bespoke.Sph.Web.kendo." + id;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                this.Response.ContentType = MimeMapping.GetMimeMapping(Path.GetExtension(id));
                return Content(result);
            }
        }

        public ActionResult Images(string id)
        {
            Console.WriteLine("************");
            Console.WriteLine(id);
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Bespoke.Sph.Web.Images." + id;

            var stream = assembly.GetManifestResourceStream(resourceName);

            var ct = MimeMapping.GetMimeMapping(Path.GetExtension(id));
            return File(stream, ct);

        }

        [ActionName("Content")]
        public ActionResult Css(string id)
        {
            Console.WriteLine("************");
            Console.WriteLine(id);
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Bespoke.Sph.Web.Content." + id;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                this.Response.ContentType = MimeMapping.GetMimeMapping(Path.GetExtension(id));
                return Content(result);
            }
        }
        public ActionResult Fonts(string id)
        {
            Console.WriteLine("************");
            Console.WriteLine(id);
            var assembly = Assembly.GetExecutingAssembly();
            var resourceName = "Bespoke.Sph.Web.fonts." + id;

            using (var stream = assembly.GetManifestResourceStream(resourceName))
            using (var reader = new StreamReader(stream))
            {
                string result = reader.ReadToEnd();
                this.Response.ContentType = MimeMapping.GetMimeMapping(Path.GetExtension(id));
                return Content(result);
            }
        }
    }
}