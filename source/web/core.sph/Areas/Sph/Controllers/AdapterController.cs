using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Domain.Api;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Areas.Sph.Controllers
{
    public class AdapterController : Controller
    {
        public async Task<ActionResult> Save()
        {
            var ef = this.GetRequestJson<Adapter>();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(ef);
                await session.SubmitChanges("Save");
            }
            return Json(new { success = true, status = "OK", id = ef.AdapterId });
        }

    }
}