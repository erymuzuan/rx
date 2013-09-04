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
            driver.Login()
                .NavigateToUrl("/#/reportdefinition.edit-id.0/0")
                .Sleep(2.Seconds())
                .ClickFirst("input[type=radio]", e => e.GetAttribute("name") == "layout")
                .Sleep(2.Seconds())
                .ClickFirst("button.btn", e => e.Text.Contains("Configuration"))
                .Sleep(1.Seconds())
                .Value("[name='Title']", REPORT_TITLE)
                .Click("a.btn-close-configuration-dialog")
                .ClickFirst("span", e => e.Text == "header")
                .ClickFirst("span", e => e.Text == "Label")
                .ClickFirst("button.btn-context-action", e => true)
                .Value("textarea.input-label-html", "=" + REPORT_TITLE + " di @Location")
                .ClickFirst("div[data-bind]", e => e.GetAttribute("class").Contains("selected-form-element"))
                .Click("#save-button")
                .Sleep(5.Seconds())
                ;

            driver.Sleep(5.Seconds(), "See the result");
            driver.Quit();
            var currentid = SPH_DATABASE.GetDatabaseScalarValue<int>("SELECT MAX([ReportDefinitionId]) FROM [Sph].[ReportDefinition]");

            Assert.AreEqual(max + 1, currentid);
        }

    }
}
