
﻿using System.Threading.Tasks;
﻿using System.Web.Mvc;
﻿using Bespoke.Sph.Commerspace.Web.ViewModels;
﻿using Bespoke.SphCommercialSpaces.Domain;

namespace Bespoke.Sph.Commerspace.Web.Controllers
{
    public partial class AppController
    {
        public async Task<ActionResult>  HeaderHtml()
        {
            var vm = new HeaderViewModel();
            var context = new SphDataContext();
            var user = User.Identity.Name;
            var profile = await context.LoadOneAsync<UserProfile>(u => u.Username == user);
            vm.StartModule = profile.StartModule;
            return View(vm);
        }
    }
}