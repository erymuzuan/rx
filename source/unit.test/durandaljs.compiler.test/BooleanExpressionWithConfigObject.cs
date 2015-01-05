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
            StringAssert.Contains("config.UserName == \"Ali\"".CompileHtml(),"config.userName === 'Ali'");
        }

        [TestMethod]
        public void UserNameEqualItemName()
        {
            StringAssert.Contains("config.UserName == item.Name".CompileHtml(),"config.userName === $data.Name()");
        }

        [TestMethod]
        public void IsAuthenticated()
        {
            StringAssert.Contains("config.IsAuthenticated".CompileHtml(),"config.isAuthenticated");
        }

        [TestMethod]
        public void IsInRole()
        {
            StringAssert.Contains("config.Roles.Contains(\"clerk\")".CompileHtml(),"config.roles.indexOf('clerk') > -1");
        }
        [TestMethod]
        public void IsInRoleArgItem()
        {
            StringAssert.Contains(
                "config.Roles.Contains(item.Name)".CompileHtml(),
                "config.roles.indexOf($data.Name()) > -1");
        }

        [TestMethod]
        public void IsAuthenticatedAndItem()
        {
            var compileHtml = "config.IsAuthenticated && item.Name == \"Ali\"".CompileHtml();
            StringAssert.Contains(compileHtml,"config.isAuthenticated && $data.Name() === 'Ali'");
        }
    }
}
