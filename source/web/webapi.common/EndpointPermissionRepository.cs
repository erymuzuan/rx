using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Bespoke.Sph.WebApi
{
    public class EndpointPermissionRepository : IEndpointPermissionRepository
    {
        public EndpointPermissionRepository()
        {
            var ps = new EndpointPermissonSetting
            {
                Controller = "PatientServiceContract",
                Action = "GetOneByIdAsync",
                Claims = new[]
                {
                    new Claim(ClaimTypes.Role, "administrators"),
                    new Claim(ClaimTypes.Role, "developers")
                }
            };
            this.Settings = new List<EndpointPermissonSetting> { ps };
        }
        public IEnumerable<EndpointPermissonSetting> Settings { get; }
        public Task<EndpointPermissonSetting> FindSettingsAsync(string controller, string action)
        {
            var setting = this.Settings.SingleOrDefault(x => x.Controller == controller && x.Action == action) ??
                          this.Settings.SingleOrDefault(x => x.Controller == controller);
            if (null == setting)
                setting = new EndpointPermissonSetting { Controller = controller, Action = action, Claims = new Claim[] { } };
            return Task.FromResult(setting);
        }
    }
}