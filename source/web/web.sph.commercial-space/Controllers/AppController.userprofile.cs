using System.Linq;
﻿using System.Threading.Tasks;
﻿using System.Web.Mvc;
using System.Web.Security;
﻿using Bespoke.Sph.Commerspace.Web.Models;
﻿using Bespoke.Sph.Commerspace.Web.ViewModels;
﻿using Bespoke.SphCommercialSpaces.Domain;
﻿using Newtonsoft.Json;
﻿using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {
        public ActionResult UserProfileHtml()
        {
            return View();
        }
        public async Task<ActionResult> UserProfileJs()
        {
            var context = new SphDataContext();
            var user = Membership.GetUser();
            var profile = await context.LoadOneAsync<UserProfile>(p => p.Username == User.Identity.Name)
                 ?? new UserProfile { Username = User.Identity.Name };

            var routeConfig = Server.MapPath("~/routes.config.js");
            var json = System.IO.File.ReadAllText(routeConfig);

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var modules = JsonConvert.DeserializeObject<JsRoute[]>(json, settings).AsQueryable()
                .Where(r => r.ShowWhenLoggedIn || User.IsInRole(r.Role))
                .Where(r => r.Visible)
                .Select(r => r.Url)
                .ToArray();

            var vm = new UserProfileViewModel
            {
                Profile = profile,
                User = user,
                StartModuleOptions = modules
            }
            ;
            return View(vm);
        }
    }
}