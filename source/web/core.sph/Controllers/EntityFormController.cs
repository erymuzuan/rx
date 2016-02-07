using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Dependencies;
using Bespoke.Sph.WebApi;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("api-rx/entity-forms")]
    public class EntityFormController : BaseApiController
    {
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Save([JsonBody]EntityForm form)
        {
            var context = new SphDataContext();
            var baru = string.IsNullOrWhiteSpace(form.Id) || form.Id == "0";
            if (baru) form.Id = form.Route.ToIdFormat();

            using (var session = context.OpenSession())
            {
                session.Attach(form);
                await session.SubmitChanges("Save");
            }
            var result = new
            {
                success = true,
                status = "OK",
                id = form.Id
            };
            if (baru) return Created("/api/entity-forms/" + form.Id, result);
            return Ok(result);
        }

        [HttpPost]
        [Route("depublish")]
        public async Task<IHttpActionResult> Depublish([JsonBody]EntityForm form)
        {
            var context = new SphDataContext();

            // look for column which points to the form
            var views = context.LoadFromSources<EntityView>(e => e.IsPublished && e.EntityDefinitionId == form.EntityDefinitionId);


            var violations = (from vw in views
                              where vw.ViewColumnCollection.Any(c => c.IsLinkColumn
                                  && c.FormRoute == form.Route)
                              select vw.Name).ToArray();
            if (violations.Any())
                return Json(new { success = false, status = "NO", message = "These views has a link to your form ", views = violations, id = form.Id });


            form.IsPublished = false;
            using (var session = context.OpenSession())
            {
                session.Attach(form);
                await session.SubmitChanges("Depublish");
            }
            return Json(new { success = true, status = "OK", message = "Your form has been successfully depublished", id = form.Id });


        }

        [HttpPost]
        [Route("publish")]
        public async Task<IHttpActionResult> Publish([JsonBody]EntityForm form)
        {
            var ds = ObjectBuilder.GetObject<DeveloperService>();
            var context = new SphDataContext();
            form.IsPublished = true;
            form.BuildDiagnostics = ds.BuildDiagnostics;

            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Id == form.EntityDefinitionId);

            var buildValidation = await form.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return Json(buildValidation);

            using (var session = context.OpenSession())
            {
                session.Attach(form);
                await session.SubmitChanges("Publish");
            }
            return Json(new { success = true, status = "OK", message = "Your form has been successfully published", id = form.Id, warnings = buildValidation.Warnings });

        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IHttpActionResult> Remove(string id)
        {
            var context = new SphDataContext();
            var form = context.LoadOneFromSources<EntityForm>(e => e.Id == id);
            if (null == form)
                return NotFound();

            using (var session = context.OpenSession())
            {
                session.Delete(form);
                await session.SubmitChanges("Remove");
            }
            return Json(new { success = true, status = "OK", message = "Your form has been successfully deleted", id = form.Id });

        }
    }
}