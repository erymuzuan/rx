using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Controllers
{
    [Authorize]
    [RoutePrefix("partial-view")]
    public class PartialViewController : BaseController
    {
        [HttpPost]
        [Route("")]
        public async Task<ActionResult> Save()
        {
            var ef = this.GetRequestJson<PartialView>();
            var context = new SphDataContext();

            var baru = string.IsNullOrWhiteSpace(ef.Id) || ef.Id == "0";
            if (baru) ef.Id = ef.Route.ToIdFormat();

            using (var session = context.OpenSession())
            {
                session.Attach(ef);
                await session.SubmitChanges("Save");
            }
            this.Response.StatusCode = (int)(baru ? HttpStatusCode.Created : HttpStatusCode.OK);
            return Json(new { success = true, status = "OK", id = ef.Id, location = $"{ConfigurationManager.BaseUrl}/sph#form.dialog.designer/{ef.Entity}/{ef.Id}" });
        }
        [HttpGet]
        [Route("members/{entity}/{path}")]
        public async Task<ActionResult> GetMembers()
        {
            await Task.Delay(500);
            return Content("");
        }

        [HttpPost]
        [Route("depublish")]
        public async Task<ActionResult> Depublish()
        {
            var context = new SphDataContext();
            var form = this.GetRequestJson<PartialView>();

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
        [Route("publish")]
        public async Task<ActionResult> Publish()
        {
            var ds = ObjectBuilder.GetObject<IDeveloperService>();
            var context = new SphDataContext();
            var form = this.GetRequestJson<PartialView>();
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
        public async Task<ActionResult> Remove(string id)
        {
            var context = new SphDataContext();
            var form = context.LoadOneFromSources<PartialView>(e => e.Id == id);
            if (null == form)
                return new HttpNotFoundResult("Cannot find form to delete , Id : " + id);

            using (var session = context.OpenSession())
            {
                session.Delete(form);
                await session.SubmitChanges("Remove");
            }
            return Json(new { success = true, status = "OK", message = "Your form has been successfully deleted", id = form.Id });

        }
    }
}