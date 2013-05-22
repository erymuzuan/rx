using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class RentalApplicationController : Controller
    {
       public async Task<ActionResult> Save(RentalApplication rentalApplication)
       {
           var context = new SphDataContext();
           rentalApplication.Status = "Pending";
           using (var session=context.OpenSession())
           {    
              session.Attach(rentalApplication);
               await session.SubmitChanges();
           }

           return Json(true);
       }

       
       public async Task<ActionResult> Approved(int id)
       {
           var context = new SphDataContext();
           var dbItem = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
           dbItem.Status = "Approved";
           using (var session = context.OpenSession())
           {
               session.Attach(dbItem);
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
        
    }
}
