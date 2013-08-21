using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.Helpers;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class LandController : Controller
    {
        public async Task<ActionResult> Save()
        {
            var land = this.GetRequestJson<Land>();
            var context = new SphDataContext();

            using (var session = context.OpenSession())
            {
                session.Attach(land);
                await session.SubmitChanges("Save");
            }

            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(land.LandId.ToString(CultureInfo.InvariantCulture));

        }


        public async Task<ActionResult> SaveMap(int landId, string path)
        {
            var spatial = ObjectBuilder.GetObject<ISpatialService<Land>>();
            var context = new SphDataContext();

            var item = await context.LoadOneAsync<Land>(b => b.LandId == landId);
            item.EncodedWkt = path;
            var points = path.Decode().ToList();
            if (
                Math.Abs(points.First().Lat - points.Last().Lat) > 0.00001
                || Math.Abs(points.First().Lng - points.Last().Lng) > 0.00001
                )
                points.Add(points.First().Clone());

            item.Wkt = points.ToWkt();

            await spatial.UpdateAsync(item);
            return Json(true);
        }


        public async Task<ActionResult> GetCenter(int id)
        {
            var spatial = ObjectBuilder.GetObject<ISpatialService<Land>>();
            var center = await spatial.GetCenterAsync(b => b.LandId == id);
            if (null == center) return Json(false, JsonRequestBehavior.AllowGet);
            return Json(center, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetEncodedPath(int id)
        {
            var spatial = ObjectBuilder.GetObject<ISpatialService<Land>>();
            var encodedPath = await spatial.GetEncodedPathAsync(b => b.LandId == id);
            if (null == encodedPath) return Json(false, JsonRequestBehavior.AllowGet);
            return Json(encodedPath, JsonRequestBehavior.AllowGet);
        }


    }
}
