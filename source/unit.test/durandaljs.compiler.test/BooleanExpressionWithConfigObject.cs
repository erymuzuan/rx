using Bespoke.Sph.Domain;
using System.Threading.Tasks;
using NUnit.Framework;


namespace durandaljs.compiler.test
{
    [TestFixture]
    public class BooleanExpressionWithConfigObject : ExpressionTestFixture
    {


        [Test]
        public void UserName()
        {
            Assert.AreEqual(
                "config.userName === 'Ali'",
                "config.UserName == \"Ali\"".CompileHtml());
        }

        [Test]
        public void UserNameEqualItemName()
        {
            Assert.AreEqual("config.userName === $data.Name()",
                "config.UserName == item.Name".CompileHtml());
        }

        [Test]
        public void IsAuthenticated()
        {
            StringAssert.Contains("config.IsAuthenticated".CompileHtml(), "config.isAuthenticated");
        }

        [Test]
        [Trace(Verbose = true)]
        public void IsInRole()
        {
            Assert.AreEqual("config.roles.indexOf('clerk') > -1",
                "config.Roles.Contains(\"clerk\")".CompileHtml(), "");
        }
        [Test]
        public void StringArrayCount()
        {
            Assert.AreEqual("config.roles.length === 1",
                "config.Roles.Count() == 1".CompileHtml(), "");
        }

        [Test]
        public async Task StringArrayLength()
        {
            await AssertAsync<int>(
                "config.roles.length",
                "config.Roles.Length");
        }

        [Test]
        public void IsInRoleArgItem()
        {
            Assert.AreEqual(
                "config.roles.indexOf($data.Name()) > -1",
                "config.Roles.Contains(item.Name)".CompileHtml(), "");
        }

        [Test]
        public void IsAuthenticatedAndItem()
        {
            var compileHtml = "config.IsAuthenticated && item.Name == \"Ali\"".CompileHtml();
            Assert.AreEqual(
                "config.isAuthenticated && $data.Name() === 'Ali'",
                compileHtml);
        }
    }
}
