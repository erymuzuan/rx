using System;
using System.ComponentModel.Composition;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Web.Helpers;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize(Roles = "developers, administrators")]
    [RoutePrefix("adapter")]
    public class AdapterController : Controller
    {
        [ImportMany("AdapterDesigner", typeof(Adapter), AllowRecomposition = true)]
        public Lazy<Adapter, IDesignerMetadata>[] Adapters { get; set; }

        [Route("installed-adapters")]
        public ActionResult InstalledAdapters()
        {
            ObjectBuilder.ComposeMefCatalog(this);
            var actions = from a in this.Adapters
                          select string.Format(@"
{{
    ""designer"" : {0},
    ""adapter"" : {1}
}}", JsonConvert.SerializeObject(a.Metadata), a.Value.ToJsonString());


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
        public async Task<ActionResult> Save()
        {
            var ef = this.GetRequestJson<Adapter>();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(ef);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", id = ef.Id });
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

            if (null == this.Adapters)
                ObjectBuilder.ComposeMefCatalog(this);

            if (null == this.Adapters)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "MEF Cannot load adapters metadata");

            var routeProviders = this.Adapters
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