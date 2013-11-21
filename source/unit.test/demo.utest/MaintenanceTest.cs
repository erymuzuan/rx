using System;
using System.Data.SqlClient;
using NUnit.Framework;
using Humanizer;

namespace web.test
{
    [TestFixture]
    public class MaintenanceTest : BrowserTest
    {
        public const string MAINTENANCE_TEMPLATE_NAME = "Penyenggaraan Am";
       private TestUser m_prosenggara;

        [SetUp]
        public void Init()
        {
            m_prosenggara = new TestUser
            {
                UserName = "prosenggara",
                Password = "122ewew323",
                FullName = "Ruzzaima",
                Department = "Senggaraan",
                Designation = "Supervisor",
                Email = "prosenggara@hotmail.com",
                Roles = new[] { "maintenance_dashboard", "can_edit_maintenance_template" },
                StartModule = "maintenance.dashboard",
                Telephone = "03-7291822"
            };
            this.AddUser(m_prosenggara);
        }

        // ReSharper disable InconsistentNaming
        [Test]
        public void _001_AddMaintenanceTemplate()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[MaintenanceTemplate] WHERE Name = @Name", new SqlParameter("@Name", MAINTENANCE_TEMPLATE_NAME));
            var max =
                this.GetDatabaseScalarValue<int>("SELECT MAX([MaintenanceTemplateId]) FROM [Sph].[MaintenanceTemplate]");
            var driver = this.InitiateDriver();
            driver.Login(m_prosenggara);
            driver.NavigateToUrl("/#/maintenance.template.list", 1.Seconds());
            driver.NavigateToUrl("#/template.maintenance-id.0/0", 3.Seconds());

            // add elements
            driver.Value("[name=Maintenance-template-name]", MAINTENANCE_TEMPLATE_NAME)
                  .Click("[id=template-isactive]")
                  .Value("[id=form-design-name]", MAINTENANCE_TEMPLATE_NAME)
                  .Value("[id=form-design-description]", MAINTENANCE_TEMPLATE_NAME)
                  ;

            //Severity
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Select list")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Tahap")
                   .Value("[name=Path]", "Severity");

            // insert combobox items for severity
            driver
                .Value(".input-combobox-value", "Major")
                .Value(".input-combobox-caption", "Major")
                ;
            driver
                .Value(".input-combobox-value", "Minor",1)
                .Value(".input-combobox-caption", "Minor", 1)
                ;
            driver
                .Value(".input-combobox-value", "Show Stopper",2)
                .Value(".input-combobox-caption", "Show Stopper", 2)
                ;

            //Priority
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Select list")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Keutamaan")
                  .Value("[name=Path]", "Priority");

            // insert combobox items for priority
            driver
                .Value(".input-combobox-value", "Tinggi")
                .Value(".input-combobox-caption", "Tinggi")
                ;
            driver
                .Value(".input-combobox-value", "Medium", 1)
                .Value(".input-combobox-caption", "Medium", 1)
                ;
            driver
                .Value(".input-combobox-value", "Low", 2)
                .Value(".input-combobox-caption", "Low", 2)
                ;

            //Komen
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Paragrapah text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Komen")
                  .Value("[name=Path]", "Remarks")
                  .SelectOption("[name=Size]", "XXL");

            // lokasi
            driver.ClickFirst("a", e => e.Text == "Add a field")
                 .ClickFirst("a", e => e.Text == "Officers")
                 .ClickFirst("a", e => e.Text == "Fields settings")
                 .Value("[name=Label]", "Pegawai Bertugas")
                 .Click("a.search-path-button")
                 .Sleep(200.Milliseconds())
                 .ClickFirst("span", e => e.Text == "Officer")
                 .ClickFirst("input", e => e.GetAttribute("data-bind") == "click: selectPathFromPicker")
                 .Sleep(1.Seconds());

            //Komen
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Juruteknik")
                  .Value("[name=Path]", "Technician")
                  .SelectOption("[name=Size]", "L");

           driver.Click("#save-button");

            driver.Sleep(TimeSpan.FromSeconds(3));


            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([MaintenanceTemplateId]) FROM [Sph].[MaintenanceTemplate]");
            Assert.IsTrue(max < latest);

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#/maintenance.template.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }

        [Test]
        public void _002_AssignWorkorder()
        {
            var templateId = this.GetDatabaseScalarValue<int>("SELECT [MaintenanceTemplateId] FROM [Sph].[MaintenanceTemplate] WHERE [Name] = @Name", new SqlParameter("@Name", MAINTENANCE_TEMPLATE_NAME));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([MaintenanceId]) FROM [Sph].[Maintenance] WHERE [Status] = 'Pemeriksaan'");
            var driver = this.InitiateDriver();
            driver.NavigateToUrl("/Account/Logoff")
                .Login(m_prosenggara)
                  .NavigateToUrl("/#/maintenance.list/Pemeriksaan");
            driver.NavigateToUrl(string.Format("/#/maintenance.detail-templateid.{0}/{0}/{1}", templateId,max), 2.Seconds());

         

          //  driver.Click("#save-button");

            driver.Sleep(TimeSpan.FromSeconds(3));


            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([MaintenanceId]) FROM [Sph].[Maintenance]");
            Assert.IsTrue(max == latest);

            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }
    }
}
