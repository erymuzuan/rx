using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using System.Web.UI;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Dependencies;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize(Roles = "developers")]
    [RoutePrefix("wf-designer")]
    public class WorkflowDesignerController : Controller
    {
        static WorkflowDesignerController()
        {
            DeveloperService.Init();
        }
        [Route("toolbox-items")]
        [OutputCache(Duration = 300)]
        public ActionResult GetToolboxItems()
        {
            var ds = ObjectBuilder.GetObject<DeveloperService>();
            var actions = from a in ds.ActivityOptions
                          select
                              $@"
{{
    ""designer"" : {JsonConvert.SerializeObject(a.Metadata)
                                  },
    ""activity"" : {a.Value.ToJsonString()}
}}";


            return Content("[" + string.Join(",", actions) + "]", "application/json", Encoding.UTF8);
        }

        [Route("icon/{name}.png")]
        [OutputCache(Duration = 31600, Location = OutputCacheLocation.Any)]
        public ActionResult GetPngIcon(string name)
        {
            var ds = ObjectBuilder.GetObject<DeveloperService>();
            var act = ds.ActionOptions
                .SingleOrDefault(x => string.Equals(x.Metadata.TypeName, name, StringComparison.InvariantCultureIgnoreCase));
            if (null != act)
            {
                var png = act.Value.GetPngIcon();
                if (null != png)
                    return File(ImageToByte2(act.Value.GetPngIcon()), "image/png");
                return HttpNotFound("Cannot find any image for " + name);
            }

            return HttpNotFound("Cannot find any activity named " + name);

        }
        [Route("editor/{name}.{extension:length(2,4)}")]
        public ActionResult GetDialog(string name, string extension)
        {
            var ds = ObjectBuilder.GetObject<DeveloperService>();
            var info = ds.ActivityOptions
                .SingleOrDefault(x => string.Equals(x.Metadata.TypeName, name, StringComparison.InvariantCultureIgnoreCase));
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

        [HttpGet]
        [Route("assemblies")]
        public ActionResult GetLoadedAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return Json(assemblies.Select(a => a.GetName().Name).ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("types/{dll}")]
        public ActionResult GetTypeFromAssembly(string dll)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly = assemblies.SingleOrDefault(a => a.GetName().Name == dll);
            if (null == assembly)
                return HttpNotFound("Cannot find assembly with the name " + dll);

            return Json(assembly.GetTypes()
                .Where(a => !a.FullName.Contains("<"))
                .Where(a => !a.FullName.Contains("`"))
                .Select(a => a.FullName).ToArray(), JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [Route("methods/{dll}/{type}")]
        public ActionResult GetMethodFromType(string dll, string type)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly = assemblies.SingleOrDefault(a => a.GetName().Name == dll);
            if (null == assembly)
                return HttpNotFound("Cannot find assembly with the name " + dll);

            var @class = assembly.GetTypes().SingleOrDefault(t => t.FullName == type);
            if (null == @class)
                return HttpNotFound("Cannot find type with the name " + type + " in " + dll);

            return Json(@class.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => !m.IsAbstract)
                .Where(m => !m.Name.StartsWith("get_"))
                .Where(m => !m.Name.StartsWith("set_"))
                .Where(m => m.DeclaringType == @class)
                .Select(a => a.Name).ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}