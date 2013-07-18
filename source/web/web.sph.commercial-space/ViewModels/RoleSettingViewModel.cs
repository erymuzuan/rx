using Bespoke.Sph.Commerspace.Web.Controllers;
using Bespoke.Sph.Commerspace.Web.Models;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.ViewModels
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