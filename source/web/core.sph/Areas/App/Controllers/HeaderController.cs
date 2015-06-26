using System.Threading.Tasks;
using System.Web.Mvc;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Web.Filters;

namespace Bespoke.Sph.Web.Areas.App.Controllers
{
    public class HeaderController : BaseAppController
    {
        [NoCache]
        public async Task<ActionResult> Html()
        {
            if (!User.Identity.IsAuthenticated)
                return View(new HtmlHeaderModel {Designation = new Designation {IsHelpVisible = false}});
            
            var context = new SphDataContext();
            var profile = await context.LoadOneAsync<UserProfile>(ua => ua.UserName == User.Identity.Name);
            if (null == profile)
                return View(new HtmlHeaderModel { Designation = new Designation { IsHelpVisible = false } });

            ViewBag.StartModule = "#" + profile.StartModule;

            var designation = (await context.LoadOneAsync<Designation>(d => d.Name == profile.Designation)) ?? new Designation { IsHelpVisible = true, HelpUri = "/docs/" };
            designation.HelpUri = string.IsNullOrWhiteSpace(designation.HelpUri) ? "/docs/" : designation.HelpUri;
            var vm = new HtmlHeaderModel
            {
                Profile = profile,
                Designation = designation
            };
            return View(vm);
        }
    }

    public class HtmlHeaderModel
    {
        public Designation Designation { get; set; }
        public UserProfile Profile { get; set; }
    }
}