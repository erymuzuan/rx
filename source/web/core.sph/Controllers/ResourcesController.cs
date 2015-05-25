using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
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
                this.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(7));
                this.Response.Cache.SetCacheability(HttpCacheability.Public);
                var lastAccessTimeUtc = System.IO.File.GetLastAccessTimeUtc(file);
                this.Response.Cache.SetLastModified(lastAccessTimeUtc);
                this.Response.Cache.SetETag(GetMd5Hash(lastAccessTimeUtc.ToString(CultureInfo.InvariantCulture)));
                return File(System.IO.File.ReadAllBytes(file), contentType);
            }

            var stream = assembly.GetManifestResourceStream(resourceName);
            if (null != stream)
            {
                this.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(7));
                this.Response.Cache.SetCacheability(HttpCacheability.Public);
                var lastAccessTimeUtc = System.IO.File.GetLastAccessTimeUtc(Server.MapPath("~/bin/core.sph.dll"));
                this.Response.Cache.SetLastModified(lastAccessTimeUtc);
                this.Response.Cache.SetETag(GetMd5Hash(lastAccessTimeUtc.ToString(CultureInfo.InvariantCulture)));
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
        static string GetMd5Hash(string input)
        {
            using (var md5Hash = MD5.Create())
            {
                // Convert the input string to a byte array and compute the hash. 
                byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

                // Create a new Stringbuilder to collect the bytes 
                // and create a string.
                StringBuilder sBuilder = new StringBuilder();

                // Loop through each byte of the hashed data  
                // and format each one as a hexadecimal string. 
                for (int i = 0; i < data.Length; i++)
                {
                    sBuilder.Append(data[i].ToString("x2"));
                }

                // Return the hexadecimal string. 
                return sBuilder.ToString();

            }
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