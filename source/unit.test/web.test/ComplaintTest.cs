using System;
using System.Data.SqlClient;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using FluentDateTime;

namespace web.test
{
    [TestFixture]
    public class ComplaintTest : BrowserTest
    {
        public const string sphDatabase = "sph";
        public const string tenantId = "4";
        [Test]
        public void _001_UserComplaint()
        {
            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + "/#/tenant.portal/" + tenantId);
            driver.Sleep(TimeSpan.FromSeconds(3));
            driver.Click("#complainttab");
            driver.Sleep(TimeSpan.FromSeconds(3));
            driver.SelectOption("[name='complaint.CommercialSpace']", "Blok Damansara Intan , GF , Lot 2,")
                .Sleep(3.Seconds())
                  .SelectOption("[name='complaint.Type']", "Kerosakan Dapur")
                  .Sleep(3.Seconds())
                  .SelectOption("[name='complaint.Category']", "Peralatan")
                  .Sleep(3.Seconds())
                  .SelectOption("[name='complaint.SubCategory']", null)
                  .Value("[name='complaint.remarks']", "alat elektrik tidak ok");
            driver.Sleep(TimeSpan.FromSeconds(3));
            driver.Click("#submit-button")
            .Sleep(TimeSpan.FromSeconds(3));
            var id = sphDatabase.GetDatabaseScalarValue<int>("SELECT MAX([ComplaintId]) FROM [Sph].[Complaint] WHERE [TenantId] =@id", new SqlParameter("@id", tenantId));
            Assert.IsTrue(id > 0);
            driver.Sleep(TimeSpan.FromSeconds(3), "See the result");
            driver.Quit();
        }
    }
}
