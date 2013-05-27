﻿using System;
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
            if (null == center) return Json(false, JsonRequestBehavior.AllowGet);
            return Json(center, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> GetEncodedPath(int id)
        {
            var spatial = ObjectBuilder.GetObject<ISpatialService<Building>>();
            var encodedPath = await spatial.GetEncodedPathAsync(b => b.BuildingId == id);
            if (null == encodedPath) return Json(false, JsonRequestBehavior.AllowGet);
            return Json(encodedPath, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SaveMap(int buildingId, string path)
        {
            var spatial = ObjectBuilder.GetObject<ISpatialService<Building>>();
            var context = new SphDataContext();

            var item = await context.LoadOneAsync<Building>(b => b.BuildingId == buildingId);
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


        public async Task<ActionResult> GetFloorPlan(int id, string floor, string lot)
        {
            var context = new SphDataContext();

            var item = await context.LoadOneAsync<Building>(b => b.BuildingId == id);
            var lotItem = item.FloorCollection.Single(f => f.Name == floor).LotCollection.Single(l => l.Name == lot);
            if (string.IsNullOrWhiteSpace(lotItem.PlanStoreId)) return Json(new { EncodedPolygon = string.Empty }, JsonRequestBehavior.AllowGet);
            var line = await context.GetScalarAsync<SpatialStore, string>(s => s.StoreId == lotItem.PlanStoreId, s => s.EncodedWkt);

            var shape = new
                {
                    EncodedPolygon = line,
                    lotItem.FillColor,
                    lotItem.FillOpacity
                };

            return Json(shape, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> AddLotFloorPlan(int id, string floorname, string lot, string fillColor, double fillOpacity, string path)
        {
            var context = new SphDataContext();
            var building = await context.LoadOneAsync<Building>(b => b.BuildingId == id);
            var lotItem = building.FloorCollection.Single(f => f.Name == floorname).LotCollection.Single(l => l.Name == lot);
            lotItem.FillOpacity = fillOpacity;
            lotItem.FillColor = fillColor;
            lotItem.PlanStoreId = Guid.NewGuid().ToString();

            var points = path.Decode().ToList();
            if (
                Math.Abs(points.First().Lat - points.Last().Lat) > 0.00001
                || Math.Abs(points.First().Lng - points.Last().Lng) > 0.00001
                )
                points.Add(points.First().Clone());


            var store = new SpatialStore
                {
                    StoreId = lotItem.PlanStoreId,
                    Type = typeof(Lot).Name,
                    Tag = string.Format("{0};{1}{2}", id, floorname, lot),
                    EncodedWkt = path,
                    Wkt = points.ToWkt()
                };

            using (var session = context.OpenSession())
            {
                session.Attach(store, building);
                await session.SubmitChanges();
            }

            var spatial = ObjectBuilder.GetObject<ISpatialService<SpatialStore>>();
            await spatial.UpdateAsync(store);

            return Json(true);
        }

        public async Task<ActionResult> SaveBuilding(Building building)
        {
            var context = new SphDataContext();
            var item = await context.LoadOneAsync<Building>(b => b.BuildingId == building.BuildingId) ?? building;
            item.Address = building.Address;
            item.Name = building.Name;
            item.LotNo = building.LotNo;
            item.Floors = building.Floors;
            item.Size = building.Size;
            item.Status = building.Status;
            item.FloorCollection.ClearAndAddRange(building.FloorCollection);

            using (var session = context.OpenSession())
            {
                session.Attach(item);
                await session.SubmitChanges();
            }
            return Json(building.BuildingId);
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
