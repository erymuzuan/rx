using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class BuildingTemplateController : Controller
    {
        public async Task<ActionResult> Save(BuildingTemplate buildingTemplate)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(buildingTemplate);
                await session.SubmitChanges();
            }
            return Json(true);
        }

    }
}
