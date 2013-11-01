using System;
using System.Data.SqlClient;
using FluentDate;
using NUnit.Framework;

namespace web.test
{
    [TestFixture]
// ReSharper disable InconsistentNaming
// ReSharper disable once InconsistentNaming
    public class _100_BuildingTest : BrowserTest
    {
        public const string BUILDING_NAME = "Bangunan Komersil Di Putrajaya (UJIAN)";
        public const string BUILDING_TEMPLATE_NAME = "Bangunan Komersil";


        private TestUser m_buildingAdmin;

        [SetUp]
        public void Init()
        {
            m_buildingAdmin = new TestUser
            {
                UserName = "buildingadmin",
                FullName = "Building Admin",
                Email = "buildingadmin@bespoke.com.my",
                Department = "Test",
                Designation = "Boss",
                Password = "abcad12334535",
                StartModule = "building.list",
                Roles = new[] { "can_add_space", "can_edit_building_template", "can_add_building" }
            };
            this.AddUser(m_buildingAdmin);
        }

        [Test]
        public void AddBuildingAndNavigateToLots()
        {
            _001_AddBuildingTemplate();
            _002_AddBuilding();
            _003_AddLots();
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
                  .Value("[name=Building-template-name]", BUILDING_TEMPLATE_NAME)
                  .Click("[id=template-isactive]")
                  .Value("[id=form-design-name]", BUILDING_TEMPLATE_NAME)
                  .Value("[id=form-design-description]", BUILDING_TEMPLATE_NAME)
                  ;

            //nama bangunan
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Nama bangunan")
                  .Value("[name=Path]", "Name");

            //descripsi bangunan - custom field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Paragrapah text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Deskripsi")
                  .Value("[name=Path]", "Descripsi") 
                  .Value("[name=Tooltip]", "Maklumat terperinci berkenaan bangunan")
                  .SelectOption("[name=Size]", "XL"); 
            
            //IsOversea bangunan - custom field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Checkboxes")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Bangunan Di Luar Negara")
                  .Value("[name=Path]", "IsOversea") 
                  .Value("[name=Tooltip]", "Bangunan di luar atau dalam negara"); 
            
            //Nama negara - jika bangunan di luar negara
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Select list")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Negara")
                  .Value("[name=Path]", "Country")
                  .Value("[name=Visible]", "CustomField('IsOversea')")
                  ;
            //isikan nama negara
            driver
                  .Value(".input-combobox-value", "Singapura")
                  .Value(".input-combobox-caption", "Singapura") 
                  .Value(".input-combobox-value", "Indonesia",1)
                  .Value(".input-combobox-caption", "Indonesia", 1)
                  .Value(".input-combobox-value", "Brunei",2)
                  .Value(".input-combobox-caption", "Brunei", 2)
                  ;

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: $root.addComboBoxOption");
            driver.Value(".input-combobox-value", "Thailand",3)
                  .Value(".input-combobox-caption", "Thailand", 3)
                  ;

            //Tarikh Dibina - Custom Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Tarikh")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Tarikh Dibina")
                  .Value("[name=Path]", "DevelopmentDate"); 
            
            //Jumlah Kontraktor - Nombor Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Nombor")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Jumlah Kontraktor")
                  .Value("[name=Path]", "TotalContractor"); 
            
            //Email - Emel Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Emel")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Email")
                  .Value("[name=Path]", "Email");

            //Website - Website Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Website")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Website")
                  .Value("[name=Path]", "Website"); 
        

            //owner - Custom Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Nama Konsesi")
                  .Value("[name=Path]", "ConsessionName");

            // Lot NO
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "No. Unit")
                  .Value("[name=Path]", "UnitNo");

            // Size
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Saiz")
                  .Value("[name=Path]", "Size");

            // address
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Address")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Alamat")
                  .Value("[name=Path]", "Address");

            // show map button
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Show map button")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Tunjuk Peta Kawasan")
                  .Value("[name=CssClass]", "btn btn-success")
                  .Value("[name=Icon]", "icon-globe")
                  ;

            // Bilangan Tingkat
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Bilangan Tingkat")
                  .Value("[name=Path]", "Floors");

        
            // blocks table
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Blocks Table")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Senarai Blok")
                  .Value("[name=Path]", "BlockCollection")
                  ;
            // custom list collection
            driver.ClickFirst("a", e => e.Text == "Add a field");
            driver.ClickFirst("a", e => e.Text == "List");
            driver.ClickFirst("a", e => e.Text == "Fields settings");
            driver.Value("[name=Label]", "Senarai Pegawai Bertugas");
                  driver.Value("[name=Path]", "Pegawai Bertugas");

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                .Value("input[type=text].custom-list-name", "Nama Penuh")
                .SelectOption("select.custom-list-type", typeof(string).GetShortAssemblyQualifiedName(), 0, false);

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField");
            driver.Value("input[type=text].custom-list-name", "Umur", 1);
                driver.SelectOption("select.custom-list-type", "Int", 1);

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                .Value("input[type=text].custom-list-name", "Dob", 2)
                .SelectOption("select.custom-list-type", "DateTime", 2);

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                .Value("input[type=text].custom-list-name", "Tetap", 3)
                .SelectOption("select.custom-list-type", "Bool", 3);

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                .Value("input[type=text].custom-list-name", "Gaji", 4)
                .SelectOption("select.custom-list-type","Decimal", 4);

            //HTML - HTML Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "HTML")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=html-text]", "Sila isi semua maklumat sebelom simpan maklumat bangunan");

            //List - List Field
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "HTML")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=html-text]", "Sila isi semua maklumat sebelom simpan maklumat bangunan");

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
                .Value("[name='Descripsi']", "Bangunan konsesi putrajaya holding mempunyai 1 blok")
                .Click("[name='IsOversea']")
                .Sleep(1.Seconds())
                .Click("[name='IsOversea']")
                .Sleep(1.Seconds())
                .Value("[name='DevelopmentDate']", "14/03/1980")
                .Value("[name='TotalContractor']", "2")
                .Value("[name='Email']", "contractor@gmail.com")
                .Value("[name='Website']", "www.cidb.com.my")
                .Value("[name=ConsessionName]", "Putrajaya Holding")
                .Value("[name='UnitNo']", "12-001")
                .Value("[name='Size']", "112991.02")
                .Value("[name='address.Street']", "Jalan Cempaka")
                .Value("[name='address.City']", "KB")
                .Value("[name='address.Postcode']", "15210")
                .SelectOption("[name='address.State']", "Kelantan");

        
            //Blok A
            driver.Click("[name=add-block-button]")
                  .Sleep(200.Milliseconds(), "Add Block")
                ;
            
            driver.Value(".input-block-name", "A")
                .Value(".input-block-description", "Blok A")
                .Value(".input-block-size", "54000")
                .Value(".input-block-floors", "8")
                ;

            //Floors for blok A
            driver.Click(".button-block-floor")
                .Sleep(200.Milliseconds())
                .Click("[name=add-floor-button]")
                .Value(".input-floor-no","GF")
                .Value(".input-floor-name","Ground Floor")
                .Value(".input-floor-size","54000")
                .Click("[name=add-floor-button]")
                .Value(".input-floor-no", "1ST",1)
                .Value(".input-floor-name", "First Floor",1)
                .Value(".input-floor-size", "54000",1)
                .Click("[name=add-floor-button]")
                .Value(".input-floor-no", "2ND",2)
                .Value(".input-floor-name", "Second Floor",2)
                .Value(".input-floor-size", "54000",2)
                .Click("input", e => e.GetAttribute("data-bind") == "click: okClick")
                .Sleep(2.Seconds())
                ;

            //Blok B
            driver.Click("[name=add-block-button]")
                 .Sleep(200.Milliseconds(), "Add Block")
               ;
            
            driver.Value(".input-block-name", "B",1)
                .Value(".input-block-description", "Blok B",1)
                .Value(".input-block-size", "54000",1)
                .Value(".input-block-floors", "8",1)
                ;
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
