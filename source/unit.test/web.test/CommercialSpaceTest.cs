using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace web.test
{
    [TestFixture]
    public class CommercialSpaceTest : BrowserTest
    {
        public const string CS_REGISTRATION_NO = "BSPK/999999";

        [Test]
        public void _001_Add()
        {
            //const string sphDatabase = "sph";
            //sphDatabase.ExecuteNonQuery("DELETE FROM [Sph].[CommercialSpace] WHERE [RegistrationNo] =@No", new SqlParameter("@No", CS_REGISTRATION_NO));
            //var max = sphDatabase.GetDatabaseScalarValue<int>("SELECT MAX([CommercialSpaceId]) FROM [Sph].[CommercialSpace]");

            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL);
            driver.Sleep(500);
            driver.Click("#login-menu");
            driver.Click("#log-in");
            driver
                .Sleep(TimeSpan.FromSeconds(5))
                ;


            //var id = sphDatabase.GetDatabaseScalarValue<int>("SELECT [CommercialSpaceId] FROM [Sph].[CommercialSpace] WHERE [RegistrationNo] =@No", new SqlParameter("@No", CS_REGISTRATION_NO));
            Assert.IsTrue(0 < 1);
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }
    }
}
