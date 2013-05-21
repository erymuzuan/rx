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
        
    }
}
