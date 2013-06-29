using System.Collections.Generic;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Helpers
{
    public static class Roles
    {
        public const string ADMIN_DASHBOARD = "admin_dashboard";
        public const string ADMIN_SETTING = "admin_setting";

        public static List<Permission> Permissions
        {
            get
            {
                var list = new List<Permission>();
                return list;
            }
        }
    }
}