using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;

namespace Bespoke.Sph.Web.Controllers
{
    [RoutePrefix("api/workflow-forms")]
    public class WorkflowFormController : BaseApiController
    {
        [HttpPost]
        [HttpPut]
        [Route("")]
        public async Task<IHttpActionResult> Save([JsonBody]WorkflowForm form)
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
        [Route("publish")]
        public async Task<IHttpActionResult> Publish([JsonBody]WorkflowForm form)
        {
            var ds = ObjectBuilder.GetObject<DeveloperService>();
            var context = new SphDataContext();
            form.BuildDiagnostics = ds.BuildDiagnostics;

            var ed = await context.LoadOneAsync<WorkflowDefinition>(e => e.Id == form.WorkflowDefinitionId);

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
            var form = context.LoadOneFromSources<WorkflowForm>(e => e.Id == id);
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