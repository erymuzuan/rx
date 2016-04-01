using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace Bespoke.Sph.WebApi
{
    public class CustomPolicyAuthorizationManager : ResourceAuthorizationManager
    {
        public override async Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            var action = context.Action.FirstOrDefault(c => c.Type == "action")?.Value;
            var controller = context.Resource.FirstOrDefault(c => c.Type == "controller")?.Value;
            Console.WriteLine(action);
            Console.WriteLine(controller);

            var repos = ObjectBuilder.GetObject<IEndpointPermissionRepository>();
            var setting = await repos.FindSettingsAsync(controller, action);
            if (null == setting) return true;

            var subject = context.Principal;
            var authorized = await setting.CheckAccessAsync(subject);
            return authorized;
        }
    }

}
