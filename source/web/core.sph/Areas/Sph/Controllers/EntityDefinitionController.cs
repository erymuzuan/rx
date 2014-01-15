using System.Globalization;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class EntityDefinitionController : Controller
    {

        public async Task<ActionResult> Save()
        {
            var ed = this.GetRequestJson<EntityDefinition>();
            var context = new SphDataContext();

            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", message = "Your entity has been successfully saved ", id = ed.EntityDefinitionId });


        }
        public async Task<ActionResult> Publish()
        {
            var ed = this.GetRequestJson<EntityDefinition>();
            var buildValidation = ed.ValidateBuild();

            if (!buildValidation.Result)
                return Json(buildValidation);

            var context = new SphDataContext();

            using (var session = context.OpenSession())
            {
                session.Attach(ed);
                await session.SubmitChanges("Publish");
            }
            return Json(new { success = true, status = "OK", message = "Your entity has been successfully published", id = ed.EntityDefinitionId });


        }
    }
}