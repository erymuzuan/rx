using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Dependencies;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("query-endpoints")]
    public class QueryEndpointController : ApiController
    {
        private readonly IDeveloperService m_developerService;
        public QueryEndpointController()
        {
            DeveloperService.Init();
            m_developerService = ObjectBuilder.GetObject<DeveloperService>();
        }

        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> Save([JsonBody]QueryEndpoint endpoint)
        {
            var context = new SphDataContext();
            var baru = string.IsNullOrWhiteSpace(endpoint.Id) || endpoint.Id == "0";
            if (baru) endpoint.Id = endpoint.Name.ToIdFormat();

            using (var session = context.OpenSession())
            {
                session.Attach(endpoint);
                await session.SubmitChanges("Save");
            }
            var code = baru ? HttpStatusCode.Created : HttpStatusCode.OK;
            var location = $"{ConfigurationManager.BaseUrl}/api/entityquery/{endpoint.Id}";
            var response = new JsonResponseMessage(code, new
            {
                success = true,
                status = "OK",
                id = endpoint.Id,
                _link = new
                {
                    rel = "self",
                    href = location
                }
            });
            response.Headers.Location = new Uri(location);
            return response;
        }




        [HttpPost]
        [Route("{id}/depublish")]
        public async Task<HttpResponseMessage> Depublish([RequestBody]QueryEndpoint endpoint, string id)
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

        [HttpPost]
        [Route("{id}/publish")]
        public async Task<HttpResponseMessage> Publish(string id, [JsonBody]QueryEndpoint endpoint)
        {
            var context = new SphDataContext();
            endpoint.IsPublished = true;
            endpoint.BuildDiagnostics = this.m_developerService.BuildDiagnostics;

            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Id == endpoint.Entity || e.Name == endpoint.Entity);

            var buildValidation = await endpoint.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return new HttpResponseMessage((HttpStatusCode)422) { Content = new JsonContent(buildValidation.ToJsonString(true)) };

            var options = new CompilerOptions();
            var sources = endpoint.GenerateCode(ed);
            var result = endpoint.Compile(options, sources);
            if (!result.Result)
            {
                return new JsonResponseMessage(HttpStatusCode.Conflict, result);
            }

            using (var session = context.OpenSession())
            {
                session.Attach(endpoint);
                await session.SubmitChanges("Publish");
            }
            return new JsonResponseMessage(new
            {
                success = true,
                status = "OK",
                message = "Your query endpoint has been successfully published",
                id = endpoint.Id,
                warnings = buildValidation.Warnings,
                _links = new
                {
                    rel = "self",
                    href = $"{ConfigurationManager.BaseUrl}/api/queryendpoint/{endpoint.Id}"
                }
            });

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<HttpResponseMessage> Remove(string id)
        {
            var context = new SphDataContext();
            var form = context.LoadOneFromSources<QueryEndpoint>(e => e.Id == id);
            if (null == form)
                return new HttpResponseMessage(HttpStatusCode.NotFound) { Content = new JsonContent("Cannot find form to delete , Id : " + id) };

            using (var session = context.OpenSession())
            {
                session.Delete(form);
                await session.SubmitChanges("Remove");
            }
            return new JsonResponseMessage(new { success = true, status = "OK", message = "Your form has been successfully deleted", id = form.Id });

        }
    }
}