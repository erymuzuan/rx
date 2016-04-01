using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace Bespoke.Sph.WebApi
{
    public class EndpointPermissonSetting
    {
        public string Parent { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Id { get; set; }
        public Claim[] Claims { get; set; }

        public Task<bool> CheckAccessAsync(ClaimsPrincipal subject)
        {
            if (this.Claims == null) return Task.FromResult(true);
            if (this.Claims.Length == 0) return Task.FromResult(true);

            if (this.Claims.Any(clm => subject.HasClaim(x => x.Type == clm.Type && x.Value == clm.Value)))
            {
                return Task.FromResult(true);
            }


            return Task.FromResult(false);
        }
    }
}