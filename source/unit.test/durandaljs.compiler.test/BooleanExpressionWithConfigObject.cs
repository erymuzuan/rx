using Bespoke.Sph.Domain;
using Bespoke.Sph.Templating;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace durandaljs.compiler.test
{
    [TestClass]
    public class BooleanExpressionWithConfigObject
    {
        [TestInitialize]
        public void SetUp()
        {
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
        }
        /*
         * 
         * debugEnabled: ko.observable(true),
            imageSettings: imageSettings,
            userName: '@User.Identity.Name',
            isAuthenticated : @(User.Identity.IsAuthenticated.ToString().ToLower()),
            routes: routes,
            startModule: startModule,
            stateOptions : @Html.Raw(Model.StateOptions),
            departmentOptions : @Html.Raw(Model.DepartmentOptions),
            applicationFullName :'@Model.ApplicationFullName',
            applicationName :'@Model.ApplicationName',
            roles :[ @Html.Raw(string.Join(",", Roles.GetRolesForUser(User.Identity.Name).Select(u => string.Format("'{0}'", u))))],
            allRoles :[ @Html.Raw(string.Join(",", Roles.GetAllRoles().Select(u => string.Format("'{0}'", u))))],
            profile : @Html.Raw(JsonConvert.SerializeObject(Model.UserProfile))
    
         * */

        [TestMethod]
        public void UserName()
        {
            Assert.AreEqual(
                "config.userName === 'Ali'",
                "config.UserName == \"Ali\"".CompileHtml());
        }

        [TestMethod]
        public void UserNameEqualItemName()
        {
            Assert.AreEqual("config.userName === $data.Name()",
                "config.UserName == item.Name".CompileHtml());
        }

        [TestMethod]
        public void IsAuthenticated()
        {
            StringAssert.Contains("config.IsAuthenticated".CompileHtml(), "config.isAuthenticated");
        }

        [TestMethod]
        public void IsInRole()
        {
            Assert.AreEqual("config.roles.indexOf('clerk') > -1",
                "config.Roles.Contains(\"clerk\")".CompileHtml(), "");
        }
        [TestMethod]
        public void StringArrayCount()
        {
            Assert.AreEqual("config.roles.length === 1",
                "config.Roles.Count() == 1".CompileHtml(), "");
        }

        [TestMethod]
        [Ignore]
        public void StringArrayLength()
        {
            Assert.AreEqual("config.roles.length  === 1",
                "config.Roles.Length == 2".CompileHtml(), "Use Count() method");
        }

        [TestMethod]
        public void IsInRoleArgItem()
        {
            Assert.AreEqual(
                "config.roles.indexOf($data.Name()) > -1",
                "config.Roles.Contains(item.Name)".CompileHtml(), "");
        }

        [TestMethod]
        public void IsAuthenticatedAndItem()
        {
            var compileHtml = "config.IsAuthenticated && item.Name == \"Ali\"".CompileHtml();
            Assert.AreEqual(
                "config.isAuthenticated && $data.Name() === 'Ali'",
                compileHtml);
        }
    }
}
