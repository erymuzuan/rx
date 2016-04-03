using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebApi;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace Bespoke.Sph.Web.OwinMiddlewares
{
    public class CustomPolicyAuthorizationManager : ResourceAuthorizationManager
    {
        public override async Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            var action = context.Action.FirstOrDefault(c => c.Type == "action")?.Value;
            var controller = context.Resource.FirstOrDefault(c => c.Type == "controller")?.Value;
            if (null != HttpContext.Current)
            {
                var oc = HttpContext.Current.GetOwinContext();
                oc.Set("rx:controller", controller);
                oc.Set("rx:action", action);
            }

            var repos = ObjectBuilder.GetObject<IEndpointPermissionRepository>();
            var setting = await repos.FindSettingsAsync(controller, action);
            if (null == setting) return true;

            var subject = context.Principal;
            var authorized = await setting.CheckAccessAsync(subject);
            return authorized;
        }
    }
}