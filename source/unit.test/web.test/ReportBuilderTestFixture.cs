using System;
using System.Data.SqlClient;
using System.Globalization;
using FluentDateTime;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

// ReSharper disable InconsistentNaming
namespace web.test
{
    [TestFixture]
    public class ReportBuilderTestFixture : BrowserTest
    {
        public const string SPH_DATABASE = "sph";
        public const string REPORT_TITLE = "Senarai Laporan Tanah(UJIAN)";

        private TestUser m_reportAdmin;

        [SetUp]
        public void Init()
        {
            m_reportAdmin = new TestUser
            {
                UserName = "reportadmin",
                FullName = "Report Admin",
                Email = "reportadmin@bespoke.com.my",
                Department = "Test",
                Designation = "Boss",
                Password = "abcad12334535",
                Roles = new[] { "admin_dashboard" }
            };
            this.AddUser(m_reportAdmin);
        }

        [Test]
        public void _001_AddReportDefinition()
        {
            SPH_DATABASE.ExecuteNonQuery("DELETE FROM [Sph].[ReportDefinition] WHERE [Title] =@Title", new SqlParameter("@Title", REPORT_TITLE));
            var max = SPH_DATABASE.GetDatabaseScalarValue<int>("SELECT MAX([ReportDefinitionId]) FROM [Sph].[ReportDefinition]");


            IWebDriver driver = new FirefoxDriver();
            driver.Login(m_reportAdmin)
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
                .Value("input.input-parameter-name", "Location")
                .Value("input.input-parameter-label", "Tapis ikut lokasi")
                .Value("select.input-parameter-type", "String")
                ;

            driver
                .Sleep(200.Milliseconds())
                .Click("button.btn-link", e => e.Text == "+ parameter")
                .Value("input.input-parameter-name", "Location2", 1)
                .Value("input.input-parameter-label", "Tapis ikut lokasi 2", 1)
                .Value("select.input-parameter-type", "String", 1)
                .Sleep(1.Seconds())
                .Click("button.btn-link", e => e.Text == "Buang", 1)
                ;
            // data source
            driver
                .ClickFirst("a[data-toggle='tab']", e => e.Text == "Datasource")
                .Value("select[name='Entity']", "Tanah")
                .Sleep(500.Milliseconds())
                .Click("a", e => e.Text == "+ Filter")
                .Value("select.input-datasource-fieldname", "Location")
                .Value("select.input-datasource-operator", "=")
                .Value("input.input-datasource-value", "@Location")

                ;
            driver
                .Click("a", e => e.Text == "+ Filter")
                .Value("select.input-datasource-fieldname", "LandId", 1)
                .Value("select.input-datasource-operator", "=", 1)
                .Value("input.input-datasource-value", "1000", 1)
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
                .Value("select.input-col-group", "Count", 1)
                ;

            driver
                .Sleep(5.Seconds(), "Check the dialog..")
                .Click("a.btn-close-configuration-dialog");/**/

            driver
            .ClickFirst("span", e => e.Text == "header") // select the header layour
            .ClickFirst("span", e => e.Text == "Label") // label element in toolboz
            .ClickFirst("button.btn-context-action", e => true)
            .Value("textarea.input-label-html", "=\"" + REPORT_TITLE + " di\"+ @Location")
            .ClickFirst("div[data-bind]", e => e.GetAttribute("class").Contains("selected-form-element"))
            ;

            driver
            .Click("div.report-layout", e => true, 1) // select the content layout
            .ClickFirst("span", e => e.Text == "Table") // table element in toolbox
            .ClickFirst("button.btn-context-action", e => e.Displayed)

            .ClickFirst("button.btn-link", e => e.Text == "+ Column")
            .Value("select.input-datagrid-columnname", "Location")
            .Value("input.input-datagrid-header", "Location")

            .ClickFirst("button.btn-link", e => e.Text == "+ Column")
            .Value("select.input-datagrid-columnname", "LandId_COUNT", 1)
            .Value("input.input-datagrid-header", "Bilangan", 1)
            .Click("button.close", e => e.Displayed)
            .Sleep(1.Seconds())
            ;



            /**/
            driver.Click("#save-button")
                .Sleep(10.Seconds(), "See the result")
                .Quit();
            var currentid = SPH_DATABASE.GetDatabaseScalarValue<int>("SELECT MAX([ReportDefinitionId]) FROM [Sph].[ReportDefinition]");

            Assert.IsTrue(currentid > max);
        }

        [Test]
        public void _002_PreviewReportDesign()
        {
            var location =
                SPH_DATABASE.GetDatabaseScalarValue<string>("SELECT TOP 1 [Location] FROM [Sph].[Land] ORDER BY NEWID()");
            var count =
                SPH_DATABASE.GetDatabaseScalarValue<int>("SELECT COUNT(*) FROM [Sph].[Land] WHERE [Location] = @Location", new SqlParameter("@Location", location));

            var driver = this.InitiateDriver();
            driver.Login(m_reportAdmin)
                .NavigateToUrl("/#/reportdefinition.list", 2.Seconds())
                .Click("a.execute-report", e => e.GetAttribute("title") == REPORT_TITLE, 2.Seconds())
                .Value("input#parameter0", location)
                .Click("button", e => e.GetAttribute("data-bind") == "click: executeCommand", 2.Seconds())
                .AssertElementExist("td", e => e.Text == count.ToString(CultureInfo.InvariantCulture));

            driver.Sleep(10.Seconds(), "See the output")
                .Quit();


        }
        [Test]
        public void _003_DeliverySchedule()
        {
            var id = SPH_DATABASE.GetDatabaseScalarValue<int>(
                    "SELECT [ReportDefinitionId] FROM [Sph].[ReportDefinition] WHERE [Title] =@Title",
                    new SqlParameter("@Title", REPORT_TITLE));
            SPH_DATABASE.ExecuteNonQuery("DELETE FROM [Sph].[ReportDelivery] WHERE [ReportDefinitionId] = @id", new SqlParameter("@id", id));



            var driver = this.InitiateDriver();
            driver.Login(m_reportAdmin)
                .NavigateToUrl("/#/reportdelivery.schedule/" + id, 1.Seconds())
                .Value("[name=Title]", "Schedule for " + REPORT_TITLE)
                .Value("[name=Users]", "admin")
                .Value("[name=Description]", "Description for schedule " + REPORT_TITLE)
                .Click("a", e => e.Text == "Add new Schedule", 500.Milliseconds())
                .ClickFirst("a", e => e.GetAttribute("data-bind") == "click: $root.startAddSchedule")
                .Sleep(500.Milliseconds())
                .Value("[name=StartHour]", "8")
                .Value("[name=EndHour]", "20")
                .Value("[name=Interval]", "4")
                .ClickAll("input.btn", e => e.GetAttribute("data-bind") == "click: $root.okDialog")
                .Sleep(2.Seconds())
                ;

            driver.Click("#save-button")
                .Sleep(2.Seconds());

            var sid = SPH_DATABASE.GetDatabaseScalarValue<int>("SELECT [ReportDeliveryId] FROM [Sph].[ReportDelivery] WHERE [ReportDefinitionId] = @id", new SqlParameter("@id", id));
            Assert.IsTrue(sid > 0);

            driver.Sleep(10.Seconds(), "See the output")
                .Quit();


        }
    }
}
