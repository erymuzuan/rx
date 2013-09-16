using Bespoke.Sph.Web.Controllers;
using Bespoke.Sph.Web.Models;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.ViewModels
{
    public class RoleSettingViewModel
    {
        private readonly ObjectCollection<RoleModel> m_rolesCollection = new ObjectCollection<RoleModel>();
        private readonly ObjectCollection<JsRoute> m_routesCollection = new ObjectCollection<JsRoute>();

        public ObjectCollection<JsRoute> Routes
        {
            get { return m_routesCollection; }
        }
        public ObjectCollection<RoleModel> Roles
        {
            get { return m_rolesCollection; }
        }
    }
}