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
    [RoutePrefix("entity-query")]
    public class EntityQueryController : ApiController
    {
        private readonly IDeveloperService m_developerService;
        public EntityQueryController()
        {
            DeveloperService.Init();
            m_developerService = ObjectBuilder.GetObject<DeveloperService>();
        }

        [HttpPost]
        [Route("")]
        public async Task<HttpResponseMessage> Save([JsonBody]EntityQuery query)
        {
            var context = new SphDataContext();
            var baru = string.IsNullOrWhiteSpace(query.Id) || query.Id == "0";
            if (baru) query.Id = query.Name.ToIdFormat();

            using (var session = context.OpenSession())
            {
                session.Attach(query);
                await session.SubmitChanges("Save");
            }
            var code = baru ? HttpStatusCode.Created : HttpStatusCode.OK;
            var location = $"{ConfigurationManager.BaseUrl}/api/entityquery/{query.Id}";
            var response = new JsonResponseMessage(code, new
            {
                success = true,
                status = "OK",
                id = query.Id,
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
        public async Task<HttpResponseMessage> Depublish([RequestBody]EntityQuery query, string id)
        {
            var context = new SphDataContext();

            query.IsPublished = false;
            using (var session = context.OpenSession())
            {
                session.Attach(query);
                await session.SubmitChanges("Depublish");
            }
            return new JsonResponseMessage(new { success = true, status = "OK", message = "Your form has been successfully depublished", id = query.Id });


        }

        [HttpPost]
        [Route("publish")]
        public async Task<HttpResponseMessage> Publish([RequestBody]EntityQuery query)
        {
            var context = new SphDataContext();
            query.IsPublished = true;
            query.BuildDiagnostics = this.m_developerService.BuildDiagnostics;

            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Id == query.Entity);

            var buildValidation = await query.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return new HttpResponseMessage((HttpStatusCode)422) { Content = new JsonContent(buildValidation.ToJsonString(true)) };

            using (var session = context.OpenSession())
            {
                session.Attach(query);
                await session.SubmitChanges("Publish");
            }
            return new JsonResponseMessage(new
            {
                success = true,
                status = "OK",
                message = "Your form has been successfully published",
                id = query.Id,
                warnings = buildValidation.Warnings,
                _links = new
                {
                    rel = "self",
                    href = $"{ConfigurationManager.BaseUrl}/api/entityquery/{query.Id}"
                }
            });

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<HttpResponseMessage> Remove(string id)
        {
            var context = new SphDataContext();
            var form = context.LoadOneFromSources<EntityQuery>(e => e.Id == id);
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