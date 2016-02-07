using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize(Roles = "developers, administrators")]
    [RoutePrefix("adapter")]
    public class AdapterController : BaseApiController
    {

        [Route("installed-adapters")]
        public IHttpActionResult InstalledAdapters()
        {
            var actions = from a in this.DeveloperService.Adapters
                          select
                              $@"
{{
    ""designer"" : {JsonConvert.SerializeObject(a.Metadata)},
    ""adapter"" : {a.Value.ToJsonString()}
}}";


            return Json("[" + string.Join(",", actions) + "]");

        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync(string id)
        {
            var context = new SphDataContext();
            var ef = await context.LoadOneAsync<Adapter>(x => x.Id == id);
            if (null == ef)
                return NotFound("Cannot find adapter with id " + id);

            using (var session = context.OpenSession())
            {
                session.Delete(ef);
                await session.SubmitChanges("Delete");
            }
            return Ok(new { success = true, status = "OK", id = ef.Id });
        }
        [Route("")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync([JsonBody]Adapter adapter)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Delete(adapter);
                await session.SubmitChanges("Delete");
            }
            return Ok(new { success = true, status = "OK", id = adapter.Id });
        }

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Create([JsonBody]Adapter adapter)
        {
            if (null == adapter)
                return BadRequest("Cannot deserialize adapter");

            var context = new SphDataContext();
            adapter.Id = adapter.Name.ToIdFormat();

            using (var session = context.OpenSession())
            {
                session.Attach(adapter);
                await session.SubmitChanges("Save");
            }


            var result = new
            {
                success = true,
                status = "OK",
                id = adapter.Id,
                link = new
                {
                    rel = "self",
                    href = "adapter/" + adapter.Id
                }
            };
            return Created($"/api/adapters/{adapter.Id}", result);
        }



        [Route("{id}")]
        [HttpPut]
        public async Task<IHttpActionResult> Save([JsonBody]Adapter adapter, string id)
        {
            if (null == adapter)
                return NotFound("Cannot deserialize adapter");

            adapter.Id = id;
            var vr = (await adapter.ValidateAsync()).ToArray();
            if (vr.Any())
            {
                return Json(new { success = false, status = "Not Valid", errors = vr });
            }

            var context = new SphDataContext();
            var baru = adapter.IsNewItem;
            if (baru)
            {
                adapter.Id = adapter.Name.ToIdFormat();
            }

            using (var session = context.OpenSession())
            {
                session.Attach(adapter);
                await session.SubmitChanges("Save");
            }
            var result = new
            {
                success = true,
                status = "OK",
                id = adapter.Id,
                link = new
                {
                    rel = "self",
                    href = "adapter/" + adapter.Id
                }
            };
            if (baru) return Created($"/api/adapters/{adapter.Id}", result);
            return Ok(result);
        }


        [Route("designer/{extension}/{jsroute}")]
        public IHttpActionResult GetDialog(string extension, string jsroute)
        {
            var lowered = jsroute.ToLowerInvariant();
            if (lowered == "definition.list")
            {
                switch (extension)
                {
                    case "js":
                        return Javascript(Properties.Resources.AdapterDefinitionListJs);
                    case "html":
                        return Html(Properties.Resources.AdapterDefinitionListHtml);
                }
            }

            if (null == this.DeveloperService.Adapters)
                return InternalServerError(new Exception("MEF Cannot load adapters metadata"));

            var routeProviders = this.DeveloperService.Adapters
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
                return NotFound("cannot find the route for " + jsroute);

            var provider = routeProviders
                .First(x => x.Routes.Any(y => y.ModuleId == route.ModuleId));

            if (extension == "js")
            {
                var js = provider.GetEditorViewModel(route);
                return Javascript(js);
            }
            var html = provider.GetEditorView(route);
            return Html(html);
        }


    }
}