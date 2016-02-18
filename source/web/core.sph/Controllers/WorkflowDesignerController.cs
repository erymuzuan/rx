using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize(Roles = "developers")]
    [RoutePrefix("api-rx/wf-designer")]
    public class WorkflowDesignerController : BaseApiController
    {
        [Route("toolbox-items")]
        public HttpResponseMessage GetToolboxItems()
        {
            var actions = from a in this.DeveloperService.ActivityOptions
                          select
                              $@"
{{
    ""designer"" : {JsonConvert.SerializeObject(a.Metadata)
                                  },
    ""activity"" : {a.Value.ToJsonString()}
}}";


            var response = new JsonResponseMessage("[" + string.Join(",", actions) + "]");
            response.Headers.CacheControl = new CacheControlHeaderValue
            {
                MaxAge = TimeSpan.FromSeconds(300),
                Private = false
            };

            return response;
        }

        [Route("icon/{name}.png")]
        public IHttpActionResult GetPngIcon(string name)
        {
            var act = this.DeveloperService.ActionOptions
                .SingleOrDefault(x => string.Equals(x.Metadata.TypeName, name, StringComparison.InvariantCultureIgnoreCase));
            if (null != act)
            {
                var png = act.Value.GetPngIcon();
                if (null != png)
                    return File(ImageToByte2(act.Value.GetPngIcon()), "image/png",null,3600);
                return NotFound();
            }

            return NotFound();

        }

        [Route("editor/{extension:length(2,4)}/{name}")]
        public IHttpActionResult GetDialog(string name, string extension)
        {
            var info = this.DeveloperService.ActivityOptions
                .SingleOrDefault(x => string.Equals(x.Metadata.TypeName, name, StringComparison.InvariantCultureIgnoreCase));
            if (null == info) return NotFound();
            if (extension == "js")
            {
                return Javascript(info.Value.GetEditorViewModel());
            }
            return Html(info.Value.GetEditorView());
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
        public IHttpActionResult GetLoadedAssemblies()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            return Ok(assemblies.Select(a => a.GetName().Name).ToArray());
        }

        [HttpGet]
        [Route("types/{dll}")]
        public IHttpActionResult GetTypeFromAssembly(string dll)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly = assemblies.SingleOrDefault(a => a.GetName().Name == dll);
            if (null == assembly)
                return NotFound("Cannot find assembly with the name " + dll);

            return Ok(assembly.GetTypes()
                .Where(a => !a.FullName.Contains("<"))
                .Where(a => !a.FullName.Contains("`"))
                .Select(a => a.FullName).ToArray());
        }

        [HttpGet]
        [Route("methods/{dll}/{type}")]
        public IHttpActionResult GetMethodFromType(string dll, string type)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            var assembly = assemblies.SingleOrDefault(a => a.GetName().Name == dll);
            if (null == assembly)
                return NotFound();

            var @class = assembly.GetTypes().SingleOrDefault(t => t.FullName == type);
            if (null == @class)
                return NotFound("Cannot find type with the name " + type + " in " + dll);

            return Ok(@class.GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .Where(m => !m.IsAbstract)
                .Where(m => !m.Name.StartsWith("get_"))
                .Where(m => !m.Name.StartsWith("set_"))
                .Where(m => m.DeclaringType == @class)
                .Select(a => a.Name).ToArray());
        }

    }
}