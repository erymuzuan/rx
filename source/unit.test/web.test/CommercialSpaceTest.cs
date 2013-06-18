using System;
using System.Data.SqlClient;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Linq;

namespace web.test
{
    [TestFixture]
    public class CommercialSpaceTest : BrowserTest
    {
      public const string CS_REGISTRATION_NO = "BSPK/999999";

        [Test]
        public void _001_Add()
        {
            const string sphDatabase = "sph";
            sphDatabase.ExecuteNonQuery("DELETE FROM [Sph].[CommercialSpace] WHERE [RegistrationNo] =@No", new SqlParameter("@No", CS_REGISTRATION_NO));
            var max = sphDatabase.GetDatabaseScalarValue<int>("SELECT MAX([CommercialSpaceId]) FROM [Sph].[CommercialSpace]");

            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + "/#/commercialspacedetail/0/-/0");
            driver.Sleep(500);

            driver
                .Value("[name='commercialSpace.RegistrationNo']", CS_REGISTRATION_NO)
                .Click("#select-lot-button")
                .SelectOption("[name='selectedBuilding']", "Blok Damansara Intan")
                .Sleep(TimeSpan.FromSeconds(3))
                .SelectOption("[name='selectedFloor']", "GF")
                .Sleep(TimeSpan.FromSeconds(3))
                .SelectOption("[name='selectedLots']", "Lot 2")
                .Click("#add-lot-button")
                .Value("[name='commercialSpace.Size']", "2000")
                .Value("[name='commercialSpace.RentalRate']", "2500")
                .SelectOption("[id='commercialSpace.RentalType']", "Monthly")
                .SelectOption("[id='commercialSpace.Category']", "Cafeteria")
                .Click("[name='commercialSpace.IsOnline']")
                .Click("[name='commercialSpace.IsAvailable']")
                .Click("#save-button")
                .Sleep(TimeSpan.FromSeconds(5))
                ;


            var id = sphDatabase.GetDatabaseScalarValue<int>("SELECT [CommercialSpaceId] FROM [Sph].[CommercialSpace] WHERE [RegistrationNo] =@No", new SqlParameter("@No", CS_REGISTRATION_NO));
            Assert.IsTrue(max < id);
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }
    }
}
