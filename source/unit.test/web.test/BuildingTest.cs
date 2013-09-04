using System;
using System.Data.SqlClient;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace web.test
{
    [TestFixture]
    public class BuildingTest : BrowserTest
    {
        const string SphDatabase = "sph";
        const string BuildingName = "Wisma Persekutuan Melaka (MITC)";
        public const string BUILDING_TEMPLATE_NAME = "Bangunan Komersil";


        [Test]
        public void AddBuildingAndNavigateToLots()
        {
            _002_AddBuilding();
            _003_AddLots();
        }

        [Test]
        public void _003_AddBuildingTemplate()
        {
            SphDatabase.ExecuteNonQuery("DELETE FROM [Sph].[BuildingTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", BUILDING_TEMPLATE_NAME));
            var max = SphDatabase.GetDatabaseScalarValue<int>("SELECT [BuildingTemplateId] FROM [Sph].[BuildingTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", BUILDING_TEMPLATE_NAME));


            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + "/Account/Login");
            driver.Login("ruzzaima");
            driver.NavigateToUrl("/#building.template.list");
            driver.NavigateToUrl("/#/template.building-id.0/0");
            driver.Sleep(TimeSpan.FromSeconds(5));


        }
        [Test]
        public void _002_AddBuilding()
        {

            SphDatabase.ExecuteNonQuery("DELETE FROM [Sph].[Building] WHERE [Name] =@Name", new SqlParameter("@Name", BuildingName));
            var max = SphDatabase.GetDatabaseScalarValue<int>("SELECT MAX([BuildingId]) FROM [Sph].[Building]");
            var templateId = SphDatabase.GetDatabaseScalarValue<int>("SELECT [BuildingTemplateId] FROM [Sph].[BuildingTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", BUILDING_TEMPLATE_NAME));


            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + "/Account/Login");
            driver.Login("ruzzaima");
            driver.NavigateToUrl("/#/building.list");
            driver.NavigateToUrl(String.Format("/#/building.detail-templateid.{0}/3/0", templateId));
            driver
                 .Sleep(TimeSpan.FromSeconds(5))
                 ;
            driver
                .Value("[name='Name']", BuildingName)
                .Value("[name='LotNo']", "12-001")
                .Value("[name='address.Street']", "Jalan Hang Tuah")
                .Value("[name='address.City']", "Melaka")
                .Value("[name='address.Postcode']", "75300")
                .SelectOption("[name='address.State']", "Kelantan")
                .Value("[name='CustomFieldValueCollection()[0].Value']", "01-08-1974")
                .Value("[name='CustomFieldValueCollection()[1].Value']", "01-11-1978")
                .Value("[name='CustomFieldValueCollection()[2].Value']", "01-02-1979")
                .Value("[name='CustomFieldValueCollection()[3].Value']", "01-12-1978")
                .Value("[name='Size']", "112991.02")
                .SelectOption("[name=' input-large']", "Baik")
                .Click("[name='add-floor-button']")
                .Sleep(TimeSpan.FromSeconds(2), "Add floor")
                .Value("[name='ko_unique_1']", "G1")
                .Value("[name='ko_unique_2']", "1st Floor")
                .Value("[name='ko_unique_3']", "48500")
                .Click("[name='add-floor-button']")
                .Sleep(TimeSpan.FromSeconds(2), "Add floor")
                .Value("[name='ko_unique_7']", "G2")
                .Value("[name='ko_unique_8']", "2nd Floor")
                .Value("[name='ko_unique_9']", "48500")
                .Click("#save-button")
                .Sleep(TimeSpan.FromSeconds(5))
                ;

            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }

        [Test]
        public void _003_AddLots()
        {
            var id = SphDatabase.GetDatabaseScalarValue<int>("SELECT [BuildingId] FROM [Sph].[Building] WHERE [Name] =@Name", new SqlParameter("@Name", BuildingName));
            const string templateId = "3";

            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL);
            driver.Sleep(150);
            driver.Click("#log-in")
                .Sleep(TimeSpan.FromSeconds(2))
                  .Value("[name='UserName']", "administrator")
                  .Value("[name='Password']", "123456")
                  .Click("[name='submit']");
            driver
                .Sleep(TimeSpan.FromSeconds(5))
                ;
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + String.Format("/#/building.detail-templateid.{0}/3/{1}", templateId, id));

            driver.Sleep(5);
            driver
                .Click("[name='ko_unique_5']")
                .Sleep(TimeSpan.FromSeconds(5))
                ;

            driver.Sleep(TimeSpan.FromSeconds(8), "See the result");
            driver.Quit();
        }
    }
}
