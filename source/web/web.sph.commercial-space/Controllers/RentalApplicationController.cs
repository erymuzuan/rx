﻿using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class RentalApplicationController : Controller
    {
       public async Task<ActionResult> SaveRentalApplicationOne(RentalApplication rentalApplication)
       {
           var context = new SphDataContext();
           using (var session=context.OpenSession())
           {
              session.Attach(rentalApplication);
               await session.SubmitChanges();
           }

           return Json(true);
       }

    }
}
