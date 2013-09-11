using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class TenantController : Controller
    {
        public async Task<ActionResult> Create(int id, string username)
        {
            var context = new SphDataContext();
           
            var application = await context.LoadOneAsync<RentalApplication>(r => r.RentalApplicationId == id);
            var tenant = await context.LoadOneAsync<Tenant>(t => t.IdSsmNo == application.CompanyRegistrationNo || t.IdSsmNo == application.Contact.IcNo) ?? new Tenant();

            if (tenant.TenantId > 0) return Json(false);

            tenant.Name = application.CompanyName ?? application.Contact.Name;
            tenant.IdSsmNo = application.CompanyRegistrationNo ?? application.Contact.IcNo;
            tenant.BussinessType = application.CompanyType;
            tenant.Phone = application.Contact.OfficeNo;
            tenant.MobilePhone = application.Contact.MobileNo;
            tenant.Email = application.Contact.Email;
            tenant.RegistrationNo = application.RegistrationNo;
            tenant.Address = application.Address;
            tenant.Username = username;
            
            var userprofile = await CreateUserProfile(tenant);

            using (var session = context.OpenSession())
            {
                session.Attach(tenant, userprofile);
                await session.SubmitChanges();
            }
            return Json(true);
        }

        private static async Task<UserProfile> CreateUserProfile(Tenant tenant)
        {
            var context = new SphDataContext();
            var designation = await context.LoadOneAsync<Designation>(d => d.Name == "Penyewa");
            if (null == designation) throw new InvalidOperationException("Cannot find designation " + "Penyewa");
            var roles = designation.RoleCollection.ToArray();

            var userProfile = new UserProfile
                {
                    FullName = tenant.Name,
                    StartModule  = designation.StartModule,
                    Designation = designation.Name,
                    Email = tenant.Email,
                    Telephone = string.Join(",", new[]{tenant.Phone,tenant.MobilePhone}),
                    Username = tenant.Username,
                    RoleTypes = string.Join(",",roles)
                };

            Membership.CreateUser(userProfile.Username, "123456", userProfile.Email);
            Roles.AddUserToRoles(userProfile.Username, roles);
            return userProfile;
        }
    }
}
