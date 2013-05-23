using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.Helpers;
using Bespoke.SphCommercialSpaces.Domain;
using System.Linq;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class MapController : Controller
    {
        public async Task<ActionResult> Get(string id, string[] filter)
        {
            var bounds = GoogleMapHelper.ParseBound(id).ToArray();

            var spatial = ObjectBuilder.GetObject<ISpatialService<Building>>();
            var buildings = await spatial.ContainsAsync(b => b.Status == "Active", bounds);
            var list = from b in buildings
                       select new
                           {
                               b.Name,
                               b.BuildingId,
                               b.Address,
                               b.EncodedWkt
                           };
            return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
        }

    }
}
