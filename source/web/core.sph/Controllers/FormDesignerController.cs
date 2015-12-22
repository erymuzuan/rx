using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.UI;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Dependencies;
using Bespoke.Sph.Web.Helpers;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("form-designer")]
    public class FormDesignerController : Controller
    {

        [Route("toolbox-items")]
        [OutputCache(Duration = 64800, Location = OutputCacheLocation.Any)]
        public ActionResult GetToolboxItems()
        {
            var ds = ObjectBuilder.GetObject<DeveloperService>();
            var actions = from a in ds.ToolboxItems
                          orderby a.Metadata.Order
                          select
                              $@"
{{
    ""designer"" : {JsonConvert.SerializeObject(a.Metadata)
                                  },
    ""element"" : {a.Value.ToJsonString()}
}}";


            return Content("[" + string.Join(",", actions) + "]", "application/json", Encoding.UTF8);
        }

        [Route("icon/{name}.png")]
        [OutputCache(Duration = 2592200, Location = OutputCacheLocation.Any)]
        public ActionResult GetPngIcon(string name)
        {
            var ds = ObjectBuilder.GetObject<DeveloperService>();

            var act = ds.ToolboxItems
                .SingleOrDefault(x => string.Equals(x.Metadata.Name, name, StringComparison.InvariantCultureIgnoreCase));
            if (null != act)
            {
                var png = act.Value.GetPngIcon();
                if (null != png)
                    return File(ImageToByte2(act.Value.GetPngIcon()), "image/png");
                return HttpNotFound("Cannot find any image for " + name);
            }

            return HttpNotFound("Cannot find any activity named " + name);

        }


        [HttpPost]
        [Route("render/{name}")]
        public async Task<ActionResult> Render(string name, [RequestBody]EntityForm form)
        {
            await form.RenderAsync(name);
            return Json(new { success = true, status = "OK", id = form.Id });
        }

        [Route("editor/{name}.{extension:length(2,4)}")]
        public ActionResult GetDialog(string name, string extension)
        {
            var ds = ObjectBuilder.GetObject<DeveloperService>();
            var info = ds.ToolboxItems
                .SingleOrDefault(x => string.Equals(x.Metadata.Name, name, StringComparison.InvariantCultureIgnoreCase));
            if (null != info)
            {
                if (extension == "js")
                    return Content(info.Value.GetEditorViewModel(), "application/javascript", Encoding.UTF8);
                return Content(info.Value.GetEditorView(), "text/html", Encoding.UTF8);
            }

            return HttpNotFound("Cannot find any form element dialog  for" + name);
        }

        public static byte[] ImageToByte2(Bitmap img)
        {
            byte[] byteArray;
            using (var stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Close();

                byteArray = stream.ToArray();
            }
            return byteArray;
        }

    }
}