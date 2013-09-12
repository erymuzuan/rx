using System;
using System.Data.SqlClient;
using System.Globalization;
using FluentDateTime;
using NUnit.Framework;

namespace web.test
{
    [TestFixture]
    public class CommercialSpaceTest : BrowserTest
    {
        private TestUser m_spaceAdmin;
        public const string CS_REGISTRATION_NO = "BSPK/999999";
        public const string CS_TEMPLATE_NAME = "Cafeteria";

        [SetUp]
        public void Init()
        {
            m_spaceAdmin = new TestUser
            {
                UserName = "ruang-admin",
                FullName = "Ruang Admin",
                Email = "ruang.admin@bespoke.com.my",
                Department = "Test",
                Designation = "Boss",
                Password = "abcad12334535",
                Roles = new[] { "can_add_commercial_space", "can_edit_commercial_space", "can_edit_commercialspace_template" }
            };
            this.AddUser(m_spaceAdmin);
        }


        [Test]
        public void AddCsTemplateAndNewCs()
        {
            _001_AddCsTemplate();
            _002_AddNewCs();
        }

        [Test]
        // ReSharper disable InconsistentNaming
        public void _001_AddCsTemplate()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[CommercialSpaceTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", CS_TEMPLATE_NAME));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([CommercialSpaceTemplateId]) FROM [Sph].[CommercialSpaceTemplate]");

            var driver = this.InitiateDriver();
            driver.Login(m_spaceAdmin);

            driver.NavigateToUrl("/#commercialspace.template.list", 2.Seconds())
                   .NavigateToUrl("/#/template.commercialspace-id.0/0", 3.Seconds());

            // add elements
            driver.Value("[name=CommercialSpace-template-category]", CS_TEMPLATE_NAME)
                  .Value("[name=CommercialSpace-template-name]", CS_TEMPLATE_NAME)
                  .Click("[id=template-isactive]")
                  .Value("[id=form-design-name]", CS_TEMPLATE_NAME)
                  .Value("[id=form-design-description]", CS_TEMPLATE_NAME)
                  ;

            //No Daftar
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "No Daftar")
                  .Value("[name=Path]", "RegistrationNo");


            // Bangunan
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Building")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Nama Bangunan")
                  .Value("[name=Path]", "BuildingName");

            // No Lot
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Lot")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Lot")
                  .Value("[name=Path]", "LotName");

            // Nama Cafe
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Nama Cafe")
                  .Value("[name=Path]", "Cafe Name");

            // alamat
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Address")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Alamat")
                  .Value("[name=Path]", "Address")
                  ;
            // peralatan / perkakas yang tersedia
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "List")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "+ Peralatan / Perkakasan")
                  .Value("[name=Path]", "EquipmentCollection")
                  .ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                  .Value(".custom-list-name", "Nama")
                  .SelectOption(".custom-list-type", "String");
            //add 2nd list
            driver
                  .ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                  .Value(".custom-list-name", "Jenis",1)
                  .SelectOption(".custom-list-type", "String",1);
            //add 3rd list
            driver
                  .ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                  .Value(".custom-list-name", "Kuantiti",2)
                  .SelectOption(".custom-list-type", "Int",2);

            // sewa
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Sewa dicadangkan")
                  .Value("[name=Path]", "RentalRate");

            // contact
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Pegawai Untuk Dihubungi")
                  .Value("[name=Path]", "ContactPerson")
                  ;

            // IsAvailable
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Checkboxes")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Permohonan Dibuka")
                  .Value("[name=Path]", "IsAvailable")
                  ;

            // IsOnline
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Checkboxes")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Permohonan Online")
                  .Value("[name=Path]", "IsOnline")
                  ; 
            
            // HTML
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "HTML")
                  .Value("[name=html-text]", "Sila pastikan maklumat ruang lengkap sebelum klik butang SIMPAN..tq")
                  
                  ;

            driver.Click("#save-button");

            driver.Sleep(5.Seconds());


            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([CommercialSpaceTemplateId]) FROM [Sph].[CommercialSpaceTemplate]");
            Assert.IsTrue(max < latest);

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#commercialspace.template.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }

        [Test]
        public void _002_AddNewCs()
        {

            this.ExecuteNonQuery("DELETE FROM [Sph].[CommercialSpace] WHERE [RegistrationNo] =@No", new SqlParameter("@No", CS_REGISTRATION_NO));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([CommercialSpaceId]) FROM [Sph].[CommercialSpace]");
            var templateId = this.GetDatabaseScalarValue<int>("SELECT [CommercialSpaceTemplateId] FROM [Sph].[CommercialSpaceTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", CS_TEMPLATE_NAME));

            var building = this.GetDatabaseScalarValue<int>("SELECT COUNT(*) FROM [Sph].[Building] WHERE [Name] = @Name",
                    new SqlParameter("@Name", BuildingTest.BUILDING_NAME));
            var permohonanId =
                this.GetDatabaseScalarValue<int>(
                    "SELECT [ApplicationTemplateId] FROM [Sph].[ApplicationTemplate] WHERE [Name] = @Name",
                    new SqlParameter("@Name", RentalApplicationTest.APP_TEMPLATE_NAME));

            Assert.AreEqual(1, building, "You'll need to run the AddBuildingTest");

            var driver = this.InitiateDriver();
            driver.Login(m_spaceAdmin);
            driver.NavigateToUrl("/#/commercialspace", 2.Seconds());
            driver.NavigateToUrl(String.Format("/#/commercialspace.detail-templateid.{0}/{0}/0/-/0", templateId), 3.Seconds());
            driver
                .Value("[name='RegistrationNo']", CS_REGISTRATION_NO)
                .Click("#select-lot-button")
                .Sleep(2.Seconds());

            driver.SelectOption("[name='selectedBuilding']", BuildingTest.BUILDING_NAME)
                .Sleep(200.Milliseconds())
                .SelectOption("[name='selectedFloor']", "1st Floor")
                .Sleep(200.Milliseconds())
                .SelectOption("[name='selectedLots']", "Lot 1")
                .Click("#add-lot-button");

            driver.Value("[name='Cafe Name']", "Cafe ABC");
            driver.Value("[name='address.Street']", "Jalan Permata")
                .Value("[name='address.City']", "Putrajaya")
                .Value("[name='address.Postcode']", "62502")
                .Value("[name='address.State']", "Selangor")
                .Value("[name='RentalRate']", "2500")
                .Value("[name='ContactPerson']", "Mohd Razali")
                .Click("[name='IsOnline']")
                .Click("[name='IsAvailable']");

            driver
            .ClickFirst("input[type=checkbox]", e => e.GetAttribute("value") == permohonanId.ToString(CultureInfo.InvariantCulture) && e.GetAttribute("data-bind") == "checked: ApplicationTemplateOptions")
            .Click("#save-button")
            .Sleep(3.Seconds());


            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([CommercialSpaceId]) FROM [Sph].[CommercialSpace]");
            Assert.IsTrue(max < latest);

            driver.NavigateToUrl("/#/commercialspace");
            driver.Sleep(5.Seconds(), "See the result");
            driver.Quit();
        }
    }
}
