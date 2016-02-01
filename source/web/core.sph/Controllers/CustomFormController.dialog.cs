using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Models;
using Bespoke.Sph.Web.Properties;
using Newtonsoft.Json;
using static System.IO.File;

namespace Bespoke.Sph.Web.Controllers
{
    public partial class CustomFormController 
    {

        [HttpDelete]
        [Route("dialog/{name}")]
        public ActionResult DeleteDialog(string name)
        {
            var logger = ObjectBuilder.GetObject<ILogger>();
            var config = Server.MapPath(APP_DATA_CUSTOM_DIALOG_JSON);
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
        [Route("rename/dialog")]
        public ActionResult RenameDialog(CustomDialog dlg)
        {
            var config = Server.MapPath(APP_DATA_CUSTOM_DIALOG_JSON);
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
                WriteAllText(js2, Resources.CustomFormDialogJsTemplate);

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
            var config = Server.MapPath(APP_DATA_CUSTOM_DIALOG_JSON);
            return Content(!Exists(config) ? "[]" : ReadAllText(config), APPLICATION_JSON);
        }

        [HttpPost]
        [Route("dialog")]
        public ActionResult SaveCustomDialog(CustomDialog dialog)
        {
            var config = Server.MapPath(APP_DATA_CUSTOM_DIALOG_JSON);
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
                WriteAllText(js, Resources.CustomFormDialogJsTemplate);

            var html = Server.MapPath($"~/SphApp/views/{dialog.Name}.html");
            if (!Exists(html))
            {
                WriteAllText(html, Resources.CustomFormDialogHtmlTemplate);
            }

            return Json(true);
        }

    }
}