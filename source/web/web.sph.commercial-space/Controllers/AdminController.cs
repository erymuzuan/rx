using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Bespoke.SphCommercialSpaces.Domain;
using Newtonsoft.Json;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public class AdminController : Controller
    {

        public ActionResult AddRole(string role)
        {
            Roles.CreateRole(role);
            return Json(true);
        }
        public async Task<ActionResult> AddUser(UserProfile userprofile)
        {
            var context = new SphDataContext();
            var user = await context.LoadOneAsync<UserProfile>(u => u.Username == userprofile.Username) ?? new UserProfile();
            var setting = await context.LoadOneAsync<Setting>(s => s.Key == "Designation.Role");
            var designation = await JsonConvert.DeserializeObjectAsync<Designation[]>(setting.Value);
            var userRoles = designation.Where(d => d.Name == userprofile.Designation).Select(d => d.Roles);
            user.RoleTypes = string.Join(",", userRoles);
            return Json(true);
        }
        public ActionResult LoadRoles()
        {
            var roleCollection = new List<string>();
            var roles = Roles.GetAllRoles();
            roleCollection.AddRange(roles);
            return Json(roleCollection);
        }
    }

    public class Designation
    {
        public string Name { get; set; }
        private readonly ObjectCollection<string> m_roles = new ObjectCollection<string>();

        public ObjectCollection<string> Roles
        {
            get { return m_roles; }
        }
    }
}
