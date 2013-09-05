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
        public const string COMPLAINT_TEMPLATE_NAME = "Aduan Kerosakan";
        private TestUser m_complaintAdmin;

        [SetUp]
        public void Init()
        {
            m_complaintAdmin = new TestUser
            {
                UserName = "complaintadmin",
                Password = "122ewew323",
                FullName = "Ruzzaima",
                Department = "",
                Designation = "",
                Email = "cekalra@hotmail.com",
                Roles = new[] { "can_edit_complaint_template" },
                StartModule = "complaint.template.list",
                Telephone = "03-7291822"
            };
            this.AddUser(m_complaintAdmin);
        }

        [Test]
        public void _001_AddComplaintTemplate()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[ComplaintTemplate] WHERE Name = @Name",new SqlParameter("@Name",COMPLAINT_TEMPLATE_NAME));
            var max =
                this.GetDatabaseScalarValue<int>("SELECT MAX([ComplaintTemplateId]) FROM [Sph].[ComplaintTemplate]");
            var driver = this.InitiateDriver();
            driver.Login(m_complaintAdmin);
            driver.NavigateToUrl("/#/complaint.template.list", 1.Seconds());
            driver.NavigateToUrl("#/template.complaint-id.0/0", 1.Seconds());

            // add elements
            driver.Value("[name=Complaint-template-category]", COMPLAINT_TEMPLATE_NAME)
                  .Value("[name=Complaint-template-name]", COMPLAINT_TEMPLATE_NAME)
                  .Click("[id=template-isactive]")
                  .Value("[id=form-design-name]", COMPLAINT_TEMPLATE_NAME)
                  .Value("[id=form-design-description]", COMPLAINT_TEMPLATE_NAME)
                  ;

            //nama bangunan
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Tajuk")
                  .Value("[name=Path]", "Title");

            //keterangan
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Paragrapah text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Keterangan")
                  .Value("[name=Path]", "Remarks")
                  .SelectOption("[name=Size]", "XXL");

            // lokasi
            driver.ClickFirst("a", e => e.Text == "Add a field")
                 .ClickFirst("a", e => e.Text == "Single line text")
                 .ClickFirst("a", e => e.Text == "Fields settings")
                 .Value("[name=Label]", "Lokasi")
                 .Click("a.search-path-button")
                 .Sleep(200.Milliseconds())
                 .ClickFirst("span", e => e.Text == "CommercialSpace")
                 .ClickFirst("input", e => e.GetAttribute("data-bind") == "click: selectPathFromPicker")
                 .Sleep(1.Seconds());

            // nama
            driver.ClickFirst("a", e => e.Text == "Add a field")
                 .ClickFirst("a", e => e.Text == "Single line text")
                 .ClickFirst("a", e => e.Text == "Fields settings")
                 .Value("[name=Label]", "Nama Pengadu")
                 .Value("[name=Path]", "ContactName");

            // email
            driver.ClickFirst("a", e => e.Text == "Add a field")
                 .ClickFirst("a", e => e.Text == "Single line text")
                 .ClickFirst("a", e => e.Text == "Fields settings")
                 .Value("[name=Label]", "Email Pengadu")
                 .Value("[name=Path]", "Email");

            //kategori
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Category")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Kategori");

            //setting kategori
            driver.ClickFirst("a", e => e.Text == "Categories")
                  .Sleep(100.Milliseconds())
                  .ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addComplaintCategory");
            driver.Value(".input-category-name", "Elektrik");
            driver.Value(".input-category-description", "Kerosakan elektrik");

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addComplaintCategory");
            driver.Value(".input-category-name", "Perabot",1);
            driver.Value(".input-category-description", "Kerosakan Perabot",1);

            driver.Click("#save-button");

            driver.Sleep(TimeSpan.FromSeconds(3));


            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([ComplaintTemplateId]) FROM [Sph].[ComplaintTemplate]");
            Assert.IsTrue(max < latest);

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#complaint.template.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }

        [Test]
        public void _003_PublicComplaint()
        {
            var templateId = this.GetDatabaseScalarValue<int>("SELECT [ComplaintTemplateId] FROM [Sph].[ComplaintTemplate] WHERE [Name] = @Name", new SqlParameter("@Name", COMPLAINT_TEMPLATE_NAME));
            var driver = this.InitiateDriver();
            driver.NavigateToUrl("/Account/Logoff")
                  .NavigateToUrl("/#/complaint");
            driver.NavigateToUrl(string.Format("/#/complaint.form-templateid.{0}/{0}",templateId),200.Milliseconds());

            driver.Value("[name=CustomFieldValueCollection()[0].Value]", "Kerosakan Lampu di Precint 8")
                  .Value("[name=Remarks]", "Lampu tidak menyala di jalan Precint 8")
                  .Value("[name=CommercialSpace]", "Lampu tidak menyala di jalan Precint 8")
                  .Value("[name=CustomFieldValueCollection()[1].Value]", "Ahmad Said")
                  .Value("[name=CustomFieldValueCollection()[2].Value]", "ahmadsaid@hotmail.com")
                  .SelectOption("[name=Category]", "Elektrik")
                  .SelectOption("[name=SubCategory]", "Lampu")
                ;
        }
    }
}
