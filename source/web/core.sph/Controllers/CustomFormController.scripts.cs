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
        [Route("script")]
        public ActionResult SaveCustomScript(CustomScript script)
        {
            var config = Server.MapPath(AppDataCustomScriptJson);
            var views = new List<CustomScript>();
            if (Exists(config))
                views = JsonConvert.DeserializeObject<List<CustomScript>>(ReadAllText(config), m_settings);

            if (views.All(x => x.Name != script.Name))
                views.Add(script);

            WriteAllText(config, JsonConvert.SerializeObject(views, Formatting.Indented, m_settings));
            var js = Server.MapPath($"~/SphApp/services/{script.Name}.js");
            if (!Exists(js))
                WriteAllText(js, Properties.Resources.CustomFormJs);

            return Json(true);
        }

        [HttpDelete]
        [Route("script/{name}")]
        public ActionResult DeleteScript(string name)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            var config = Server.MapPath(AppDataCustomPartialViewJson);
            var scripts = new List<CustomScript>();
            if (Exists(config))
                scripts = JsonConvert.DeserializeObject<List<CustomScript>>(ReadAllText(config), m_settings);

            var deleted = scripts.SingleOrDefault(x => x.Name == name);
            if (null == deleted) return HttpNotFound("Cannot find script with original name " + name);

            scripts.Remove(deleted);

            logger.Log(new LogEntry { Message = "Deleting script " + name, Details = JsonConvert.SerializeObject(deleted, Formatting.Indented, m_settings), Operation = "Delete", Severity = Severity.Log });
            WriteAllText(config, JsonConvert.SerializeObject(scripts, Formatting.Indented, m_settings));

            var js = Server.MapPath($"~/SphApp/services/{name}.js");
            if (Exists(js))
            {
                logger.Log(new LogEntry { Message = "Deleting file " + js, Details = ReadAllText(js), Operation = "Delete", Severity = Severity.Log });
                Delete(js);
            }
            return Json(true);
        }



        [HttpGet]
        [Route("scripts")]
        public ActionResult GetScripts()
        {
            var config = Server.MapPath(AppDataCustomScriptJson);
            return Content(!Exists(config) ? "[]" : ReadAllText(config), APPLICATION_JSON);
        }
        [HttpPost]
        [Route("rename/script")]
        public ActionResult RenameScript(CustomScript view)
        {
            var config = Server.MapPath(AppDataCustomScriptJson);
            var views = new List<CustomScript>();
            if (Exists(config))
                views = JsonConvert.DeserializeObject<List<CustomScript>>(ReadAllText(config), m_settings);

            var editedView = views.SingleOrDefault(x => x.Name == view.OriginalName);
            if (null == editedView) return HttpNotFound("Cannot find view with original name " + view.OriginalName);

            editedView.Name = view.Name;
            editedView.OriginalName = view.OriginalName;

            WriteAllText(config, JsonConvert.SerializeObject(views, Formatting.Indented, m_settings));

            var js = Server.MapPath($"~/SphApp/services/{view.OriginalName}.js");
            var js2 = Server.MapPath($"~/SphApp/services/{view.Name}.js");
            if (Exists(js))
                Move(js, js2);
            else
                WriteAllText(js2, Properties.Resources.CustomFormJs);


            return Json(true);
        }
    }
}