using System;
using System.Data.SqlClient;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace web.test
{
    [TestFixture]
    public class CommercialSpaceTest : BrowserTest
    {
        public const string CS_REGISTRATION_NO = "BSPK/999999";
        public const string CS_TEMPLATE_NAME = "Cafeteria";

        [Test]
        public void _001_AddCsTemplate()
        {
            const string sphDatabase = "sph";
            sphDatabase.ExecuteNonQuery("DELETE FROM [Sph].[CommercialSpaceTemplate] WHERE [RegistrationNo] =@No", new SqlParameter("@No", CS_REGISTRATION_NO));
            var max = sphDatabase.GetDatabaseScalarValue<int>("SELECT MAX([CommercialSpaceId]) FROM [Sph].[CommercialSpace]");
                
            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL);
            driver.Sleep(500);
            driver.Click("#login-menu").Sleep(TimeSpan.FromSeconds(2));
            driver.Click("#log-in")
                .Sleep(TimeSpan.FromSeconds(2))
                  .Value("[name='UserName']", "ruzzaima")
                  .Value("[name='Password']", "123456")
                  .Click("[name='submit']");
            driver
                .Sleep(TimeSpan.FromSeconds(5))
                ;
        

            var id = sphDatabase.GetDatabaseScalarValue<int>("SELECT [CommercialSpaceId] FROM [Sph].[CommercialSpace] WHERE [RegistrationNo] =@No", new SqlParameter("@No", CS_REGISTRATION_NO));
            Assert.IsTrue(max < id);
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }

        [Test]
        public void _001_Add()
        {
            const string sphDatabase = "sph";
            sphDatabase.ExecuteNonQuery("DELETE FROM [Sph].[CommercialSpace] WHERE [RegistrationNo] =@No", new SqlParameter("@No", CS_REGISTRATION_NO));
            var max = sphDatabase.GetDatabaseScalarValue<int>("SELECT MAX([CommercialSpaceId]) FROM [Sph].[CommercialSpace]");

            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL);
            driver.Sleep(500);
            driver.Click("#login-menu").Sleep(TimeSpan.FromSeconds(2));
            driver.Click("#log-in")
                .Sleep(TimeSpan.FromSeconds(2))
                  .Value("[name='UserName']", "ruzzaima")
                  .Value("[name='Password']", "123456")
                  .Click("[name='submit']");
            driver.Sleep(TimeSpan.FromSeconds(2))
                ;

            var templateId = sphDatabase.GetDatabaseScalarValue<int>("SELECT [CommercialSpaceTemplateId] FROM [Sph].[CommercialSpaceTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", CS_TEMPLATE_NAME));

            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + "/#/commercialspace");
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL +String.Format("/#/commercialspace.detail-templateid.{0}/1/0/-/0",templateId));

       

            //var id = sphDatabase.GetDatabaseScalarValue<int>("SELECT [CommercialSpaceId] FROM [Sph].[CommercialSpace] WHERE [RegistrationNo] =@No", new SqlParameter("@No", CS_REGISTRATION_NO));
            //Assert.IsTrue(max < id);
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }
    }
}
