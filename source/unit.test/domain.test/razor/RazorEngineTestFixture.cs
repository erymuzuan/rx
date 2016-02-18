using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Templating;
using domain.test.triggers;
using Xunit;

namespace domain.test.razor
{
    
    public class RazorEngineTestFixture
    {
        public RazorEngineTestFixture()
        {
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
        }

        [Fact]
        public async Task Engine()
        {
            var razor = new RazorEngine();
            var result = await razor.GenerateAsync("My name is @Model.Name", new { Name = "Erymuzuan" });
            Assert.Equal("My name is Erymuzuan", result);
            var result1 = await razor.GenerateAsync("My name is @Model.Name", new { Name = "Michaes" });
            Assert.Equal("My name is Michaes", result1);
            var result2 = await razor.GenerateAsync("My name is @Model.Address.State", new { Address = new { State = "Erymuzuan" } });
            Assert.Equal("My name is Erymuzuan", result2);
        }

        [Fact]
        public async Task ViewBag()
        {
            var ldap = ObjectBuilder.GetObject<IDirectoryService>();
            var razor = new RazorEngine();
            var result = await razor.GenerateAsync("My name is @Model.Name created by @ViewBag.UserName on @DateTime.Today.ToString(\"f\")", new { Name = "Erymuzuan" });
            Assert.Equal(string.Format("My name is Erymuzuan created by {0} on {1:f}", ldap.CurrentUserName, DateTime.Today), result);
        }

    }
}