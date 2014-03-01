using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class EntityFormController : Controller
    {
        public async Task<ActionResult> Save()
        {
            var ef = this.GetRequestJson<EntityForm>();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(ef);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", id = ef.EntityFormId });
        }
        public async Task<ActionResult> Publish()
        {
            var context = new SphDataContext();
            var form = this.GetRequestJson<EntityForm>();
            form.IsPublished = true;
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.EntityDefinitionId == form.EntityDefinitionId);

            var buildValidation =await form.ValidateBuildAsync(ed);
            if (!buildValidation.Result)
                return Json(buildValidation);

            using (var session = context.OpenSession())
            {
                session.Attach(form);
                await session.SubmitChanges("Publish");
            } 
            return Json(new { success = true, status = "OK", message = "Your form has been successfully published", id = form.EntityFormId });

        }
    }
}