using System.Security.Claims;
using System.Threading.Tasks;
using Bespoke.Sph.WebApi;
using Xunit;

namespace webapi.common.test
{
    public class PermissionSettingTest
    {
        [Fact]
        public void BuildHierachy()
        {
            var eps = new EndpointPermissonSetting
            {
                Claims = new[] { new ClaimSetting("email", "erymuzuan", "a") }
            };

            eps.OverrideClaims(new ClaimSetting("email", "erymuzuan", "d"));
            Assert.Equal(1, eps.Claims.Length);
            Assert.Equal("d", eps.Claims[0].Permission);
        }
        [Fact]
        public async Task CheckAccessAllowed()
        {
            var eps = new EndpointPermissonSetting { Claims = new[]
            {
                new ClaimSetting(ClaimTypes.Role, "administrators", "a"),
                new ClaimSetting(ClaimTypes.Role, "developers", "a")
            
            } };
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, "administrators"),
                new Claim(ClaimTypes.Role, "developers")
            };
            var subject = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var allowed = await eps.CheckAccessAsync(subject);
            Assert.True(allowed);
        }
        [Fact]
        public async Task CheckAccessNoSetting()
        {
            var eps = new EndpointPermissonSetting();
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, "administrators"),
                new Claim(ClaimTypes.Role, "developers")
            };
            var subject = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var allowed = await eps.CheckAccessAsync(subject);
            Assert.True(allowed);
        }
        [Fact]
        public async Task CheckAccessDenied()
        {
            var eps = new EndpointPermissonSetting { Claims = new[]
            {
                new ClaimSetting(ClaimTypes.Role, "administrators", "a"),
                new ClaimSetting(ClaimTypes.Role, "developers", "d")
            
            } };
            var claims = new[]
            {
                new Claim(ClaimTypes.Role, "administrators"),
                new Claim(ClaimTypes.Role, "developers")
            };
            var subject = new ClaimsPrincipal(new ClaimsIdentity(claims));
            var allowed = await eps.CheckAccessAsync(subject);
            Assert.False(allowed);
        }
    }
}