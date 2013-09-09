using System;
using System.Data.SqlClient;
using NUnit.Framework;
using FluentDateTime;

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
        public void _003_PublicComplaint()
        {
            var templateId = this.GetDatabaseScalarValue<int>("SELECT [ComplaintTemplateId] FROM [Sph].[ComplaintTemplate] WHERE [Name] = @Name", new SqlParameter("@Name", MAINTENANCE_TEMPLATE_NAME));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([ComplaintId]) FROM [Sph].[Complaint]");
            var driver = this.InitiateDriver();
            driver.NavigateToUrl("/Account/Logoff")
                  .NavigateToUrl("/#/complaint");
            driver.NavigateToUrl(string.Format("/#/complaint.form-templateid.{0}/{0}", templateId), 2.Seconds());

            //  driver.Value("[name=CustomFieldValueCollection()[0].Value]", "Kerosakan Lampu di Precint 8");
            driver.Value("[name=Remarks]", "Lampu tidak menyala di jalan Precint 8");
            driver.Value("[name=CommercialSpace]", "Lampu tidak menyala di jalan Precint 8");
            //    driver.Value("[name=CustomFieldValueCollection()[1].Value]", "Ahmad Said");
            //   driver.Value("[name=CustomFieldValueCollection()[2].Value]", "ahmadsaid@hotmail.com");
            driver.SelectOption("[name=Category]", "Elektrik");
            driver.SelectOption("[name=SubCategory]", "Lampu");

            driver.Click("#save-button");

            driver.Sleep(TimeSpan.FromSeconds(3));


            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([ComplaintId]) FROM [Sph].[Complaint]");
            Assert.IsTrue(max < latest);

            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }
    }
}
