<<<<<<< HEAD
﻿using System.Collections.Generic;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Helpers
{
    public static class Roles
    {
        public const string ADMIN_DASHBOARD = "admin_dashboard";
        public const string ADMIN_SETTING = "admin_setting";
        public const string ADMIN_USERS = "admin_users";

        public static List<Permission> Permissions
        {
            get
            {
                var list = new List<Permission>();
                return list;
            }
        }
    }
=======
﻿using System.Collections.Generic;
using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Helpers
{
    public static class Roles
    {
        public const string ADMIN_DASHBOARD = "admin_dashboard";
        public const string ADMIN_SETTING = "admin_setting";
        public const string ADMIN_USERS = "admin_users";

        public static List<Permission> Permissions
        {
            get
            {
                var list = new List<Permission>();
                return list;
            }
        }
    }
>>>>>>> 7d25030947e14ce64ad0e9692662c7745ee785e9
}