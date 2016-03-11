using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize]
    [RoutePrefix("api/form-dialogs")]
    public class FormDialogController : BaseApiController
    {
        [HttpPost]
        [Route("")]
        public async Task<IHttpActionResult> Save([JsonBody]FormDialog dialog)
        {
            var context = new SphDataContext();

            var baru = string.IsNullOrWhiteSpace(dialog.Id) || dialog.Id == "0";
            if (baru) dialog.Id = dialog.Route.ToIdFormat();

            using (var session = context.OpenSession())
            {
                session.Attach(dialog);
                await session.SubmitChanges("Save");
            }
            var response = (new { success = true, status = "OK", id = dialog.Id, location = $"{ConfigurationManager.BaseUrl}/sph#form.dialog.designer/{dialog.Entity}/{dialog.Id}" });

            if (baru) return Created($"{ConfigurationManager.BaseUrl}/api/form-dialogs/{dialog.Id}", response);
            return Ok(response);
        }
        [HttpGet]
        [Route("members/{entity}/{path}")]
        public async Task<IHttpActionResult> GetMembers()
        {
            await Task.Delay(500);
            return Ok("");
        }

        [HttpPost]
        [Route("{id}/depublish")]
        public async Task<IHttpActionResult> Depublish(string id, [JsonBody]FormDialog form)
        {
            var context = new SphDataContext();

            // look for column which points to the form
            var views = context.LoadFromSources<EntityView>(e => e.IsPublished && e.EntityDefinitionId == form.Entity);


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
        [Route("{id}/publish")]
        public async Task<IHttpActionResult> Publish(string id, [JsonBody]FormDialog form)
        {
            var ds = ObjectBuilder.GetObject<IDeveloperService>();
            var context = new SphDataContext();
            form.IsPublished = true;
            form.BuildDiagnostics = ds.BuildDiagnostics;

            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.Id == form.Entity);

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
            var form = context.LoadOneFromSources<FormDialog>(e => e.Id == id);
            if (null == form)
                return NotFound("Cannot find form to delete , Id : " + id);

            using (var session = context.OpenSession())
            {
                session.Delete(form);
                await session.SubmitChanges("Remove");
            }
            return Json(new { success = true, status = "OK", message = "Your form has been successfully deleted", id = form.Id });

        }
    }
}