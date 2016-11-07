using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Bespoke.Sph.WebApi;
using Bespoke.Sph.Domain;
using Humanizer;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace webapi.common.test
{
    public class PermissionRepositoryTest
    {
        private ITestOutputHelper Console { get; }
        private readonly IList<EndpointPermissonSetting> m_permissionTree = new List<EndpointPermissonSetting>();

        public PermissionRepositoryTest(ITestOutputHelper helper)
        {
            Console = helper;
            BuildTree();
            var cache = new Mock<ICacheManager>();
            cache.Setup(x => x.Get<EndpointPermissonSetting[]>(Constants.ENDPOINT_PERMISSIONS_CACHE_KEY))
                .Returns(() => m_permissionTree.ToArray());
            ObjectBuilder.AddCacheList(cache.Object);
        }

        private void BuildTree()
        {

            m_permissionTree.Clear();
            m_permissionTree.Add(new EndpointPermissonSetting
            {
                Parent = "Parent1",
                Controller = "Controller1",
                Action = "Action1",
                Claims = new[]
                {
                    new ClaimSetting("email", "erymuzuan@gmail.com","a"),
                }

            });
            m_permissionTree.Add(new EndpointPermissonSetting
            {
                Parent = "Custom",
                Claims = new[]
                {
                    new ClaimSetting(ClaimTypes.Anonymous, "true","d"),
                }

            });
            m_permissionTree.Add(new EndpointPermissonSetting
            {
                Parent = "Custom",
                Controller = "CustomCustomer",
                Claims = new[]
                {
                    new ClaimSetting(ClaimTypes.Anonymous, "true","a"),
                }

            });
            m_permissionTree.Add(new EndpointPermissonSetting
            {
                Parent = "Parent1",
                Claims = new[]
                {
                    new ClaimSetting("role", "clerk","a"),
                }

            });
            m_permissionTree.Add(new EndpointPermissonSetting
            {
                Claims = new[]
                {
                    new ClaimSetting(ClaimTypes.Role, "developers","a"),
                }

            });
            Console.WriteLine("Building tree for " + m_permissionTree.Count);

        }


        [Fact]
        public async Task TreeAsync()
        {
            var repos = new EndpointPermissionRepository();
            var setting = await repos.FindSettingsAsync("Controller1", "Action1");
            Assert.NotNull(setting);

            Assert.Equal(3, setting.Claims.Length);
        }
        [Fact]
        public async Task TreeActionNotInTree()
        {
            var repos = new EndpointPermissionRepository();
            var setting = await repos.FindSettingsAsync("Controller1", "Action2");
            Assert.NotNull(setting);

            Assert.Equal(2, setting.Claims.Length);
        }

        [Fact]
        public async Task CustomAction()
        {
            var repos = new EndpointPermissionRepository();
            var setting = await repos.FindSettingsAsync("CustomCustomer", "FindWithCreditRatings");
            Assert.NotNull(setting);

            Assert.Equal(2, setting.Claims.Length);
            Assert.Equal(ClaimTypes.Role, setting.Claims[0].Type);
            Assert.Equal(ClaimTypes.Anonymous, setting.Claims[1].Type);
        }

        [Fact]
        public async Task CheckAccessAllowed()
        {
            var setting = new EndpointPermissonSetting
            {
                Claims = new[]
                {
                    new ClaimSetting(ClaimTypes.Role, "developers", "a"),
                    new ClaimSetting(ClaimTypes.Role, "customers", "d")
                }
            };

            var claims = new[]
            {
                new Claim(ClaimTypes.Role, "developers")
            };
            var subject = new ClaimsPrincipal();
            var identity = new ClaimsIdentity(claims);
            subject.AddIdentity(identity);
            var authorized = await setting.CheckAccessAsync(subject);

            Assert.True(authorized);

        }
        [Fact]
        public async Task CheckAccessDenied()
        {
            var setting = new EndpointPermissonSetting
            {
                Claims = new[]
                {
                    new ClaimSetting(ClaimTypes.Role, "developers", "a"),
                    new ClaimSetting(ClaimTypes.Role, "customers", "d")
                }
            };

            var subject = CreateClaimsPrincipal($"{ClaimTypes.Role},developers", $"{ClaimTypes.Role},customers", $"{ClaimTypes.Email},erymuzuan@gmail.com");
            var authorized = await setting.CheckAccessAsync(subject);

            Assert.False(authorized);

        }

        /// <summary>
        /// claims in Type,Value
        /// </summary>
        /// <param name="claims">Type,Value</param>
        /// <returns></returns>
        private static ClaimsPrincipal CreateClaimsPrincipal(params string[] claims)
        {
            var list = from t in claims
                       let w = t.Split(',')
                       select new Claim(w[0], w[1]);

            var subject = new ClaimsPrincipal();
            var identity = new ClaimsIdentity(list);
            subject.AddIdentity(identity);
            return subject;
        }

        [Fact]
        public async Task CustomWithAnonymous()
        {
            var repos = new EndpointPermissionRepository();
            var setting = await repos.FindSettingsAsync("CustomCustomer", "FindWithCreditRatings");

            var subject = CreateClaimsPrincipal($"{ClaimTypes.Anonymous},true");
            var authorized = await setting.CheckAccessAsync(subject);

            Assert.True(authorized, PrintTable(setting.Claims));

        }
        [Fact]
        public async Task CustomWithEmail()
        {
            m_permissionTree.Add(new EndpointPermissonSetting
            {
                Parent = "Custom",
                Controller = "CustomWithEmail",
                Claims = new[]
                {
                    new ClaimSetting(ClaimTypes.Email, "erymuzuan@gmail.com", "a"),
                    new ClaimSetting(ClaimTypes.Email, "erymuzuan@yahoo.com", "d"),
                }
            });
            var repos = new EndpointPermissionRepository();
            var setting = await repos.FindSettingsAsync("CustomWithEmail", "FindMe");

            var gmail = CreateClaimsPrincipal($"{ClaimTypes.Email},erymuzuan@gmail.com");
            var gmailOk = await setting.CheckAccessAsync(gmail);

            Assert.True(gmailOk, PrintTable(setting.Claims));

            var yahoo = CreateClaimsPrincipal($"{ClaimTypes.Email},erymuzuan@yahoo.com");
            var yahooOk = await setting.CheckAccessAsync(yahoo);

            Assert.False(yahooOk, PrintTable(setting.Claims));

        }

        private string PrintTable(ClaimSetting[] settings)
        {
            var table = new StringBuilder();
            table.AppendLine("|                   Type                                        |   Value    | A |");
            table.AppendLine("|--------------------------------------------------------------------------------|");
            foreach (var p in settings)
            {
                table.AppendLine($"|{p.Type.ToFix(63)}| {p.Value.ToFix(11)}| {p.Permission.ToFix(2)}|");

            }
            return table.ToString();
        }
    }

    public static class StringExtensions
    {
        public static string ToFix(this string value, int length)
        {
            var truncate = value.Truncate(length, Truncator.FixedLength, TruncateFrom.Left);
            if (truncate.Length == length)
                return truncate;
            return value + new string(' ', length - value.Length);
        }
    }
}