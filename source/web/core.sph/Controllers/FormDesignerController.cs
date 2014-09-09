using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("form-designer")]
    public class FormDesignerController : Controller
    {
        [ImportMany("FormDesigner", typeof(FormElement), AllowRecomposition = true)]
        public Lazy<FormElement, IDesignerMetadata>[] ToolboxItems { get; set; }

        [Route("toolbox-items")]
        public ActionResult GetToolboxItems()
        {
            if (null == this.ToolboxItems)
                ObjectBuilder.ComposeMefCatalog(this);
            var actions = from a in this.ToolboxItems
                          orderby a.Metadata.Order
                          select string.Format(@"
{{
    ""designer"" : {0},
    ""element"" : {1}
}}", JsonConvert.SerializeObject(a.Metadata), a.Value.ToJsonString());


            return Content("[" + string.Join(",", actions) + "]", "application/json", Encoding.UTF8);
        }

        [Route("icon/{name}.png")]
        public ActionResult GetPngIcon(string name)
        {
            if (null == this.ToolboxItems)
                ObjectBuilder.ComposeMefCatalog(this);

            var act = this.ToolboxItems
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
            if (null == this.ToolboxItems)
                ObjectBuilder.ComposeMefCatalog(this);

            var info = this.ToolboxItems
                .SingleOrDefault(x => string.Equals(x.Metadata.Name, name, StringComparison.InvariantCultureIgnoreCase));
            if (null != info)
            {
                if (extension == "js")
                    return Content(info.Value.GetEditorViewModel(), "application/javascript", Encoding.UTF8);
                return Content(info.Value.GetEditorView(), "text/html", Encoding.UTF8);
            }

            return HttpNotFound("Cannot find any activity named " + name);
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