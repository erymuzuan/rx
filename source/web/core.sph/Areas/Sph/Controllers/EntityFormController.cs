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
            var ef = this.GetRequestJson<EntityForm>();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(ef);
                await session.SubmitChanges("Publish");
            }
            return Json(new { success = true, status = "OK", id = ef.EntityFormId });
        }
    }
}