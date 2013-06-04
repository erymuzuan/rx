using System;
using System.Data.SqlClient;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Linq;

namespace web.test
{
    [TestFixture]
    public class BuildingTest : BrowserTest
    {
        const string SphDatabase = "sph";
        const string BuildingName = "Damansara Utama Uptown Block A";
        [Test]
        public void AddBuilding()
        {
            const string sphDatabase = "sph";
            sphDatabase.ExecuteNonQuery("DELETE FROM [Sph].[Building] WHERE [Name] =@Name", new SqlParameter("@Name", BuildingName));
            var max = sphDatabase.GetDatabaseScalarValue<int>("SELECT MAX([BuildingId]) FROM [Sph].[Building]");

            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + "/#/buildingdetail/0");
            driver.Sleep(500);

            driver
                .Value("[name='building.Name']", BuildingName)
                .Value("[name='building.LotNo']", "12A")
                .Value("[name='building.Address.Street']", "No 2 Jalan SS 21/26")
                .Value("[name='building.Address.City']", "Petaling Jaya")
                .Value("[name='building.Address.Postcode']", "47400")
                .SelectOption("[name='building.Address.State']", "Selangor")
                .Value("[name='building.Size']", "48500")
                .Value("[name='building.Floors']", "3")
                .SelectOption("[name='building.Status']", "Good")
                .Value("[name='ko_unique_1']", "G")
                .Value("[name='ko_unique_2']", "Ground Floor")
                .Value("[name='ko_unique_3']", "48500")
                .Click("[name='add-floor-button']")
                .Sleep(TimeSpan.FromSeconds(2), "Add floor")
                .Value("[name='ko_unique_7']", "G1")
                .Value("[name='ko_unique_8']", "1st Floor")
                .Value("[name='ko_unique_9']", "48500")
                .Click("[name='add-floor-button']")
                .Sleep(TimeSpan.FromSeconds(2), "Add floor")
                .Value("[name='ko_unique_13']", "G2")
                .Value("[name='ko_unique_14']", "2nd Floor")
                .Value("[name='ko_unique_15']", "48500")
                .Click("#save-button")
                ;


            var id = sphDatabase.GetDatabaseScalarValue<int>("SELECT [BuildingId] FROM [Sph].[Building] WHERE [Name] =@Name", new SqlParameter("@Name", BuildingName));
            Assert.IsTrue(max < id);
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }

        [Test]
        public void AddLots()
        {
            var id = SphDatabase.GetDatabaseScalarValue<int>("SELECT [BuildingId] FROM [Sph].[Building] WHERE [Name] =@Name", new SqlParameter("@Name", BuildingName));

            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + "/#/lotdetail/" + id + "/Ground Floor");
            driver.Sleep(500);
            var usage = new[] {"Cafe", "Restaurant", "Books and Magazine", "Laundry", "Computers", "Clothing","Electronics"};
            for (int i = 0; i < 10; i++)
            {
                var lot = string.Format("[name='ko_unique_{0}']", (i * 4) + 1);
                var size = string.Format("[name='ko_unique_{0}']", (i * 4) + 2);
                var use = string.Format("[name='ko_unique_{0}']", (i * 4) + 3);
                driver
                      .Click("#add-new-lot")
                      .Sleep(250)
                      .Value(lot, "G-00" + i)
                      .Value(size, "1250")
                      .Value(use, usage.OrderBy(s => Guid.NewGuid()).First())
                    ;

            }

            driver.Sleep(TimeSpan.FromSeconds(8), "See the result");
            driver.Quit();
        }
    }
}
