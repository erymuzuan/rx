using System;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Templating;
using domain.test.triggers;
using NUnit.Framework;

namespace domain.test.razor
{
    [TestFixture]
    public class RazorEngineTestFixture
    {
        [SetUp]
        public void Setup()
        {
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockLdap());
        }

        [Test]
        public async Task Engine()
        {
            var razor = new RazorEngine();
            var result = await razor.GenerateAsync("My name is @Model.Name", new { Name = "Erymuzuan" });
            Assert.AreEqual("My name is Erymuzuan", result);
            var result1 = await razor.GenerateAsync("My name is @Model.Name", new { Name = "Michaes" });
            Assert.AreEqual("My name is Michaes", result1);
            var result2 = await razor.GenerateAsync("My name is @Model.Address.State", new { Address = new { State = "Erymuzuan" } });
            Assert.AreEqual("My name is Erymuzuan", result2);
        }

        [Test]
        public async Task ViewBag()
        {
            var ldap = ObjectBuilder.GetObject<IDirectoryService>();
            var razor = new RazorEngine();
            var result = await razor.GenerateAsync("My name is @Model.Name created by @ViewBag.UserName on @DateTime.Today.ToString(\"f\")", new { Name = "Erymuzuan" });
            Assert.AreEqual(string.Format("My name is Erymuzuan created by {0} on {1:f}", ldap.CurrentUserName, DateTime.Today), result);
        }

    }
}