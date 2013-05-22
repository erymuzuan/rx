using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Commerspace.Web.Helpers;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class BuildingController : Controller
    {

        public async Task<ActionResult> GetCenter(int id)
        {
            var spatial = ObjectBuilder.GetObject<ISpatialService<Building>>();
            var center = await spatial.GetCenterAsync(b => b.BuildingId == id);
            return Json(center, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetEncodedPath(int id)
        {
            var spatial = ObjectBuilder.GetObject<ISpatialService<Building>>();
            var encodedPath = await spatial.GetEncodedPathAsync(b => b.BuildingId == id);
            return Json(encodedPath, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SaveMap(int buildingId, string path)
        {
            var spatial = ObjectBuilder.GetObject<ISpatialService<Building>>();
            var context = new SphDataContext();

            var item = await context.LoadOneAsync<Building>(b => b.BuildingId == buildingId);
            item.EncodedWkt = path;
            item.Wkt = path.Decode().ToWkt();

            await spatial.UpdateAsync(item);
            return Json(true);
        }


        public async Task<ActionResult> GetFloorPlan(int id, string floor, string lot)
        {
            var context = new SphDataContext();

            var item = await context.LoadOneAsync<Building>(b => b.BuildingId == id);
            var lotItem = item.FloorCollection.Single(f => f.Name == floor).LotCollection.Single(l => l.Name == lot);
            if (string.IsNullOrWhiteSpace(lotItem.PlanStoreId)) return Json(null, JsonRequestBehavior.AllowGet);
            var line = await context.GetScalarAsync<SpatialStore, string>(s => s.StoreId == lotItem.PlanStoreId, s => s.EncodedWkt);

            var shape = new
                {
                    Encoded = line,
                    lotItem.FillColor,
                    lotItem.FillOpacity
                };

            return Json(shape, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AddLotFloorPlan(int id, string floor, string lot, string fillColor, double fillOpacity, string path)
        {
            var context = new SphDataContext();
            var building = await context.LoadOneAsync<Building>(b => b.BuildingId == id);
            var lotItem = building.FloorCollection.Single(f => f.Name == floor).LotCollection.Single(l => l.Name == lot);
            lotItem.FillOpacity = fillOpacity;
            lotItem.FillColor = fillColor;
            lotItem.PlanStoreId = Guid.NewGuid().ToString();

            var store = new SpatialStore
                {
                    StoreId = lotItem.PlanStoreId,
                    Type = typeof(Lot).Name,
                    Tag = string.Format("{0};{1}{2}", id, floor, lot),
                    EncodedWkt = path,
                    Wkt = path.Decode().ToWkt()
                };

            using (var session = context.OpenSession())
            {
                session.Attach(store);
                await session.SubmitChanges();
            }

            var spatial = ObjectBuilder.GetObject<ISpatialService<SpatialStore>>();
            await spatial.UpdateAsync(store);

            return Json(true);
        }

        public async Task<ActionResult> SaveBuilding(Building building)
        {
            var context = new SphDataContext();
            using (var session = context.OpenSession())
            {
                session.Attach(building);
                await session.SubmitChanges();
            }
            return Json(true);
        }

        public async Task<ActionResult> AddLot(Floor floor, int buildingId, string floorname)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<Building>(b => b.BuildingId == buildingId);
            var dbfloor = dbItem.FloorCollection.Single(f => f.Name == floorname);

            dbItem.BuildingId = buildingId;
            dbItem.FloorCollection.Replace(dbfloor, floor);

            using (var session = context.OpenSession())
            {
                session.Attach(dbItem);
                await session.SubmitChanges();
            }
            return Json(true);
        }
    }
}
