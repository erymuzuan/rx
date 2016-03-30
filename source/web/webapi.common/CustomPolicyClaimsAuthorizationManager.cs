using System;
using System.Linq;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace Bespoke.Sph.WebApi
{
    public class CustomPolicyAuthorizationManager : ResourceAuthorizationManager
    {
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            var action = context.Action.FirstOrDefault(c => c.Type == "action")?.Value;
            var controller = context.Resource.FirstOrDefault(c => c.Type == "controller")?.Value;
            Console.WriteLine(action);
            Console.WriteLine(controller);

            var repos = ObjectBuilder.GetObject<IEndpointPermissionRepository>();
            var setting = repos.Settings.SingleOrDefault(x => x.Controller == controller && x.Action == action);
            if (null == setting) return Ok();

            var subject = context.Principal;
            if (setting.Claims.Any(clm => subject.HasClaim(x => x.Type == clm.Type && x.Value == clm.Value)))
            {
                return Ok();
            }


            return Nok();
        }
    }

}
