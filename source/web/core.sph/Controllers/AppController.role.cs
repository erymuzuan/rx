using System.Linq;
﻿using System.Web.Mvc;
using Bespoke.Sph.Web.Models;
using Bespoke.Sph.Web.ViewModels;
using Newtonsoft.Json;
﻿using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Web.Controllers
{
    public partial class AppController
    {
        public ActionResult RoleSettingsHtml()
        {
            var rolesConfig = Server.MapPath("~/roles.config.js");
            var routeConfig = Server.MapPath("~/routes.config.js");
            var json = System.IO.File.ReadAllText(rolesConfig);
            var routeJson = System.IO.File.ReadAllText(routeConfig);

            var vm = new RoleSettingViewModel();
            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var roles = JsonConvert.DeserializeObject<RoleModel[]>(json, settings);
            var routes = JsonConvert.DeserializeObject<JsRoute[]>(routeJson, settings);
            vm.Roles.ClearAndAddRange(roles);
            vm.Routes.ClearAndAddRange(routes);
            return View(vm);
        }

        public ActionResult RoleSettingsJs()
        {
            var rolesConfig = Server.MapPath("~/roles.config.js");
            var json = System.IO.File.ReadAllText(rolesConfig);

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var roles = JsonConvert.DeserializeObject<RoleModel[]>(json, settings);
            
            return View(roles.AsEnumerable());
        }
    }
}