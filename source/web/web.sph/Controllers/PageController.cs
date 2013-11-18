using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Controllers
{
    public class PageController : Controller
    {
        public async Task<ActionResult> Save()
        {
            var page = this.GetRequestJson<Page>();
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(page);
                await session.SubmitChanges();
            }
            return Json(new {status = "OK", success = true});
        }
	}
}