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
using Bespoke.Sph.Web.ViewModels;
using DiffPlex;
using DiffPlex.DiffBuilder;
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
        public const string CUSTOM_ROUTE_CONFIG = "~/App_Data/routes.config.json";
        public const string APP_DATA_CUSTOM_PARTIAL_VIEW_JSON = "~/App_Data/custom-partial-view.json";
        public const string APP_DATA_CUSTOM_DIALOG_JSON = "~/App_Data/custom-dialog.json";
        public const string APP_DATA_CUSTOM_SCRIPT_JSON = "~/App_Data/custom-script.json";

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
            var customRouteConfig = Server.MapPath(CUSTOM_ROUTE_CONFIG);
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

            var scriptConfig = Server.MapPath(APP_DATA_CUSTOM_SCRIPT_JSON);
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

            var dialogConfig = Server.MapPath(APP_DATA_CUSTOM_DIALOG_JSON);
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

            var viewConfig = Server.MapPath(APP_DATA_CUSTOM_PARTIAL_VIEW_JSON);
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
                    var result = UnpackAsync(zip, folder);

                    return Content(JsonConvert.SerializeObject(result, Formatting.Indented, m_settings), "application/json");

                }
            }
            catch (Exception e)
            {
                return Json(new { success = false, exception = e.GetType().FullName, message = e.Message, stack = e.StackTrace });
            }
            return Json(new { success = false });


        }
        private object UnpackAsync(string zipFile, string folder)
        {
            var setting = new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All };
            setting.Converters.Add(new StringEnumConverter());

            ZipFile.ExtractToDirectory(zipFile, folder);

            var routesConfig = $"{folder}\\routes.config.json";
            var forms = new List<JsRoute>();
            if (Exists(routesConfig))
                forms.AddRange(JsonConvert.DeserializeObject<JsRoute[]>(ReadAllText(routesConfig)));

            var dialogsConfig = $"{folder}\\custom-dialog.json";
            var dialogs = new List<CustomDialog>();
            if (Exists(dialogsConfig))
                dialogs.AddRange(JsonConvert.DeserializeObject<CustomDialog[]>(ReadAllText(dialogsConfig)));


            var scriptsConfig = $"{folder}\\custom-script.json";
            var scripts = new List<CustomScript>();
            if (Exists(scriptsConfig))
                scripts.AddRange(JsonConvert.DeserializeObject<CustomScript[]>(ReadAllText(scriptsConfig)));

            var viewsConfig = $"{folder}\\custom-partial-view.json";
            var views = new List<CustomPartialView>();
            if (Exists(viewsConfig))
                views.AddRange(JsonConvert.DeserializeObject<CustomPartialView[]>(ReadAllText(viewsConfig)));

            // TODO : return files that changed, added or removed only
            foreach (var sc in scripts)
            {
                sc.JsDiff = sc.GetDiff(folder);
            }
            foreach (var dlg in dialogs)
            {
                dlg.JsDiff = dlg.GetJsDiff(folder);
                dlg.HtmlDiff = dlg.GetHtmlDiff(folder);
            }

            var myViewConfig = Server.MapPath(APP_DATA_CUSTOM_PARTIAL_VIEW_JSON);
            var myViews = new CustomPartialView[] { };
            if (Exists(myViewConfig))
                myViews = JsonConvert.DeserializeObject<CustomPartialView[]>(ReadAllText(myViewConfig));

            foreach (var vw in myViews)
            {
                if (views.All(x => x.Name != vw.Name))
                {
                    vw.HtmlDiff = "deleted";
                    vw.JsDiff = "deleted";
                    views.Add(vw);
                }
            }
            foreach (var vw in views)
            {
                vw.JsDiff = vw.GetJsDiff(folder);
                vw.HtmlDiff = vw.GetHtmlDiff(folder);
            }

            var routes = from r in forms
                         let jsDiff = r.GetJsDiff(folder)
                         let htmlDiff = r.GetHtmlDiff(folder)
                         select new
                         {
                             r.Route,
                             r.ModuleId,
                             r.Role,
                             r.Caption,
                             r.Title,
                             r.Icon,
                             r.Nav,
                             JsDiff = jsDiff,
                             HtmlDiff = htmlDiff
                         };

            return new
            {
                success = true,
                message = "",
                folder,
                routes,
                dialogs,
                views,
                scripts
            };



        }

        [HttpPost]
        [Route("import")]
        public ActionResult Import(string file, string folder, string diff = "changed", string type = "")
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            var fullName = $"{folder}\\{file}";
            var destination = $"{ConfigurationManager.WebPath}\\{file}";

            if (!Exists(fullName))
                return HttpNotFound("Cannot find " + fullName);

            if (Exists(destination))
                logger.Log(new LogEntry { Message = "Apply Patch : " + destination, Details = ReadAllText(destination), Operation = "Apply Patch", Severity = Severity.Log });

            Copy(fullName, destination, true);


            if (diff == "changed")
                return Json(new { success = true, status = "OK" });

            // entry into the config for added
            if (diff == "added")
            {
                switch (type)
                {
                    case "partial-view":
                        var vw = new CustomPartialView { Name = Path.GetFileNameWithoutExtension(fullName), UseViewModel = true };
                        var viewConfig = Server.MapPath(APP_DATA_CUSTOM_PARTIAL_VIEW_JSON);
                        var views = JsonConvert.DeserializeObject<CustomPartialView[]>(ReadAllText(viewConfig), m_settings)
                            .ToList();
                        if (views.Any(x => x.Name == vw.Name)) break;
                        views.Add(vw);
                        WriteAllText(viewConfig, JsonConvert.SerializeObject(views, Formatting.Indented, m_settings));
                        break;
                    case "route":
                        var moduleId = "viewmodels/" + Path.GetFileNameWithoutExtension(fullName);
                        var importedRoutes = JsonConvert.DeserializeObject<JsRoute[]>(ReadAllText($"{folder}\\routes.config.json"), m_settings);
                        var form = importedRoutes.FirstOrDefault(x => x.ModuleId == moduleId);
                        if (null == form) break;

                        var routeConfig = Server.MapPath(CUSTOM_ROUTE_CONFIG);
                        var routes = JsonConvert.DeserializeObject<JsRoute[]>(ReadAllText(routeConfig), m_settings)
                            .ToList();
                        if (routes.Any(x => x.ModuleId == form.ModuleId)) break;
                        routes.Add(form);
                        WriteAllText(routeConfig, JsonConvert.SerializeObject(routes, Formatting.Indented, m_settings));
                        break;
                    case "dialog":
                        var dlg = new CustomDialog { Name = Path.GetFileNameWithoutExtension(fullName) };
                        var dlgConfig = Server.MapPath(APP_DATA_CUSTOM_DIALOG_JSON);
                        var dialogs = JsonConvert.DeserializeObject<CustomDialog[]>(ReadAllText(dlgConfig), m_settings)
                            .ToList();
                        if (dialogs.Any(x => x.Name == dlg.Name)) break;
                        dialogs.Add(dlg);
                        WriteAllText(dlgConfig, JsonConvert.SerializeObject(dialogs, Formatting.Indented, m_settings));
                        break;
                    case "script":
                        var sc = new CustomScript { Name = Path.GetFileNameWithoutExtension(fullName) };
                        var scConfig = Server.MapPath(APP_DATA_CUSTOM_SCRIPT_JSON);
                        var scripts = JsonConvert.DeserializeObject<CustomScript[]>(ReadAllText(scConfig), m_settings)
                            .ToList();
                        if (scripts.Any(x => x.Name == sc.Name)) break;
                        scripts.Add(sc);
                        WriteAllText(scConfig, JsonConvert.SerializeObject(scripts, Formatting.Indented, m_settings));
                        break;
                    default:
                        throw new ArgumentException("Cannot find " + type, nameof(type));
                }
            }

            return Json(new { success = true, status = "OK" });

        }
        [HttpPost]
        [Route("import-all")]
        public ActionResult ImportAll(string folder)
        {
            foreach (var js in Directory.GetFiles($"{folder}\\SphApp\\viewmodels", "*.js"))
            {
                Copy(js, $"{ConfigurationManager.WebPath}\\SphApp\\viewmodels\\{Path.GetFileName(js)}", true);
            }
            foreach (var js in Directory.GetFiles($"{folder}\\SphApp\\services", "*.js"))
            {
                Copy(js, $"{ConfigurationManager.WebPath}\\SphApp\\viewmodels\\{Path.GetFileName(js)}", true);
            }

            foreach (var html in Directory.GetFiles($"{folder}\\SphApp\\views", "*.html"))
            {
                Copy(html, $"{ConfigurationManager.WebPath}\\SphApp\\views\\{Path.GetFileName(html)}", true);
            }

            ImportAllRoutes(folder);
            ImportAllDialogs(folder);
            ImportAllViews(folder);
            ImportAllScripts(folder);


            return Json(new { success = true, status = "OK" });

        }

        private void ImportAllRoutes(string folder)
        {
            var customRouteConfig = Server.MapPath(CUSTOM_ROUTE_CONFIG);
            var customJsRoutes = new List<JsRoute>();
            if (Exists(customRouteConfig))
            {
                var json = ReadAllText(customRouteConfig);
                customJsRoutes.AddRange(JsonConvert.DeserializeObject<JsRoute[]>(json, m_settings));
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

            var combinedRoutesConfigJson = JsonConvert.SerializeObject(customJsRoutes, Formatting.Indented, m_settings);
            WriteAllText(customRouteConfig, combinedRoutesConfigJson);
        }

        private void ImportAllDialogs(string folder)
        {
            var dialogConfig = Server.MapPath(APP_DATA_CUSTOM_DIALOG_JSON);
            var dialogs = new List<CustomDialog>();
            if (Exists(dialogConfig))
            {
                var json = ReadAllText(dialogConfig);
                dialogs.AddRange(JsonConvert.DeserializeObject<CustomDialog[]>(json, m_settings));
            }

            string importedDialogConfig = $"{folder}\\custom-form-dialog.json";
            var importedDialogConfigJson = ReadAllText(importedDialogConfig);
            var importedDialogs = JsonConvert.DeserializeObject<CustomDialog[]>(importedDialogConfigJson).ToList();
            foreach (var dlg in importedDialogs)
            {
                if (dialogs.All(x => x.Name != dlg.Name))
                {
                    dialogs.Add(dlg);
                }
            }

            var combinedDialogConfigJson = JsonConvert.SerializeObject(dialogs, Formatting.Indented, m_settings);
            WriteAllText(dialogConfig, combinedDialogConfigJson);
        }
        private void ImportAllScripts(string folder)
        {
            var config = Server.MapPath(APP_DATA_CUSTOM_SCRIPT_JSON);
            var scripts = new List<CustomScript>();
            if (Exists(config))
            {
                var json = ReadAllText(config);
                scripts.AddRange(JsonConvert.DeserializeObject<CustomScript[]>(json, m_settings));
            }

            string importedDialogConfig = $"{folder}\\custom-form-script.json";
            var importConfigJson = ReadAllText(importedDialogConfig);
            var importedScripts = JsonConvert.DeserializeObject<CustomScript[]>(importConfigJson).ToList();
            foreach (var sc in importedScripts)
            {
                if (scripts.All(x => x.Name != sc.Name))
                {
                    scripts.Add(sc);
                }
            }

            var combineConfigJson = JsonConvert.SerializeObject(scripts, Formatting.Indented, m_settings);
            WriteAllText(config, combineConfigJson);
        }

        private void ImportAllViews(string folder)
        {
            var config = Server.MapPath(APP_DATA_CUSTOM_PARTIAL_VIEW_JSON);
            var views = new List<CustomPartialView>();
            if (Exists(config))
            {
                var json = ReadAllText(config);
                views.AddRange(JsonConvert.DeserializeObject<CustomPartialView[]>(json, m_settings));
            }

            string importedDialogConfig = $"{folder}\\custom-form-partial-view.json";
            var importedDialogConfigJson = ReadAllText(importedDialogConfig);
            var importedDialogs = JsonConvert.DeserializeObject<CustomPartialView[]>(importedDialogConfigJson).ToList();
            foreach (var dlg in importedDialogs)
            {
                if (views.All(x => x.Name != dlg.Name))
                {
                    views.Add(dlg);
                }
            }

            var combinedConfigJson = JsonConvert.SerializeObject(views, Formatting.Indented, m_settings);
            WriteAllText(config, combinedConfigJson);
        }

        [Route("compare")]
        public ActionResult Compare(string folder, string file)
        {

            var imported = ReadAllText($"{folder}\\{file}");
            var mine = ReadAllText($"{ConfigurationManager.WebPath}\\{file}");
            var vm = new PageCompareViewModel { Latest = imported, Old = mine, LogId = "" };


            var diffBuilder = new SideBySideDiffBuilder(new Differ());
            var diff = diffBuilder.BuildDiffModel(vm.Old, vm.Latest);
            vm.Diff = diff;
            return View(vm);
        }
    }
}