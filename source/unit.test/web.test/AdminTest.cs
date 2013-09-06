using System.Data.SqlClient;
using System.Web.Security;
using FluentDateTime;
using NUnit.Framework;

namespace web.test
{

    [TestFixture]
    public class AdminTest : BrowserTest
    {
        public const string USERNAME = "izzati";

        [Test]
        public void _001_AddUser()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[UserProfile] WHERE [UserName] = @UserName",
                new SqlParameter("@UserName", USERNAME));

            if (Membership.GetUser(USERNAME) != null)
            {
                Membership.DeleteUser(USERNAME);
            }

            var driver = this.InitiateDriver();
            driver.Login()

             .NavigateToUrl("/#/users", 2.Seconds());
            driver.Click("#add-user-button")
                .Sleep(1.Seconds());

            driver.Value("[name='Username']", USERNAME)
                  .Value("[name='Password']", "123456")
                  .Value("[name='ConfirmPassword']", "123456")
                  .Value("[name='FullName']", "Noor Izzati")
                  .Value("[name='Email']", "izzati@hotmail.com")
                  .SelectOption("[name='designation']", "Manager")
                  .Value("[name='Telephone']", "013-7724568");

            driver.Sleep(2.Seconds());
            driver.Click("#save-button")
            .Sleep(2.Seconds());

            var count = this.GetDatabaseScalarValue<int>("SELECT COUNT([UserProfileId]) FROM [Sph].[UserProfile] WHERE [Username] =@Username", new SqlParameter("@Username", USERNAME));
            Assert.AreEqual(1, count, "There should only be 1 izzati");


            var user = Membership.GetUser(USERNAME);
            Assert.IsNotNull(user);


            driver.Sleep(5.Seconds(), "See the result");
            driver.Quit();
        }


    }
}
