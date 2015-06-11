using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Areas.App.Controllers;
using Bespoke.Sph.Web.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using static System.IO.File;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("custom-forms")]
    [Authorize(Roles = "developers")]
    public partial class CustomFormController : BaseAppController
    {

        readonly JsonSerializerSettings m_settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
        const string CustomRouteConfig = "~/App_Data/routes.config.json";
        private const string AppDataCustomPartialViewJson = "~/App_Data/custom-partial-view.json";
        private const string AppDataCustomDialogJson = "~/App_Data/custom-dialog.json";
        private const string AppDataCustomScriptJson = "~/App_Data/custom-script.json";

        [HttpGet]
        [Route("download")]
        public ActionResult Index()
        {
            var tempFolder = Path.Combine(Path.GetTempPath(), "rx.package.-custom-forms");
            if (Directory.Exists(tempFolder)) Directory.Delete(tempFolder, true);
            Directory.CreateDirectory(tempFolder);
            var zip = tempFolder + ".zip";

            var viewFolder = $"{tempFolder}\\SphApp\\views";
            if (!Directory.Exists(viewFolder))
                Directory.CreateDirectory(viewFolder);

            var viewModelsFolder = $"{tempFolder}\\SphApp\\viewmodels";
            if (!Directory.Exists(viewModelsFolder))
                Directory.CreateDirectory(viewModelsFolder);

            var serviceFolder = $"{tempFolder}\\SphApp\\services";
            if (!Directory.Exists(serviceFolder))
                Directory.CreateDirectory(serviceFolder);

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var customRouteConfig = Server.MapPath(CustomRouteConfig);
            if (Exists(customRouteConfig))
            {
                var json = ReadAllText(customRouteConfig);
                var customJsRoutes = JsonConvert.DeserializeObject<JsRoute[]>(json, settings);
                Copy(customRouteConfig, $"{tempFolder}\\routes.config.json", true);


                foreach (var form in customJsRoutes)
                {
                    var name = form.ModuleId.Replace("viewmodels/", "");

                    var js = $"{ConfigurationManager.WebPath}\\SphApp\\viewmodels\\{name}.js";
                    if (Exists(js))
                        Copy(js, $"{tempFolder}\\SphApp\\viewmodels\\{name}.js", true);

                    var html = $"{ConfigurationManager.WebPath}\\SphApp\\views\\{name}.html";
                    if (Exists(html))
                        Copy(html, $"{tempFolder}\\SphApp\\views\\{name}.html", true);

                }

            }

            var scriptConfig = Server.MapPath(AppDataCustomScriptJson);
            if (Exists(scriptConfig))
            {
                var scripts = JsonConvert.DeserializeObject<CustomScript[]>(ReadAllText(scriptConfig));
                Copy(scriptConfig, $"{tempFolder}\\{Path.GetFileName(scriptConfig)}", true);

                foreach (var sc in scripts)
                {
                    var file = $"{ConfigurationManager.WebPath}\\SphApp\\services\\{sc.Name}.js";
                    if (Exists(file))
                        Copy(file, $"{tempFolder}\\SphApp\\services\\{sc.Name}.js");
                }
            }

            var dialogConfig = Server.MapPath(AppDataCustomDialogJson);
            if (Exists(dialogConfig))
            {
                var dialogs = JsonConvert.DeserializeObject<CustomDialog[]>(ReadAllText(dialogConfig));
                Copy(dialogConfig, $"{tempFolder}\\{Path.GetFileName(dialogConfig)}", true);

                foreach (var d in dialogs)
                {
                    var js = $"{ConfigurationManager.WebPath}\\SphApp\\viewmodels\\{d.Name}.js";
                    if (Exists(js))
                        Copy(js, $"{tempFolder}\\SphApp\\viewmodels\\{d.Name}.js", true);

                    var html = $"{ConfigurationManager.WebPath}\\SphApp\\views\\{d.Name}.html";
                    if (Exists(html))
                        Copy(html, $"{tempFolder}\\SphApp\\views\\{d.Name}.html", true);
                }
            }

            var viewConfig = Server.MapPath(AppDataCustomPartialViewJson);
            if (Exists(viewConfig))
            {
                var views = JsonConvert.DeserializeObject<CustomPartialView[]>(ReadAllText(viewConfig));
                Copy(viewConfig, $"{tempFolder}\\{Path.GetFileName(viewConfig)}", true);
                foreach (var v in views)
                {
                    var js = $"{ConfigurationManager.WebPath}\\SphApp\\viewmodels\\{v.Name}.js";
                    if (Exists(js))
                        Copy(js, $"{tempFolder}\\SphApp\\viewmodels\\{v.Name}.js", true);

                    var html = $"{ConfigurationManager.WebPath}\\SphApp\\views\\{v.Name}.html";
                    if (Exists(html))
                        Copy(html, $"{tempFolder}\\SphApp\\views\\{v.Name}.html", true);
                }
            }


            if (Exists(zip))
                Delete(zip);
            ZipFile.CreateFromDirectory(tempFolder, zip);


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

                    return Json(new { success = true, status = "OK" });

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
                Copy(js, $"{ConfigurationManager.WebPath}\\SphApp\\viewmodels\\{Path.GetFileName(js)}", true);
            }

            foreach (var html in Directory.GetFiles(folder, "*.html"))
            {
                Copy(html, $"{ConfigurationManager.WebPath}\\SphApp\\views\\{Path.GetFileName(html)}", true);
            }

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var customRouteConfig = Server.MapPath(CustomRouteConfig);
            var customJsRoutes = new List<JsRoute>();
            if (Exists(customRouteConfig))
            {
                var json = ReadAllText(customRouteConfig);
                customJsRoutes.AddRange(JsonConvert.DeserializeObject<JsRoute[]>(json, settings));
            }

            string importedRouteConfig = $"{folder}\\routes.config.json";
            var importedJson = ReadAllText(importedRouteConfig);
            var importedRoutes = JsonConvert.DeserializeObject<JsRoute[]>(importedJson).ToList();
            foreach (var route in importedRoutes)
            {
                if (customJsRoutes.All(x => x.Route != route.Route))
                {
                    customJsRoutes.Add(route);
                }
            }

            var text = JsonConvert.SerializeObject(customJsRoutes, Formatting.Indented, settings);
            WriteAllText(customRouteConfig, text);

        }
        private static void CopyFile(string fileName, string folder, string path)
        {
            var view = Path.Combine(ConfigurationManager.WebPath, $"SphApp\\{folder}\\{fileName}");
            if (Exists(view))
                Copy(view, $"{path}\\{fileName}", true);
        }

    }
}