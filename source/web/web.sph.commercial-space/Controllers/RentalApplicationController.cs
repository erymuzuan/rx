using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class RentalApplicationController : Controller
    {

        public async Task<ActionResult> Submit(RentalApplication rentalApplication)
        {
            var context = new SphDataContext();
            rentalApplication.Status = "New";

            var audit = new AuditTrail
                {
                    Operation = "Submit",
                    DateTime =  DateTime.Now,
                    User = User.Identity.Name,
                    Type = typeof(RentalApplication).FullName,
                    EntityId = rentalApplication.RentalApplicationId
                };

            using (var session = context.OpenSession())
            {
                session.Attach(rentalApplication);
                await session.SubmitChanges();
            }
            using (var session = context.OpenSession())
            {
                audit.EntityId = rentalApplication.RentalApplicationId;
                session.Attach(audit);
                await session.SubmitChanges();
            }

            return Json(true);
        }

        public async Task<ActionResult> WaitingList(int id)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Status = "Waiting";
            using (var session = context.OpenSession())
            {
                session.Attach(dbItem);
                await session.SubmitChanges();
            }

            return Json(true);
        }

        public async Task<ActionResult> Approved(int id)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Status = "Approved";


            var audit = new AuditTrail
            {
                Operation = "Approval",
                DateTime = DateTime.Now,
                User = User.Identity.Name,
                Type = typeof(RentalApplication).FullName,
                EntityId = id
            };
            using (var session = context.OpenSession())
            {
                session.Attach(dbItem, audit);
                await session.SubmitChanges();
            }

            return Json(true);
        }

        public async Task<ActionResult> Declined(int id)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Status = "Declined";
            using (var session = context.OpenSession())
            {
                session.Attach(dbItem);
                await session.SubmitChanges();
            }

            return Json(true);
        }

        public async Task<ActionResult> Returned(int id)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Status = "Returned";
            using (var session = context.OpenSession())
            {
                session.Attach(dbItem);
                await session.SubmitChanges();
            }

            return Json(true);
        }

        public async Task<ActionResult> GenerateOfferLetter(int id)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Status = "WaitingConfirmation";
            using (var session = context.OpenSession())
            {
                session.Attach(dbItem);
                await session.SubmitChanges();
            }

            return Json(true);
        }

        public async Task<ActionResult> RejectedOfferLetter(int id)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Status = "OfferRejected";
            using (var session = context.OpenSession())
            {
                session.Attach(dbItem);
                await session.SubmitChanges();
            }

            return Json(true);
        }

        public async Task<ActionResult> Confirmed(int id)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            dbItem.Status = "Confirmed";
            using (var session = context.OpenSession())
            {
                session.Attach(dbItem);
                await session.SubmitChanges();
            }

            return Json(true);
        }


    }
}
