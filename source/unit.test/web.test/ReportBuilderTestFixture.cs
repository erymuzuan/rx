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
                .ClickFirst("input[type=radio]", e => e.GetAttribute("name") == "layout");


            // configuration - general
            driver
            .ClickFirst("button.btn", e => e.Text.Contains("Configuration"))
            .Sleep(200.Milliseconds())
            .Value("[name='Title']", REPORT_TITLE)
            .Value("[name='Category']", "Tanah")
            .Value("[name='Description']", "Senarai tanah mengikut lokasi")
            .Click("[name='IsActive']");

            // parameters
            driver
                .ClickFirst("a[data-toggle='tab']", e => e.Text == "Parameters")
                .Sleep(200.Milliseconds())
                .Click("button.btn-link", e => e.Text == "+ parameter")
                .Value("input.input-parameter-name","Location")
                .Value("input.input-parameter-label","Tapis ikut lokasi")
                .Value("select.input-parameter-type","String")
                ;

            driver
                .Sleep(200.Milliseconds())
                .Click("button.btn-link", e => e.Text == "+ parameter")
                .Value("input.input-parameter-name","Location2",1)
                .Value("input.input-parameter-label","Tapis ikut lokasi 2",1)
                .Value("select.input-parameter-type","String",1)
                .Sleep(1.Seconds())
                .Click("button.btn-link", e => e.Text == "Buang", 1)
                ;
            // data source
            driver
                .ClickFirst("a[data-toggle='tab']", e => e.Text == "Datasource")
                .Value("select[name='Entity']","Tanah")
                .Sleep(500.Milliseconds())
                .Click("a", e => e.Text == "+ Filter")
                .Value("select.input-datasource-fieldname","Location")
                .Value("select.input-datasource-operator","=")
                .Value("input.input-datasource-value","@Location")

                ;
            driver
                .Click("a", e => e.Text == "+ Filter")
                .Value("select.input-datasource-fieldname","LandId",1)
                .Value("select.input-datasource-operator","=",1)
                .Value("input.input-datasource-value", "1000",1)
                .Click("a", e => e.Displayed && e.Text == "Buang", 1)

                ;
            
            // column options
            driver
                .ClickFirst("a[data-toggle='tab']", e => e.Text == "Available Columns")
                .Click("label.checkbox", e => e.Text.Contains("Location"))
                .Click("label.checkbox", e => e.Text.Contains("LandId"))
                ;
            
            // column properties
            driver
                .ClickFirst("a[data-toggle='tab']", e => e.Text == "Columns Properties")
                .Value("select.input-col-group", "Group")
                .Value("select.input-col-group", "Count",1)
                ;

            driver
                .Sleep(5.Seconds(),"Check the dialog..")
                .Click("a.btn-close-configuration-dialog");/**/

            driver
            .ClickFirst("span", e => e.Text == "header") // select the header layour
            .ClickFirst("span", e => e.Text == "Label") // label element in toolboz
            .ClickFirst("button.btn-context-action", e => true)
            .Value("textarea.input-label-html", "=\"" + REPORT_TITLE + " di\"+ @Location")
            .ClickFirst("div[data-bind]", e => e.GetAttribute("class").Contains("selected-form-element"))
            ;

            driver
            .Click("div.report-layout", e => true,1) // select the content layout
            .ClickFirst("span", e => e.Text == "Table") // table element in toolbox
            .ClickFirst("button.btn-context-action", e => e.Displayed)

            .ClickFirst("button.btn-link",e => e.Text == "+ Column")
            .Value("select.input-datagrid-columnname","Location")
            .Value("input.input-datagrid-header","Location")

            .ClickFirst("button.btn-link",e => e.Text == "+ Column")
            .Value("select.input-datagrid-columnname","LandId_COUNT",1)
            .Value("input.input-datagrid-header","Bilangan",1)
            .Click("button.close",e => e.Displayed)
            .Sleep(1.Seconds())
            ;

            

            /**/
            driver.Click("#save-button")
                .Sleep(10.Seconds(), "See the result")
                .Quit();
            var currentid = SPH_DATABASE.GetDatabaseScalarValue<int>("SELECT MAX([ReportDefinitionId]) FROM [Sph].[ReportDefinition]");

            Assert.AreEqual(max + 1, currentid);
        }

    }
}
