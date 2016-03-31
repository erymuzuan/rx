using System.Security.Claims;

namespace Bespoke.Sph.WebApi
{
    public class EndpointPermissonSetting
    {
        public string Controller { get; set; }
        public string Action { get; set; }
        public string Id { get; set; }
        public Claim[] Claims { get; set; }
    }
}