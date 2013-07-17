using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.SphCommercialSpaces.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class DesignationController : Controller
    {
        public async Task<ActionResult> Save(Designation designation)
        {
            var context = new SphDataContext();
            var users = await context.LoadAsync(context.UserProfiles.Where(u => u.Designation == designation.Name));
            foreach (var user in users.ItemCollection)
            {
                user.RoleTypes = string.Join(",", designation.RoleCollection);
                user.StartModule = designation.StartModule;

                using (var session = context.OpenSession())
                {
                    session.Attach(user);
                    await session.SubmitChanges("Update user designation");
                }
            }

            using (var session = context.OpenSession())
            {
                session.Attach(designation);
                await session.SubmitChanges("Add/update new designation");
            }
            this.Response.ContentType = "application/json; charset=utf-8";
            return Content(await JsonConvert.SerializeObjectAsync(designation));

        }

    }
}
