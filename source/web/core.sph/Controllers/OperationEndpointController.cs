using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Compilers;
using Bespoke.Sph.Domain.Extensions;
using Bespoke.Sph.Web.Helpers;
using Bespoke.Sph.WebApi;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("api/operation-endpoints")]
    public class OperationEndpointController : BaseApiController
    {
       
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Save([JsonBody]OperationEndpoint endpoint)
        {
            var context = new SphDataContext();
            var repos = ObjectBuilder.GetObject<ISourceRepository>();

            var ed = await repos.LoadOneAsync<EntityDefinition>(x => x.Name == endpoint.Entity);
            if (null == ed)
                return NotFound($"Cannot find Entity {endpoint.Entity}");

            var baru = string.IsNullOrWhiteSpace(endpoint.Id) || endpoint.Id == "0";
            if (baru) { endpoint.Id = $"{endpoint.Entity} {endpoint.Name}".ToIdFormat();}
            if (string.IsNullOrWhiteSpace(endpoint.Resource))
                endpoint.Resource = ed.Plural.ToIdFormat();


            using (var session = context.OpenSession())
            {
                session.Attach(endpoint);
                await session.SubmitChanges("Save");
            }
            var location = $"{ConfigurationManager.BaseUrl}/api/operation-endpoints/{endpoint.Id}";
            var response = new
            {
                success = true,
                status = "OK",
                id = endpoint.Id,
                _links = new[]  {
                    new Link("self", location, "GET") ,
                    new Link("publish",$"{ConfigurationManager.BaseUrl}/api/operation-endpoints/{endpoint.Id}/publish", "PUT"),
                    new Link("depublish",$"{ConfigurationManager.BaseUrl}/api/operation-endpoints/{endpoint.Id}/depublish","PUT"),
                    new Link( "delete", $"{ConfigurationManager.BaseUrl}/api/operation-endpoints/{endpoint.Id}","DELETE")}
            };
            if (baru)
                return Created(new Uri(location), response);
            return Ok(response);
        }


        [HttpPut]
        [Route("{id}/depublish")]
        public async Task<HttpResponseMessage> Depublish([RequestBody]OperationEndpoint endpoint, string id)
        {
            var context = new SphDataContext();

            endpoint.IsPublished = false;
            using (var session = context.OpenSession())
            {
                session.Attach(endpoint);
                await session.SubmitChanges("Depublish");
            }
            return new JsonResponseMessage(new { success = true, status = "OK", message = "Your form has been successfully depublished", id = endpoint.Id });


        }

        [HttpPut]
        [Route("{id}/publish")]
        public async Task<IHttpActionResult> Publish(string id, [JsonBody]OperationEndpoint endpoint)
        {
            var context = new SphDataContext();
            var repos = ObjectBuilder.GetObject<ISourceRepository>();

            endpoint.IsPublished = true;
            endpoint.BuildDiagnostics = this.DeveloperService.BuildDiagnostics;

            var ed = await repos.LoadOneAsync<EntityDefinition>(e => e.Id == endpoint.Entity || e.Name == endpoint.Entity);

            var buildValidation = await endpoint.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return ResponseMessage(new HttpResponseMessage() {StatusCode = (HttpStatusCode)422,});

            var result = await endpoint.CompileAsync();
            if (!result.Result)
                return Invalid(result);
            

            using (var session = context.OpenSession())
            {
                session.Attach(endpoint);
                await session.SubmitChanges("Publish");
            }
            var content = new
            {
                success = true,
                status = "OK",
                message = "Your operation endpoint has been successfully published",
                id = endpoint.Id,
                warnings = buildValidation.Warnings,
                _links = new
                {
                    rel = "self",
                    href = $"{ConfigurationManager.BaseUrl}/api/operation-endpoints/{endpoint.Id}"
                }
            };
            return Ok(content);

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<HttpResponseMessage> Remove(string id)
        {
            var context = new SphDataContext();
            var repos = ObjectBuilder.GetObject<ISourceRepository>();
            var form = await repos.LoadOneAsync<OperationEndpoint>(e => e.Id == id);
            if (null == form)
                return new HttpResponseMessage(HttpStatusCode.NotFound) { Content = new JsonContent("Cannot find endpoint to delete , Id : " + id) };

            using (var session = context.OpenSession())
            {
                session.Delete(form);
                await session.SubmitChanges("Remove");
            }
            return new JsonResponseMessage(new { success = true, status = "OK", message = "Your endpoint has been successfully deleted", id = form.Id });

        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IHttpActionResult> Get(string id)
        {
            var repos = ObjectBuilder.GetObject<ISourceRepository>();
            var endpoint = await repos.LoadOneAsync<OperationEndpoint>(e => e.Id == id);
            if (null == endpoint)
                return NotFound();

            return Ok(endpoint);

        }
    }
}