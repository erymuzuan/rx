using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("custom-forms")]
    [Authorize(Roles = "developers")]
    public class CustomFormController : Controller
    {

        const string CustomRouteConfig = "~/App_Data/routes.config.json";
        [HttpGet]
        [Route("download")]
        public ActionResult Index()
        {
            var path = Path.Combine(Path.GetTempPath(), "rx.package.-custom-forms");
            if (Directory.Exists(path)) Directory.Delete(path, true);
            Directory.CreateDirectory(path);
            var zip = path + ".zip";

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var customRouteConfig = Server.MapPath(CustomRouteConfig);
            var customJsRoutes = new JsRoute[] { };
            if (System.IO.File.Exists(customRouteConfig))
            {
                var json = System.IO.File.ReadAllText(customRouteConfig);
                customJsRoutes = JsonConvert.DeserializeObject<JsRoute[]>(json, settings);
                System.IO.File.Copy(customRouteConfig, $"{path}\\routes.config.json", true);
            }

            foreach (var form in customJsRoutes)
            {
                Copy(form.ModuleId.Replace("viewmodels/", "") + ".html", "views", path);
                Copy(form.ModuleId.Replace("viewmodels/", "") + ".js", "viewmodels", path);
            }


            if (System.IO.File.Exists(zip))
                System.IO.File.Delete(zip);
            ZipFile.CreateFromDirectory(path, zip);


            return File(zip, MimeMapping.GetMimeMapping(zip), $"rx.package-custom-forms-{Environment.MachineName}-{DateTime.Now:yyyyMMdd-HHmm}.zip");
        }
        [Route("upload")]
        public ActionResult Upload(IEnumerable<HttpPostedFileBase> files)
        {
            try
            {
                foreach (var postedFile in files)
                {
                    var fileName = Path.GetFileName(postedFile.FileName);
                    if (string.IsNullOrWhiteSpace(fileName)) throw new Exception("Filename is empty or null");


                    var zip = Path.Combine(Path.GetTempPath(), fileName);
                    postedFile.SaveAs(zip);

                    var folder = Directory.CreateDirectory(Path.GetTempFileName() + "extract").FullName;
                    UnpackAsync(zip, folder);

                    return Json(new { success = true, status = "OK"});

                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, exception = e.GetType().FullName, message = e.Message, stack = e.StackTrace });
            }
            return Json(new { success = false });


        }
        private void UnpackAsync(string zipFile, string folder)
        {
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            setting.Converters.Add(new StringEnumConverter());

            ZipFile.ExtractToDirectory(zipFile, folder);

            foreach (var js in Directory.GetFiles(folder, "*.js"))
            {
                System.IO.File.Copy(js, $"{ConfigurationManager.WebPath}\\SphApp\\viewmodels\\{Path.GetFileName(js)}", true);
            }

            foreach (var html in Directory.GetFiles(folder, "*.html"))
            {
                System.IO.File.Copy(html, $"{ConfigurationManager.WebPath}\\SphApp\\views\\{Path.GetFileName(html)}", true);
            }

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var customRouteConfig = Server.MapPath(CustomRouteConfig);
            var customJsRoutes = new List<JsRoute>();
            if (System.IO.File.Exists(customRouteConfig))
            {
                var json = System.IO.File.ReadAllText(customRouteConfig);
                customJsRoutes.AddRange(JsonConvert.DeserializeObject<JsRoute[]>(json, settings));
            }

            string importedRouteConfig = $"{folder}\\routes.config.json";
            var importedJson = System.IO.File.ReadAllText(importedRouteConfig);
            var importedRoutes = JsonConvert.DeserializeObject<JsRoute[]>(importedJson).ToList();
            foreach (var route in importedRoutes)
            {
                if (customJsRoutes.All(x => x.Route != route.Route))
                {
                    customJsRoutes.Add(route);
                }
            }

            var text = JsonConvert.SerializeObject(customJsRoutes, Formatting.Indented, settings);
            System.IO.File.WriteAllText(customRouteConfig, text);

        }
        private static void Copy(string fileName, string folder, string path)
        {
            var view = Path.Combine(ConfigurationManager.WebPath, $"SphApp\\{folder}\\{fileName}");
            if (System.IO.File.Exists(view))
                System.IO.File.Copy(view, $"{path}\\{fileName}", true);
        }

    }
}