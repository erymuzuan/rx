using System.Data.SqlClient;
using System.Linq;
using System.Web.Security;
using FluentDateTime;
using NUnit.Framework;

namespace web.test
{

    [TestFixture]
// ReSharper disable  InconsistentNaming
    public class _000_AdminTest : BrowserTest
    {
        private TestUser m_admin;

        [SetUp]
        public void Init()
        {
            m_admin = new TestUser
            {
                UserName = "admin",
                FullName = "Administrator",
                Email = "admin@bespoke.com.my",
                Department = "Test",
                Designation = "Boss",
                Password = "123456",
                Roles = new[] { "admin_user", "admin_dashboard" }
            };
            this.AddUser(m_admin);
        }

        public const string USERNAME = "izzati";

        [Test]
// ReSharper disable InconsistentNaming
        public void _001_AddUser()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[UserProfile] WHERE [UserName] = @UserName",
                new SqlParameter("@UserName", USERNAME));

            var designation = this.GetDatabaseScalarValue<string>("SELECT MAX([Name]) FROM [Sph].[Designation]");
            if (Membership.GetUser(USERNAME) != null)
            {
                Membership.DeleteUser(USERNAME);
            }

            var driver = this.InitiateDriver();
            driver.Login(m_admin)

             .NavigateToUrl("/#/users", 2.Seconds());
            driver.ClickFirst("button", e => e.Text == "Tambah Pengguna")
                .Sleep(1.Seconds());

            driver.Value("[name=Username]", USERNAME)
                  .Value("[name=Password]", "123456")
                  .Value("[name=ConfirmPassword]", "123456")
                  .Value("[name=FullName]", "Noor Izzati")
                  .Value("[name=Email]", "izzati@hotmail.com")
                  .SelectOption("[name=designation]", designation)
                  .Value("[name=Telephone]", "013-7724568");

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

        [Test]
        public void _002_CreateLotsOfUsers()
        {
            var users = from i in Enumerable.Range(1, 100)
                select new TestUser
                {
                    UserName = "admin" + i,
                    FullName = "Administrator",
                    Email = string.Format("admin{0}@bespoke.com.my",i),
                    Department = "Test",
                    Designation = "Boss",
                    Password = "123456",
                    Roles = new[] { "admin_user" }
                };
            foreach (var u in users)
            {
                this.AddUser(u);
            }
        }
    }
}
