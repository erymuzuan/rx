using System.Linq;
﻿using System.Web.Mvc;
﻿using Newtonsoft.Json;
﻿using Newtonsoft.Json.Serialization;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {
        public ActionResult RoleSettingsHtml()
        {
            var rolesConfig = Server.MapPath("~/roles.config.js");
            var json = System.IO.File.ReadAllText(rolesConfig);

            var settings = new JsonSerializerSettings { ContractResolver = new CamelCasePropertyNamesContractResolver() };
            var roles = JsonConvert.DeserializeObject<RoleModel[]>(json, settings);
            
            return View(roles);
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

    public class RoleModel
    {
        public string Group { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}