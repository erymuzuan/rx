using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize]
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

        [HttpGet]
        [Route("{id}/activities/{webid:guid}/schema")]
        public async Task<IHttpActionResult> GetFormSchema(string id, string webid)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<WorkflowForm>(x => x.Id == id);
            if (null == form) return NotFound();

            var wd = await context.LoadOneAsync<WorkflowDefinition>(x => x.Id == form.WorkflowDefinitionId);
            var act = wd?.GetActivity<ReceiveActivity>(webid);
            if (null == act) return NotFound();

            var @var = wd.VariableDefinitionCollection.SingleOrDefault(x => x.Name == act.MessagePath) as ValueObjectVariable;
            if (null == @var)
                return Invalid((HttpStatusCode)409, new { message = $"The [{act.Name}] has not specify a correct MessagePath[{act.MessagePath}]" });
            var schema = await @var.GenerateCustomJavascriptAsync(wd);
            return Json(schema);
        }

        [HttpGet]
        [Route("{id}/activities/{webid:guid}/vod")]
        public async Task<IHttpActionResult> GetFormVod(string id, string webid)
        {
            var context = new SphDataContext();
            var form = await context.LoadOneAsync<WorkflowForm>(x => x.Id == id);
            if (null == form) return NotFound();

            var wd = await context.LoadOneAsync<WorkflowDefinition>(x => x.Id == form.WorkflowDefinitionId);
            var act = wd?.GetActivity<ReceiveActivity>(webid);
            if (null == act) return NotFound();

            var @var = wd.VariableDefinitionCollection.SingleOrDefault(x => x.Name == act.MessagePath) as ValueObjectVariable;
            if (null == @var)
                return Invalid((HttpStatusCode)409, new { message = $"The [{act.Name}] has not specify a correct MessagePath[{act.MessagePath}]" });
            var vod = await context.LoadOneAsync<ValueObjectDefinition>(x => x.Name == @var.TypeName);
            return Json(vod.ToJsonString(true));
        }
    }
}