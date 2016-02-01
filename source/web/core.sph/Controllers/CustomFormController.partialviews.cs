using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Models;
using Newtonsoft.Json;
using static System.IO.File;

namespace Bespoke.Sph.Web.Controllers
{
    public partial class CustomFormController
    {
        [HttpPost]
        [Route("partial-view")]
        public ActionResult SaveCustomPartialView(CustomPartialView view)
        {
            var config = Server.MapPath(APP_DATA_CUSTOM_PARTIAL_VIEW_JSON);
            var views = new List<CustomPartialView>();
            if (Exists(config))
                views = JsonConvert.DeserializeObject<List<CustomPartialView>>(ReadAllText(config), m_settings);

            if (views.All(x => x.Name != view.Name))
                views.Add(view);

            WriteAllText(config, JsonConvert.SerializeObject(views, Formatting.Indented, m_settings));
            var js = Server.MapPath($"~/SphApp/viewmodels/{view.Name}.js");
            if (Exists(js) && view.UseViewModel)
                WriteAllText(js, Properties.Resources.CustomFormJs);

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
            var config = Server.MapPath(APP_DATA_CUSTOM_PARTIAL_VIEW_JSON);
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



        [HttpPost]
        [Route("rename/partial-view")]
        public ActionResult RenamePartialView(CustomPartialView view)
        {
            var config = Server.MapPath(APP_DATA_CUSTOM_PARTIAL_VIEW_JSON);
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
                WriteAllText(js2, Properties.Resources.CustomFormJs);

            var html = Server.MapPath($"~/SphApp/views/{view.OriginalName}.html");
            var html2 = Server.MapPath($"~/SphApp/views/{view.Name}.html");
            if (Exists(html))
                Move(html, html2);

            return Json(true);
        }
        [HttpGet]
        [Route("partial-views")]
        public ActionResult PartialViews()
        {
            var config = Server.MapPath(APP_DATA_CUSTOM_PARTIAL_VIEW_JSON);
            return Content(!Exists(config) ? "[]" : ReadAllText(config), APPLICATION_JSON);
        }

    }
}