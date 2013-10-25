using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Helpers;

namespace Bespoke.Sph.Web.Controllers
{
    public class SpaceController : Controller
    {
        public async Task<ActionResult> Save(Space space)
        {
            var buildingId = space.BuildingId;
            var context = new SphDataContext();

            var building = await context.LoadOneAsync<Building>(b => b.BuildingId == buildingId);
            if (null != building)
            {
                space.State = building.Address.State ?? space.Address.State;
                space.City = building.Address.City ?? space.Address.City;
                space.BuildingName = building.Name;

            }
            var template = await context.LoadOneAsync<SpaceTemplate>(s => s.SpaceTemplateId == space.TemplateId);
            var result = space.ValidateBusinessRule(template.BusinessRuleCollection);
            if (result.Success == false)
            {
                return Json(result);
            }
            using (var session = context.OpenSession())
            {
                session.Attach(space);
                await session.SubmitChanges();
            }
            return Json(result);
        }


        public async Task<ActionResult> SaveMap(int spaceId, string path, LatLng point)
        {
            if (spaceId == 0) throw new ArgumentException("Space is not yet saved", "spaceId");
            var spatial = ObjectBuilder.GetObject<ISpatialService<Space>>();
            var context = new SphDataContext();

            var space = await context.LoadOneAsync<Space>(b => b.SpaceId == spaceId);

            if (null != point)
            {
                space.EncodedWkt = GoogleMapHelper.EncodeLatLong(new[] { point });
                space.Wkt = point.ToWKT();
            }

            if (!string.IsNullOrWhiteSpace(path))
            {

                space.EncodedWkt = path;
                var points = path.Decode().ToList();
                if (
                    Math.Abs(points.First().Lat - points.Last().Lat) > 0.00001
                    || Math.Abs(points.First().Lng - points.Last().Lng) > 0.00001
                    )
                    points.Add(points.First().Clone());

                space.Wkt = points.ToWkt();
            }

            await spatial.UpdateAsync(space);
            return Json(true);
        }

        public async Task<ActionResult> GetCenter(int id)
        {
            if (id == 0) return Json(false, JsonRequestBehavior.AllowGet);

            var spatial = ObjectBuilder.GetObject<ISpatialService<Space>>();
            var context = new SphDataContext();
            var center = await spatial.GetCenterAsync(b => b.SpaceId == id);
            var space = await context.LoadOneAsync<Space>(s => s.SpaceId == id);

            if (null == center)
            {
                // try to get from building
                var buildingSpatial = ObjectBuilder.GetObject<ISpatialService<Building>>();
                center = await buildingSpatial.GetCenterAsync(b => b.BuildingId == space.BuildingId);
                if (null == center)
                    return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(center, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetEncodedPath(int id)
        {
            var spatial = ObjectBuilder.GetObject<ISpatialService<Space>>();
            var encodedPath = await spatial.GetEncodedPathAsync(b => b.SpaceId == id);
            if (null == encodedPath) return Json(false, JsonRequestBehavior.AllowGet);
            return Json(encodedPath, JsonRequestBehavior.AllowGet);
        }

    }
}
