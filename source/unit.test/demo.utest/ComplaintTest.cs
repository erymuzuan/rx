using System;
using System.Data.SqlClient;
using System.Xml.Linq;
using NUnit.Framework;
using Humanizer;

namespace web.test
{
    [TestFixture]
    public class ComplaintTest : BrowserTest
    {
        public const string COMPLAINT_TEMPLATE_NAME = "Aduan Kerosakan(UJIAN)";
        public const string COMPLAINT_CATEGORY = "Elektrikal";
        public const string CS_NAME = "NO 23 Jalan 3, Presint 8";
        private TestUser m_proaduan;

        [SetUp]
        public void Init()
        {
            m_proaduan = new TestUser
            {
                UserName = "proaduan",
                Password = "122ewew323",
                FullName = "Ruzzaima",
                Department = "",
                Designation = "",
                Email = "proaduan@hotmail.com",
                Roles = new[] { "can_assign_complaint", "can_edit_complaint_template" },
                StartModule = "complaint.dashboard",
                Telephone = "03-7291822"
            };
            this.AddUser(m_proaduan);
        }

        [Test]
        public void ComplaintFromPublicFlowTest()
        {
            _001_AddComplaintTemplate();
            _002_PublicComplaint();
            _003_AssignComplaintToDepartment();
        }



        [Test]
        // ReSharper disable InconsistentNaming
        public void _001_AddComplaintTemplate()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[ComplaintTemplate] WHERE Name = @Name", new SqlParameter("@Name", COMPLAINT_TEMPLATE_NAME));
            var max =
                this.GetDatabaseScalarValue<int>("SELECT MAX([ComplaintTemplateId]) FROM [Sph].[ComplaintTemplate]");
            var driver = this.InitiateDriver();
            driver.Login(m_proaduan);
            driver.NavigateToUrl("/#/complaint.template.list", 1.Seconds());
            driver.NavigateToUrl("#/template.complaint-id.0/0", 3.Seconds());

            // add elements
            driver.Value("[name=Complaint-template-category]", COMPLAINT_CATEGORY)
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
            driver.Click(".add-subcategory-button");

            // add sub category for first category
            driver.ClickFirst("input", e => e.GetAttribute("data-bind") == "click: addSubCategory");
            driver.Value(".input-subcategory", "Lampu");

            driver.ClickFirst("input", e => e.GetAttribute("data-bind") == "click: addSubCategory")
                .Value(".input-subcategory", "Kipas", 1);
            driver.ClickFirst("input", e => e.GetAttribute("data-bind") == "click: addSubCategory")
                .Value(".input-subcategory", "Penghawa dingin", 2);
            driver.ClickFirst("input", e => e.GetAttribute("data-bind") == "click: addSubCategory")
                .Value(".input-subcategory", "Litar pintas/trip", 3);

            driver.ClickFirst("input", e => e.GetAttribute("data-bind") == "click: saveSubCategoryCommand").Sleep(1.Seconds());

            //2nd category
            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addComplaintCategory")
                .Value(".input-category-name", "Perpaipan", 1)
                .Value(".input-category-description", "Kerosakan Perpaipan", 1)
                .Click(".add-subcategory-button", 1);

            // add sub category for 2nd category
            driver.ClickFirst("input", e => e.GetAttribute("data-bind") == "click: addSubCategory")
                .Value(".input-subcategory", "Saluran tersumbat");
            driver.ClickFirst("input", e => e.GetAttribute("data-bind") == "click: addSubCategory")
                .Value(".input-subcategory", "Paip bocor", 1);
            driver.ClickFirst("input", e => e.GetAttribute("data-bind") == "click: addSubCategory")
                .Value(".input-subcategory", "Pili/pam rosak", 2);
            driver.ClickFirst("input", e => e.GetAttribute("data-bind") == "click: addSubCategory")
                .Value(".input-subcategory", "Air kotor", 3);

            driver.ClickFirst("input", e => e.GetAttribute("data-bind") == "click: saveSubCategoryCommand").Sleep(200.Milliseconds());

            driver.Click("#save-button");

            driver.Sleep(3.Seconds());


            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([ComplaintTemplateId]) FROM [Sph].[ComplaintTemplate]");
            Assert.IsTrue(max < latest);

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#/complaint.template.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }

        [Test]
        public void _002_PublicComplaint()
        {
            var templateId = this.GetDatabaseScalarValue<int>("SELECT [ComplaintTemplateId] FROM [Sph].[ComplaintTemplate] WHERE [Name] = @Name", new SqlParameter("@Name", COMPLAINT_TEMPLATE_NAME));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([ComplaintId]) FROM [Sph].[Complaint]");
            var driver = this.InitiateDriver();
            driver.NavigateToUrl("/Account/Logoff")
                  .NavigateToUrl("/#/complaint");
            driver.NavigateToUrl(string.Format("/#/complaint.form-templateid.{0}/{0}", templateId), 2.Seconds());

            driver.Value("[name='Title']", "Kerosakan Lampu di Precint 8")
            .Value("[name=Remarks]", "Lampu tidak menyala di jalan Precint 8")
            .Value("[name=CommercialSpace]", CS_NAME)
            .Value("[name=ContactName]", "Ahmad Said")
            .Value("[name=Email]", "ahmadsaid@hotmail.com")
            .SelectOption("[name=Category]", "Perpaipan")
            .SelectOption("[name=SubCategory]", "Air kotor");

            driver.Click("#save-button");

            driver.Sleep(3.Seconds());


            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([ComplaintId]) FROM [Sph].[Complaint]");
            Assert.IsTrue(max < latest);
            var xml = XElement.Parse(this.GetDatabaseScalarValue<string>("SELECT [Data] FROM [Sph].[Complaint] WHERE [ComplaintId] =@ComplaintId", new SqlParameter("@ComplaintId", latest)));
            Assert.AreEqual(templateId, xml.GetAtrributeInt32Value("TemplateId"));
            Assert.AreEqual(COMPLAINT_TEMPLATE_NAME, xml.GetAttributeStringValue("Type"));
            Assert.AreEqual(CS_NAME, xml.GetAttributeStringValue("CommercialSpace"));

            driver.Sleep(5.Seconds(), "See the result");
            driver.Quit();
        }


        [Test]
        public void _003_AssignComplaintToDepartment()
        {

            var complaintId = this.GetDatabaseScalarValue<int>("SELECT MAX([ComplaintId]) FROM [Sph].[Complaint] WHERE [Status]='Baru'");
            var xml = XElement.Parse(this.GetDatabaseScalarValue<string>("SELECT [Data] FROM [Sph].[Complaint] WHERE [ComplaintId] =@ComplaintId", new SqlParameter("@ComplaintId", complaintId)));
            var firstStatus = this.GetDatabaseScalarValue<string>("SELECT [Status] FROM [Sph].[Complaint] WHERE [ComplaintId]=@Id", new SqlParameter("@Id", complaintId));
            var templateid = xml.GetAtrributeInt32Value("TemplateId");

            var driver = this.InitiateDriver();
            driver.Login(m_proaduan)
                 .NavigateToUrl("/#/complaint.dashboard", 2.Seconds());
            driver.NavigateToUrl(string.Format("/#/complaint.assign-templateid.{0}/{1}", templateid, complaintId), 2.Seconds());

            driver.SelectOption("[name=department]", "Senggaraan")
                .ClickFirst("button", e => e.Text == "Simpan");

            driver.Sleep(TimeSpan.FromSeconds(3));


            var latestStatus = this.GetDatabaseScalarValue<string>("SELECT [Status] FROM [Sph].[Complaint] WHERE [ComplaintId] = @Id", new SqlParameter("@Id", complaintId));
            Assert.AreNotEqual(firstStatus, latestStatus);

            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }
    }
}
