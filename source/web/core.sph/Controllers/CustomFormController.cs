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
    public class CustomFormController : BaseAppController
    {
        private const string ViewModel = @"
define([""services/datacontext""],
    function(context)
        {

            var activate = function() {
                    return true;

                },
                attached = function(view) {

                };

            var vm = {
            activate : activate,
            attached: attached
        };


        return vm;

    });";
        private const string DialogViewModel = @"
define([""plugins/dialog""],
    function(dialog)
        {

            var dlg = ko.observable(),
                okClick = function(data, ev) {
                if (bespoke.utils.form.checkValidity(ev.target))
                {
                    dialog.close(this, ""OK"");
                }

            },
            cancelClick = function() {
                dialog.close(this, ""Cancel"");
            };

            var vm = {
            dlg:dlg,
            okClick: okClick,
            cancelClick:
            cancelClick
        };


        return vm;

    });";
        readonly JsonSerializerSettings m_settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };

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
            if (Exists(customRouteConfig))
            {
                var json = ReadAllText(customRouteConfig);
                customJsRoutes = JsonConvert.DeserializeObject<JsRoute[]>(json, settings);
                Copy(customRouteConfig, $"{path}\\routes.config.json", true);
            }

            foreach (var form in customJsRoutes)
            {
                CopyFile(form.ModuleId.Replace("viewmodels/", "") + ".html", "views", path);
                CopyFile(form.ModuleId.Replace("viewmodels/", "") + ".js", "viewmodels", path);
            }


            if (Exists(zip))
                Delete(zip);
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

        [HttpPost]
        [Route("partial-view")]
        public ActionResult SaveCustomPartialView(CustomPartialView view)
        {
            var config = Server.MapPath("~/App_Data/custom-partial-view.json");
            var views = new List<CustomPartialView>();
            if (Exists(config))
                views = JsonConvert.DeserializeObject<List<CustomPartialView>>(ReadAllText(config), m_settings);

            if (views.All(x => x.Name != view.Name))
                views.Add(view);

            WriteAllText(config, JsonConvert.SerializeObject(views, Formatting.Indented, m_settings));
            var js = Server.MapPath($"~/SphApp/viewmodels/{view.Name}.js");
            if (Exists(js) && view.UseViewModel)
                WriteAllText(js, ViewModel);

            var html = Server.MapPath($"~/SphApp/views/{view.Name}.html");
            if (!Exists(html))
                WriteAllText(html, "<div></div>");

            return Json(true);
        }

        [HttpDelete]
        [Route("partial-view/{name}")]
        public ActionResult DeletePartialView(string name)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            var config = Server.MapPath("~/App_Data/custom-partial-view.json");
            var views = new List<CustomPartialView>();
            if (Exists(config))
                views = JsonConvert.DeserializeObject<List<CustomPartialView>>(ReadAllText(config), m_settings);

            var deleted = views.SingleOrDefault(x => x.Name == name);
            if (null == deleted) return HttpNotFound("Cannot find view with original name " + name);

            views.Remove(deleted);

            logger.Log(new LogEntry { Message = "Deleting Partial View " + name, Details = JsonConvert.SerializeObject(deleted, Formatting.Indented, m_settings), Operation = "Delete", Severity = Severity.Log });
            WriteAllText(config, JsonConvert.SerializeObject(views, Formatting.Indented, m_settings));

            var js = Server.MapPath($"~/SphApp/viewmodels/{name}.js");
            if (Exists(js))
            {
                logger.Log(new LogEntry { Message = "Deleting file " + js, Details = ReadAllText(js), Operation = "Delete", Severity = Severity.Log });
                Delete(js);
            }

            var html = Server.MapPath($"~/SphApp/views/{name}.html");
            if (Exists(html))
            {
                logger.Log(new LogEntry { Message = "Deleting file " + html, Details = ReadAllText(html), Operation = "Delete", Severity = Severity.Log });
                Delete(html);
            }
            return Json(true);
        }

        [HttpDelete]
        [Route("dialog/{name}")]
        public ActionResult DeleteDialog(string name)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            var config = Server.MapPath("~/App_Data/custom-dialog.json");
            var dialogs = new List<CustomDialog>();
            if (Exists(config))
                dialogs = JsonConvert.DeserializeObject<List<CustomDialog>>(ReadAllText(config), m_settings);

            var deleted = dialogs.SingleOrDefault(x => x.Name == name);
            if (null == deleted) return HttpNotFound("Cannot find dialog name " + name);

            dialogs.Remove(deleted);
            logger.Log(new LogEntry { Message = "Deleting Dialog " + name, Details = JsonConvert.SerializeObject(deleted, Formatting.Indented, m_settings), Operation = "Delete", Severity = Severity.Log });
            WriteAllText(config, JsonConvert.SerializeObject(dialogs, Formatting.Indented, m_settings));

            var js = Server.MapPath($"~/SphApp/viewmodels/{name}.js");
            if (Exists(js))
            {
                logger.Log(new LogEntry { Message = "Deleting file " + js, Details = ReadAllText(js), Operation = "Delete", Severity = Severity.Log });
                Delete(js);
            }

            var html = Server.MapPath($"~/SphApp/views/{name}.html");
            if (Exists(html))
            {
                logger.Log(new LogEntry { Message = "Deleting file " + html, Details = ReadAllText(html), Operation = "Delete", Severity = Severity.Log });
                Delete(html);
            }
            return Json(true);
        }


        [HttpPost]
        [Route("rename/partial-view")]
        public ActionResult RenamePartialView(CustomPartialView view)
        {
            var config = Server.MapPath("~/App_Data/custom-partial-view.json");
            var views = new List<CustomPartialView>();
            if (Exists(config))
                views = JsonConvert.DeserializeObject<List<CustomPartialView>>(ReadAllText(config), m_settings);

            var editedView = views.SingleOrDefault(x => x.Name == view.OriginalName);
            if (null == editedView) return HttpNotFound("Cannot find view with original name " + view.OriginalName);

            editedView.Name = view.Name;
            editedView.UseViewModel = view.UseViewModel;
            editedView.OriginalName = view.OriginalName;

            WriteAllText(config, JsonConvert.SerializeObject(views, Formatting.Indented, m_settings));

            var js = Server.MapPath($"~/SphApp/viewmodels/{view.OriginalName}.js");
            var js2 = Server.MapPath($"~/SphApp/viewmodels/{view.Name}.js");
            if (Exists(js) && view.UseViewModel)
                Move(js, js2);
            if (!Exists(js) && view.UseViewModel)
                WriteAllText(js2, ViewModel);

            var html = Server.MapPath($"~/SphApp/views/{view.OriginalName}.html");
            var html2 = Server.MapPath($"~/SphApp/views/{view.Name}.html");
            if (Exists(html))
                Move(html, html2);

            return Json(true);
        }

        [HttpPost]
        [Route("rename/dialog")]
        public ActionResult RenameDialog(CustomDialog dlg)
        {
            var config = Server.MapPath("~/App_Data/custom-dialog.json");
            var dialogs = new List<CustomDialog>();
            if (Exists(config))
                dialogs = JsonConvert.DeserializeObject<List<CustomDialog>>(ReadAllText(config), m_settings);

            var editedDialog = dialogs.SingleOrDefault(x => x.Name == dlg.OriginalName);
            if (null == editedDialog) return HttpNotFound("Cannot find view with original name " + dlg.OriginalName);

            editedDialog.Name = dlg.Name;
            editedDialog.OriginalName = dlg.OriginalName;

            WriteAllText(config, JsonConvert.SerializeObject(dialogs, Formatting.Indented, m_settings));

            var js = Server.MapPath($"~/SphApp/viewmodels/{dlg.OriginalName}.js");
            var js2 = Server.MapPath($"~/SphApp/viewmodels/{dlg.Name}.js");
            if (Exists(js))
                Move(js, js2);
            else
                WriteAllText(js2, DialogViewModel);

            var html = Server.MapPath($"~/SphApp/views/{dlg.OriginalName}.html");
            var html2 = Server.MapPath($"~/SphApp/views/{dlg.Name}.html");
            if (Exists(html))
                Move(html, html2);

            return Json(true);
        }

        [HttpGet]
        [Route("dialogs")]
        public ActionResult Dialogs()
        {
            var config = Server.MapPath("~/App_Data/custom-dialog.json");
            return Content(!Exists(config) ? "[]" : ReadAllText(config), APPLICATION_JSON);
        }
        [HttpGet]
        [Route("partial-views")]
        public ActionResult PartialViews()
        {
            var config = Server.MapPath("~/App_Data/custom-partial-view.json");
            return Content(!Exists(config) ? "[]" : ReadAllText(config), APPLICATION_JSON);
        }

        [HttpPost]
        [Route("dialog")]
        public ActionResult SaveCustomDialog(CustomDialog dialog)
        {
            var config = Server.MapPath("~/App_Data/custom-dialog.json");
            var dialogs = new List<CustomDialog>();
            if (Exists(config))
            {
                dialogs = JsonConvert.DeserializeObject<List<CustomDialog>>(ReadAllText(config), m_settings);
            }
            if (dialogs.All(x => x.Name != dialog.Name))
            {
                dialogs.Add(dialog);
            }
            WriteAllText(config, JsonConvert.SerializeObject(dialogs, Formatting.Indented, m_settings));

            var js = Server.MapPath($"~/SphApp/viewmodels/{dialog.Name}.js");
            if (!Exists(js))
                WriteAllText(js, DialogViewModel);

            var html = Server.MapPath($"~/SphApp/views/{dialog.Name}.html");
            if (!Exists(html))
            {
                WriteAllText(html, "<div></div>");
            }

            return Json(true);
        }

    }
}