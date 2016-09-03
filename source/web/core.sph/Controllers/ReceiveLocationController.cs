using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.WebApi;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize(Roles = "developers, administrators")]
    [RoutePrefix("receive-locations")]
    public class ReceiveLocationController : BaseApiController
    {

        [Route("installed")]
        [HttpGet]
        public IHttpActionResult GetInstalledReceiveLocations()
        {
            var actions = from a in this.DeveloperService.ReceiveLocationOptions
                select
                $@"
{{
    ""designer"" : {JsonConvert.SerializeObject(a.Metadata)},
    ""location"" : {a.Value.ToJsonString()}
}}";
           

            return Json("[" + string.Join(",", actions) + "]");

        }


        [HttpPost]
        [PostRoute("{id}/publish")]
        public async Task<IHttpActionResult> CompileAsync([JsonBody]ReceiveLocation loc, string id)
        {
            var portIsNewItem = loc.IsNewItem;
            if (portIsNewItem)
                loc.Id = loc.Name.ToIdFormat();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(loc);
                await session.SubmitChanges();
            }
            var port = context.LoadOneFromSources<ReceivePort>(x => x.Id == loc.ReceivePort);
            var cr = await loc.CompileAsync(port);
            if (!cr.Result)
                return Invalid(HttpStatusCode.BadRequest, cr);

            if (portIsNewItem)
                return Created($"/receive-locations/{loc.Id}/publish", new { success = true });
            return Ok(new { success = true });
        }

        [Route("{id}")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync(string id)
        {
            var context = new SphDataContext();
            var ef = await context.LoadOneAsync<ReceiveLocation>(x => x.Id == id);
            if (null == ef)
                return NotFound("Cannot find receive location with id " + id);

            using (var session = context.OpenSession())
            {
                session.Delete(ef);
                await session.SubmitChanges("Delete");
            }
            return Ok(new { success = true, status = "OK", id = ef.Id });
        }
        [Route("")]
        [HttpDelete]
        public async Task<IHttpActionResult> DeleteAsync([JsonBody]ReceiveLocation  location)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Delete( location);
                await session.SubmitChanges("Delete");
            }
            return Ok(new { success = true, status = "OK", id =  location.Id });}

        [Route("")]
        [HttpPost]
        public async Task<IHttpActionResult> Create([JsonBody]ReceiveLocation  location)
        {
            if (null ==  location)
                return BadRequest("Cannot deserialize receive location");

            var context = new SphDataContext();
             location.Id =  location.Name.ToIdFormat();

            using (var session = context.OpenSession())
            {
                session.Attach( location);
                await session.SubmitChanges("Save");
            }


            var result = new
            {
                success = true,
                status = "OK",
                id =  location.Id,
                link = new
                {
                    rel = "self",
                    href = "receive location/" +  location.Id
                }
            };
            return Created($"/api/receive locations/{ location.Id}", result);
        }



        [PostRoute("")]
        [HttpPost]
        public async Task<IHttpActionResult> Save([JsonBody]ReceiveLocation  location, string id)
        {
            if (null ==  location)
                return NotFound("Cannot deserialize receive location");

             location.Id = id;
            var vr = (await  location.ValidateAsync()).ToArray();
            if (vr.Any())
            {
                return Json(new { success = false, status = "Not Valid", errors = vr });
            }

            var context = new SphDataContext();
            var baru =  location.IsNewItem;
            if (baru)
            {
                 location.Id =  location.Name.ToIdFormat();
            }

            using (var session = context.OpenSession())
            {
                session.Attach( location);
                await session.SubmitChanges("Save");
            }
            var result = new
            {
                success = true,
                status = "OK",
                id =  location.Id,
                link = new
                {
                    rel = "self",
                    href = "receive location/" +  location.Id
                }
            };
            if (baru) return Created($"/api/receive locations/{ location.Id}", result);
            return Ok(result);
        }


        [Route("designer/{jsroute}/{extension}")]
        [HttpGet]
        public IHttpActionResult GetDialog(string extension, string jsroute)
        {
            var lowered = jsroute.ToLowerInvariant();
            
            if (null == this.DeveloperService.ReceiveLocationOptions)
                return InternalServerError(new Exception("MEF Cannot load receive locations metadata"));

            var routeProviders = this.DeveloperService.ReceiveLocationOptions
                .Where(x => null != x.Metadata.RouteTableProvider)
                .Select(x => Activator.CreateInstance(x.Metadata.RouteTableProvider) as IRouteTableProvider)
                .Where(x => null != x)
                .Select(x => x)
                .ToList();
            var route = routeProviders
                .Select(x => x.Routes)
                .SelectMany(x => x.ToArray())
                .SingleOrDefault(r => r.ModuleId.ToLowerInvariant().Replace("viewmodels/receive location.", "") == lowered);


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