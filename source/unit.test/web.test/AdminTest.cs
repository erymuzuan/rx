using System;
using System.Data.SqlClient;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace web.test
{

    [TestFixture]
    public class AdminTest : BrowserTest
    {
        public const string USERNAME = "izzati";
        [Test]
        public void _001_AddUser()
        {
            const string sphDatabase = "sph";

            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + "/#/users");
            driver.Sleep(TimeSpan.FromSeconds(3));
            driver.Click("#add-user-button");
            driver.Sleep(TimeSpan.FromSeconds(3));
            driver.Value("[name='Username']", USERNAME)
                  .Value("[name='Password']", "123456")
                  .Value("[name='ConfirmPassword']", "123456")
                  .Value("[name='FullName']", "Noor Izzati")
                  .Value("[name='Email']", "izzati@hotmail.com")
                  .SelectOption("[name='designation']", "Manager")
                  .Value("[name='Telephone']", "013-7724568");
            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.Click("#save-button")
            .Sleep(TimeSpan.FromSeconds(5));

            var id = sphDatabase.GetDatabaseScalarValue<int>("SELECT [UserProfileId] FROM [Sph].[UserProfile] WHERE [Username] =@Username", new SqlParameter("@Username", USERNAME));
            Assert.IsTrue(id > 0);
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }
    }
}
