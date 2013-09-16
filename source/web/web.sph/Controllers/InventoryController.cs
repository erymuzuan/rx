using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.Controllers
{
    public class InventoryController : Controller
    {
        public async Task<ActionResult> Save(Inventory inventory)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(inventory);
                await session.SubmitChanges("Add/Update Inventory");
            }
            return Json(true);
        }

    }
}
