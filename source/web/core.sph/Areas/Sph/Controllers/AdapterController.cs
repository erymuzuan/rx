using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize(Roles = "developers, administrators")]
    [RoutePrefix("adapter")]
    public class AdapterController : Controller
    {

        static AdapterController()
        {
            DeveloperService.Init();
        }
        [Route("installed-adapters")]
        public ActionResult InstalledAdapters()
        {
            var developerService = ObjectBuilder.GetObject<DeveloperService>();
            var actions = from a in developerService.Adapters
                          select
                              $@"
{{
    ""designer"" : {JsonConvert.SerializeObject(a.Metadata)},
    ""adapter"" : {a.Value.ToJsonString()}
}}";


            return Content("[" + string.Join(",", actions) + "]", "application/json", Encoding.UTF8);

        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(string id)
        {
            var context = new SphDataContext();
            var ef = await context.LoadOneAsync<Adapter>(x => x.Id == id);
            if (null == ef)
                return HttpNotFound("Cannot find adapter with id " + id);

            using (var session = context.OpenSession())
            {
                session.Delete(ef);
                await session.SubmitChanges("Delete");
            }
            return Json(new { success = true, status = "OK", id = ef.Id });
        }
        [Route("")]
        [HttpDelete]
        public async Task<ActionResult> DeleteAsync()
        {
            var ef = this.GetRequestJson<Adapter>();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Delete(ef);
                await session.SubmitChanges("Delete");
            }
            return Json(new { success = true, status = "OK", id = ef.Id });
        }

        [Route("")]
        [HttpPost]
        public async Task<ActionResult> Create()
        {
            var adapter = this.GetRequestJson<Adapter>();
            if (null == adapter)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Cannot deserialize adapter");

            var context = new SphDataContext();
            adapter.Id = adapter.Name.ToIdFormat();

            using (var session = context.OpenSession())
            {
                session.Attach(adapter);
                await session.SubmitChanges("Save");
            }

            this.Response.StatusCode = (int)HttpStatusCode.Created;
            this.Response.StatusDescription = "Created";

            return Json(new
            {
                success = true,
                status = "OK",
                id = adapter.Id,
                link = new
                {
                    rel = "self",
                    href = "adapter/" + adapter.Id
                }
            });
        }



        [Route("{id}")]
        [HttpPut]
        public async Task<ActionResult> Save(string id)
        {
            var adapter = this.GetRequestJson<Adapter>();
            if (null == adapter)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Cannot deserialize adapter");

            adapter.Id = id;
            var vr = (await adapter.ValidateAsync()).ToArray();
            if (vr.Any())
            {
                return Json(new { success = false, status = "Not Valid", errors = vr });
            }

            var context = new SphDataContext();
            this.Response.StatusCode = (int)HttpStatusCode.Accepted;
            if (adapter.IsNewItem)
            {
                adapter.Id = adapter.Name.ToIdFormat();
                this.Response.StatusCode = (int)HttpStatusCode.Created;
            }

            using (var session = context.OpenSession())
            {
                session.Attach(adapter);
                await session.SubmitChanges("Save");
            }
            return Json(new
            {
                success = true,
                status = "OK",
                id = adapter.Id,
                link = new
                {
                    rel = "self",
                    href = "adapter/" + adapter.Id
                }
            });
        }


        [Route("designer/{extension}/{jsroute}")]
        public ActionResult GetDialog(string extension, string jsroute)
        {
            var lowered = jsroute.ToLowerInvariant();
            if (lowered == "definition.list")
            {
                if (extension == "js")
                    return Content(Properties.Resources.AdapterDefinitionListJs, "application/javascript", Encoding.UTF8);
                if (extension == "html")
                    return Content(Properties.Resources.AdapterDefinitionListHtml, "text/html", Encoding.UTF8);
            }

            var ds = ObjectBuilder.GetObject<DeveloperService>();
            if (null == ds.Adapters)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "MEF Cannot load adapters metadata");

            var routeProviders = ds.Adapters
                .Where(x => null != x.Metadata.RouteTableProvider)
                .Select(x => Activator.CreateInstance(x.Metadata.RouteTableProvider) as IRouteTableProvider)
                .Where(x => null != x)
                .Select(x => x)
                .ToList();
            var route = routeProviders
                .Select(x => x.Routes)
                .SelectMany(x => x.ToArray())
                .SingleOrDefault(r => r.ModuleId.ToLowerInvariant().Replace("viewmodels/adapter.", "") == lowered);


            if (null == route)
                return HttpNotFound("cannot find the route for " + jsroute);

            var provider = routeProviders
                .First(x => x.Routes.Any(y => y.ModuleId == route.ModuleId));

            if (extension == "js")
            {
                var js = provider.GetEditorViewModel(route);
                return Content(js, "application/javascript", Encoding.UTF8);
            }
            var html = provider.GetEditorView(route);
            return Content(html, "text/html", Encoding.UTF8);
        }


    }
}