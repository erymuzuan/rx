using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class EntityViewController : Controller
    {
        public async Task<ActionResult> Save()
        {
            var ef = this.GetRequestJson<EntityView>();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(ef);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", id = ef.EntityViewId });
        }
        public async Task<ActionResult> Publish()
        {
            var view = this.GetRequestJson<EntityView>();
            var context = new SphDataContext();
            var ed = await context.LoadOneAsync<EntityDefinition>(e => e.EntityDefinitionId == view.EntityDefinitionId);

            var buildValidation = await view.ValidateBuild(ed);
            if (!buildValidation.Result)
                return Json(buildValidation);

            view.IsPublished = true;
            using (var session = context.OpenSession())
            {
                session.Attach(view);
                await session.SubmitChanges("Publish");
            }
            return Json(new { success = true, status = "OK", id = view.EntityViewId });
        }
    }
}