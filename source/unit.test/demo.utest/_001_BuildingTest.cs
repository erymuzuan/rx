using System;
using System.Data.SqlClient;
using NUnit.Framework;
using FluentDate;

namespace web.test
{
    [TestFixture]
// ReSharper disable InconsistentNaming
// ReSharper disable once InconsistentNaming
    public class _100_BuildingTest : BrowserTest
    {
        public const string BUILDING_NAME = "Asrama Siswi Malaya (UJIAN)";
        public const string BUILDING_TEMPLATE_NAME = "Bangunan";


        private TestUser m_buildingAdmin;

        [SetUp]
        public void Init()
        {
            m_buildingAdmin = new TestUser
            {
                UserName = "buildingadmin",
                FullName = "Building Admin",
                Email = "buildingadmin@bespoke.com.my",
                Department = "Unit Hartanah",
                Designation = "Pegawai Hartanah",
                Password = "123456",
                StartModule = "building.list",
                Roles = new[] { "can_add_building", "can_edit_building_template" }
            };
            this.AddUser(m_buildingAdmin);
        }

        [Test]
        public void AddBuildingAndNavigateToLots()
        {
            _001_AddBuildingTemplate();
            _002_AddBuilding();
        }

        [Test]
        // ReSharper disable  InconsistentNaming
        public void _001_AddBuildingTemplate()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[BuildingTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", BUILDING_TEMPLATE_NAME));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([BuildingTemplateId]) FROM [Sph].[BuildingTemplate]");


            var driver = this.InitiateDriver();
            driver.Login(m_buildingAdmin);
            driver.NavigateToUrl("/#building.template.list", 2.Seconds())
                  .NavigateToUrl("/#/template.building-id.0/0", 4.Seconds());

            // add elements
            driver.Value("[name=Building-template-category]", BUILDING_TEMPLATE_NAME)
                  .Sleep(1.Seconds())
                  .Value("[name=Building-template-name]", BUILDING_TEMPLATE_NAME)
                  .Sleep(1.Seconds())
                  .Click("[id=template-isactive]")
                  .Value("[id=form-design-name]", BUILDING_TEMPLATE_NAME)
                  .Sleep(1.Seconds())
                  .Value("[id=form-design-description]", BUILDING_TEMPLATE_NAME)
                  .Sleep(1.Seconds())
                  ;

            //nama bangunan
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Nama bangunan")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "Name")
                  .Sleep(1.Seconds())
                  .SelectOption("[name=Size]", "XL")
                  .Sleep(1.Seconds());


            //no Rujukan
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "No. Rujukan")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "BuildingRegistrationNo")
                  .Sleep(1.Seconds())
                  .SelectOption("[name=Size]", "XL")
                  .Sleep(1.Seconds());

            //building description
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Paragrapah text")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Penerangan")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "Note")
                  .Sleep(1.Seconds())
                  .Value("[name=Tooltip]", "Maklumat terperinci berkenaan bangunan")
                  .Sleep(1.Seconds())
                  .SelectOption("[name=Size]", "XL")
                  .Sleep(1.Seconds()); 
            
            //Jenis Bangunan
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Select list")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Jenis Bangunan")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "BuildingType")
                  .Sleep(1.Seconds())
                  .SelectOption("[name=Size]", "XL")
                  .Sleep(1.Seconds());

            //isikan Jenis bangunan
            driver
                  .Value(".input-combobox-value", "[Sila Pilih]")
                  .Sleep(1.Seconds())
                  .Value(".input-combobox-caption", "[Sila Pilih]")
                  .Sleep(1.Seconds())
                  .Value(".input-combobox-value", "Bangunan Tinggi",1)
                  .Sleep(1.Seconds())
                  .Value(".input-combobox-caption", "Bangunan Tinggi", 1)
                  .Sleep(1.Seconds())
                  .Value(".input-combobox-value", "Apartment",2)
                  .Sleep(1.Seconds())
                  .Value(".input-combobox-caption", "Apartment", 2)
                  .Sleep(1.Seconds());

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: $root.addComboBoxOption");
            driver.Value(".input-combobox-value", "Asrama", 3)
                  .Value(".input-combobox-caption", "Asrama", 3)
                  .Sleep(1.Seconds());
            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: $root.addComboBoxOption");
            driver.Value(".input-combobox-value", "Semi-D", 4)
                  .Value(".input-combobox-caption", "Semi-D", 4)
                  .Sleep(1.Seconds());
            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: $root.addComboBoxOption");
            driver.Value(".input-combobox-value", "Bangalow", 5)
                  .Value(".input-combobox-caption", "Bangalow", 5)
                  .Sleep(1.Seconds());

            // address
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Address")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Alamat")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "Address");

            // show map button
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Show map button")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Peta Lokasi")
                  .Sleep(1.Seconds())
                  .Value("[name=CssClass]", "btn btn-success")
                  .Sleep(1.Seconds())
                  .Value("[name=Icon]", "icon-globe")
                  .Sleep(1.Seconds());

            //Section
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Section")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Spesifikasi")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "Section")
                  .Sleep(1.Seconds());

            // Size
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Nombor")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Saiz")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "BuildingSize")
                  .Sleep(1.Seconds())
                  .SelectOption("[name=Size]", "XL")
                  .Sleep(1.Seconds());

            //Tarikh Dibina - Custom Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Tarikh")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Tarikh Dibina")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "BuiltDateTime")
                  .Sleep(1.Seconds())
                  .SelectOption("[name=Size]", "XL")
                  .Sleep(1.Seconds());

            //Tarikh Diduduki - Custom Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Tarikh")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Tarikh Diduduki")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "RegisteredDateTime")
                  .Sleep(1.Seconds())
                  .SelectOption("[name=Size]", "XL")
                  .Sleep(1.Seconds()); 
            
            //Jumlah Blok
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Nombor")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Jumlah Blok")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "Blocks")
                  .Sleep(1.Seconds())
                  .SelectOption("[name=Size]", "XL")
                  .Sleep(1.Seconds());

            // blocks table
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Blocks Table")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Senarai Blok")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "BlockCollection")
                  .Sleep(1.Seconds());

            //Section
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Section")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Lain-lain Maklumat")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "Section")
                  .Sleep(1.Seconds());

            //Email - Emel Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Emel")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Email")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "Email")
                  .Sleep(1.Seconds())
                  .SelectOption("[name=Size]", "XL")
                  .Sleep(1.Seconds());

            //Website - Website Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Website")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Website")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "Website")
                  .Sleep(1.Seconds())
                  .SelectOption("[name=Size]", "XL")
                  .Sleep(1.Seconds()); 
        
            
            // custom list collection
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds());
            driver.ClickFirst("a", e => e.Text == "List")
                  .Sleep(1.Seconds());
            driver.ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds());
            driver.Value("[name=Label]", "Senarai Pegawai Bertugas")
                  .Sleep(1.Seconds());
                  driver.Value("[name=Path]", "Pegawai Bertugas")
                        .Sleep(1.Seconds());

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                .Sleep(1.Seconds())
                .Value("input[type=text].custom-list-name", "Nama Penuh")
                .Sleep(1.Seconds())
                .SelectOption("select.custom-list-type", typeof(string).GetShortAssemblyQualifiedName(), 0, false)
                .Sleep(1.Seconds());

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                  .Sleep(1.Seconds());
            driver.Value("input[type=text].custom-list-name", "Jawatan", 1)
                  .Sleep(1.Seconds());
            driver.SelectOption("select.custom-list-type", typeof(string).GetShortAssemblyQualifiedName(), 1, false)
                  .Sleep(1.Seconds());

            //HTML - HTML Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "HTML")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=html-text]", "Sila pastikan semua maklumat telah diisi sebelum simpan maklumat bangunan.")
                  .Sleep(1.Seconds());

            driver.Click("#save-button")
                .Sleep(5.Seconds());


            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([BuildingTemplateId]) FROM [Sph].[BuildingTemplate]");
            Assert.IsTrue(max < latest);

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#building.template.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }

        [Test]
        public void _002_AddBuilding()
        {

            this.ExecuteNonQuery("DELETE FROM [Sph].[Building] WHERE [Name] =@Name", new SqlParameter("@Name", BUILDING_NAME));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([BuildingId]) FROM [Sph].[Building]");
            var templateId = this.GetDatabaseScalarValue<int>("SELECT [BuildingTemplateId] FROM [Sph].[BuildingTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", BUILDING_TEMPLATE_NAME));


            var driver = this.InitiateDriver();
            driver.Login(m_buildingAdmin);
            driver.NavigateToUrl("/#/building.list", 2.Seconds());

            driver.NavigateToUrl(String.Format("/#/building.detail-templateid.{0}/{0}/0", templateId), 6.Seconds());


            driver
                .Value("[name='Name']", BUILDING_NAME)
                .Sleep(1.Seconds())
                .Value("[name='BuildingRegistrationNo']", "BSPK20130925001")
                .Sleep(1.Seconds())
                .Value("[name='Note']", "Bangunan yang menyediakan kemudahan penginapan bagi pelajar Universiti Malaya")
                .SelectOption("[name='BuildingType']", "Asrama")
                .Sleep(1.Seconds())
                .Value("[name='address.Street']", "Federal Territory of Kuala Lumpur")
                .Sleep(1.Seconds())
                .Value("[name='address.City']", "Kuala Lumpur")
                .Sleep(1.Seconds())
                .Value("[name='address.Postcode']", "50603")
                .Sleep(1.Seconds())
                .SelectOption("[name='address.State']", "Wilayah Persekutuan Kuala Lumpur")
                .Sleep(120.Seconds(), "SET MAP MANUALLY!!")
                .Sleep(5.Seconds())
                .Value("[name='BuildingSize']", "112991.02")
                .Sleep(1.Seconds())
                .Value("[name='BuiltDateTime']", "14/03/1980")
                .Sleep(1.Seconds())
                .Value("[name='RegisteredDateTime']", "14/03/1980")
                .Sleep(1.Seconds())
                .Value("[name='Blocks']", "5");

            driver.Click("[name='add-block-button']")
                .Sleep(200.Milliseconds(), "Add Block")
                .Value(".input-block-name", "1")
                .Sleep(1.Seconds())
                .Value(".input-block-description", "Blok A")
                .Sleep(1.Seconds())
                .Value(".input-block-size", "10000")
                .Sleep(1.Seconds())
                .Value(".input-block-floors", "1")
                .Sleep(1.Seconds());


            driver.Click("[name='ko_unique_6']")
                .Sleep(3.Seconds(), "Add Floor")
                .Value(".input-floor-no", "GF")
                .Sleep(1.Seconds())
                .Value(".input-floor-name", "Ground Floor")
                .Sleep(1.Seconds())
                .Value(".input-floor-size", "10000")
                .Sleep(1.Seconds())
                .Click("[name = 'okButton']");

          driver.Value("[name='Email']", "contractor@gmail.com")
                .Sleep(1.Seconds())
                .Value("[name='Website']", "www.cidb.com.my")
                .Sleep(1.Seconds());

            driver.Click("#save-button")
            .Sleep(TimeSpan.FromSeconds(2))
            ;
            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([BuildingId]) FROM [Sph].[Building]");
            Assert.IsTrue(max < latest);

            driver.NavigateToUrl("/#/building.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");

            driver.Quit();
        }

        [Test]
        public void _003_AddLots()
        {
            var id = this.GetDatabaseScalarValue<int>("SELECT [BuildingId] FROM [Sph].[Building] WHERE [Name] =@Name", new SqlParameter("@Name", BUILDING_NAME));
            var templateId = this.GetDatabaseScalarValue<int>("SELECT [BuildingTemplateId] FROM [Sph].[BuildingTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", BUILDING_TEMPLATE_NAME));

            var driver = this.InitiateDriver();
            driver.Login(m_buildingAdmin)
                .NavigateToUrl(String.Format("/#/building.detail-templateid.{0}/{0}/{1}", templateId, id),2.Seconds());

            driver
                .NavigateToUrl(String.Format("/#/lotdetail/{0}/1st Floor", id), 1.Seconds());


            driver.Click("#add-new-lot")
            .Value(".input-lot-name", "Lot 1")
            .Value(".input-lot-size", "2500")
            .Value(".input-lot-usage", "Cafeteria")
            .Sleep(200.Milliseconds());

            driver.Click("#add-new-lot");
            driver.Value(".input-lot-name", "Lot 2", 1);
            driver.Value(".input-lot-size", "2300", 1);
            driver.Value(".input-lot-usage", "Dobi", 1)
            .Sleep(200.Milliseconds());

            driver
            .Click("#add-new-lot")
            .Value(".input-lot-name", "Lot 3", 2)
            .Value(".input-lot-size", "500", 2)
            .Value(".input-lot-usage", "ATM", 2)
            .Sleep(TimeSpan.FromSeconds(2));


            driver.Click("#save-button");

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#/building.list");
            driver.NavigateToUrl(String.Format("/#/lotdetail/{0}/1st Floor", id));
            driver.Sleep(TimeSpan.FromSeconds(3), "See the result");

            driver.Quit();
        }
    }
}
