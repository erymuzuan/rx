using System;
using System.Data.SqlClient;
using FluentDateTime;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace web.test
{
    [TestFixture]
    public class ReportBuilderTestFixture : BrowserTest
    {
        public const string SPH_DATABASE = "sph";
        public const string REPORT_TITLE = "Senarai Laporan Tanah(UJIAN)";

        [Test]
        public void _001_AddReportDefinition()
        {
            SPH_DATABASE.ExecuteNonQuery("DELETE FROM [Sph].[ReportDefinition] WHERE [Title] =@Title", new SqlParameter("@Title", REPORT_TITLE));
            var max = SPH_DATABASE.GetDatabaseScalarValue<int>("SELECT MAX([ReportDefinitionId]) FROM [Sph].[ReportDefinition]");


            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + "/Account/Login");
            driver.Sleep(150)
                .Sleep(TimeSpan.FromSeconds(2))
                .Value("[name='UserName']", "admin")
                .Value("[name='Password']", "123456")
                .Click("[name='submit']")
                .Sleep(5.Seconds())
                .Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + "/#/reportdefinition.edit-id.0/0");
            
            driver
                .Sleep(5.Seconds())
                .ClickFirst("input[type=radio]",e => e.GetAttribute("name") =="layout")
                .Value("[name='Name']", REPORT_TITLE)
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
                //.Click("#save-button")
                .Sleep(5.Seconds())
                ;

            driver.Sleep(5.Seconds(), "See the result");
            driver.Quit();
        }

    }
}
