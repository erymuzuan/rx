using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class TenantController : Controller
    {
        public async Task<ActionResult> Create(int id)
        {
            var context = new SphDataContext();
            var application = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            var tenant = await context.LoadOneAsync<Tenant>(t => t.IdSsmNo == application.CompanyRegistrationNo || t.IdSsmNo == application.Contact.IcNo) ?? new Tenant{};

            tenant.Name = application.CompanyName ?? application.Contact.Name;
            tenant.IdSsmNo = application.CompanyRegistrationNo ?? application.Contact.IcNo;
            tenant.BussinessType = application.CompanyType;
            tenant.Phone = application.Contact.OfficeNo;
            tenant.MobilePhone = application.Contact.MobileNo;
            tenant.Email = application.Contact.Email;
            tenant.RegistrationNo = application.RegistrationNo;
            tenant.Address = application.Address;

            using (var session = context.OpenSession())
            {
                session.Attach(tenant);
                await session.SubmitChanges();
            }
            return Json(true);
        }

    }
}
