using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Web.Controllers
{
    public class OrganizationController : Controller
    {
        public async Task<ActionResult> Save(Organization organization)
        {
            var context = new SphDataContext();
            var dbItem = await context.LoadOneAsync<Organization>(o => o.OrganizationId == organization.OrganizationId) ?? organization;
            if (dbItem != organization)
            {
                dbItem.Name = organization.Name;
                dbItem.RegistrationNo = organization.RegistrationNo;
                dbItem.Address = organization.Address;
            }

            using (var session = context.OpenSession())
            {
                session.Attach(dbItem);
                await session.SubmitChanges();
            }
            
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await JsonConvert.SerializeObjectAsync(organization));

        }
    }
}
