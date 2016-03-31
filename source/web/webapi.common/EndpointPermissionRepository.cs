using System.Collections.Generic;
using System.Security.Claims;

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
    }
}