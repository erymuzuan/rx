﻿using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize(Roles = "developers")]
    [RoutePrefix("api-rx/wf-designer")]
    public class WorkflowDesignerController : BaseApiController
    {
        [Route("toolbox-items")]
        //[OutputCache(Duration = 300)]
        public HttpResponseMessage GetToolboxItems()
        {
            var ds = ObjectBuilder.GetObject<IDeveloperService>();
            var actions = from a in ds.ActivityOptions
                          select
                              $@"
{{
    ""designer"" : {JsonConvert.SerializeObject(a.Metadata)
                                  },
    ""activity"" : {a.Value.ToJsonString()}
}}";


            return new JsonResponseMessage("[" + string.Join(",", actions) + "]");
        }

        [Route("icon/{name}.png")]
       // [OutputCache(Duration = 31600, Location = OutputCacheLocation.Any)]
        public HttpResponseMessage GetPngIcon(string name)
        {
            var ds = ObjectBuilder.GetObject<DeveloperService>();
            var act = ds.ActionOptions
                .SingleOrDefault(x => string.Equals(x.Metadata.TypeName, name, StringComparison.InvariantCultureIgnoreCase));
            if (null != act)
            {
                var png = act.Value.GetPngIcon();
                if (null != png)
                    return File(ImageToByte2(act.Value.GetPngIcon()), "image/png");
                return new HttpResponseMessage(HttpStatusCode.NotFound);
            }

            return new HttpResponseMessage(HttpStatusCode.NotFound);

        }

        [Route("editor/{name}.{extension:length(2,4)}")]
        public HttpResponseMessage GetDialog(string name, string extension)
        {
            var ds = ObjectBuilder.GetObject<DeveloperService>();
            var info = ds.ActivityOptions
                .SingleOrDefault(x => string.Equals(x.Metadata.TypeName, name, StringComparison.InvariantCultureIgnoreCase));
            if (null == info) return new HttpResponseMessage(HttpStatusCode.NotFound);
            var response = new HttpResponseMessage(HttpStatusCode.OK);
            if (extension == "js")
            {
                response.Content = new StringContent(info.Value.GetEditorViewModel());
                response.Headers.Add("Content-Type", "application/javascript");
            }
            else
            {
                response.Content = new StringContent(info.Value.GetEditorView());
                response.Headers.Add("Content-Type", "text/html");
            }

            return response;
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