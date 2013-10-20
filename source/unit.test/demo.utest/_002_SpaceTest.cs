using System;
using System.Data.SqlClient;
using System.Globalization;
using FluentDateTime;
using NUnit.Framework;

namespace web.test
{
    [TestFixture]
    // ReSharper disable InconsistentNaming
    public class _200_SpaceTest : BrowserTest
    {
        private TestUser m_spaceAdmin;
        public const string CS_REGISTRATION_NO = "BSPK/01102013002";
        public const string CS_TEMPLATE_NAME = "Ruang Kedai";
        public const string BUILDING_NAME = "Asrama Siswi Malaya (UJIAN)";
        

        [SetUp]
        public void Init()
        {
            m_spaceAdmin = new TestUser
            {
                UserName = "ruang-admin",
                FullName = "Ruang Admin",
                Email = "ruang.admin@bespoke.com.my",
                Department = "Unit Hartanah",
                Designation = "Pegawai Hartanah",
                Password = "123456",
                StartModule = "space.list",
                Roles = new[] { "can_add_space", "can_edit_space", "can_edit_space_template" }
            };
            this.AddUser(m_spaceAdmin);
        }


        [Test]
        public void AddCsTemplateAndNewCs()
        {
            _001_SpaceTemplate();
            _002_AddNewSpace();
        }

        [Test]
        public void _001_SpaceTemplate()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[SpaceTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", CS_TEMPLATE_NAME));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([SpaceTemplateId]) FROM [Sph].[SpaceTemplate]");

            var driver = this.InitiateDriver();
            driver.Login(m_spaceAdmin);

            driver.NavigateToUrl("/#space.template.list", 2.Seconds())
                   .NavigateToUrl("/#/template.space-id.0/0", 3.Seconds());

            // add elements
            driver.Value("[name=Space-template-category]", CS_TEMPLATE_NAME)
                  .Sleep(1.Seconds())
                  .Value("[name=Space-template-name]", CS_TEMPLATE_NAME)
                  .Sleep(1.Seconds())
                  .Click("[id=template-isactive]")
                  .Value("[id=form-design-name]", CS_TEMPLATE_NAME)
                  .Sleep(1.Seconds())
                  .Value("[id=form-design-description]", CS_TEMPLATE_NAME)
                  ;

            //Kategori
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Select list")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Kategori")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "Category")
                  .Sleep(1.Seconds())
                  .SelectOption("[name=Size]", "XL")
                  .Sleep(1.Seconds())
                  ;
            //isikan Jenis Kategori Kedai
            driver
                  .Value(".input-combobox-value", "[Sila Pilih]")
                  .Value(".input-combobox-caption", "[Sila Pilih]")
                  .Value(".input-combobox-value", "Kedai Runcit", 1)
                  .Value(".input-combobox-caption", "Kedai Runcit", 1)
                  .Value(".input-combobox-value", "Kedai Dobi", 2)
                  .Value(".input-combobox-caption", "Kedai Dobi", 2)
                  .Sleep(1.Seconds())
                  ;

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: $root.addComboBoxOption");
            driver.Value(".input-combobox-value", "Kedai Gunting", 3)
                  .Value(".input-combobox-caption", "Kedai Gunting", 3);
            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: $root.addComboBoxOption");
            driver.Value(".input-combobox-value", "Kedai Buku", 4)
                  .Value(".input-combobox-caption", "Kedai Buku", 4);
            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: $root.addComboBoxOption");
            driver.Value(".input-combobox-value", "Kedai Kek", 5)
                  .Value(".input-combobox-caption", "Kedai Kek", 5);
            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: $root.addComboBoxOption");
            driver.Value(".input-combobox-value", "Kedai Jahit", 6)
                  .Value(".input-combobox-caption", "Kedai Jahit", 6);
            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: $root.addComboBoxOption");
            driver.Value(".input-combobox-value", "Kafeteria", 7)
                  .Value(".input-combobox-caption", "Kafeteria", 7)
                  .Sleep(1.Seconds());

            //No Daftar
            driver.ClickFirst("a", e => e.Text == "Add a field")
                .Sleep(1.Seconds())
                .ClickFirst("a", e => e.Text == "Single line text")
                .Sleep(1.Seconds())
                .ClickFirst("a", e => e.Text == "Fields settings")
                .Sleep(1.Seconds())
                .Value("[name=Label]", "No Daftar")
                .Sleep(1.Seconds())
                .Value("[name=Path]", "RegistrationNo")
                .Sleep(1.Seconds())
                .SelectOption("[name=Size]", "XL")
                .Sleep(1.Seconds());

            // Bangunan
            driver.ClickFirst("a", e => e.Text == "Add a field")
                .Sleep(1.Seconds())
                .ClickFirst("a", e => e.Text == "Building")
                .Sleep(1.Seconds())
                .ClickFirst("a", e => e.Text == "Fields settings")
                .Sleep(1.Seconds())
                .Value("[name=Label]", "Pilih Bangunan Sedia-ada")
                .Sleep(1.Seconds())
                .Value("[name=Path]", "BuildingId")
                .Sleep(1.Seconds())
                .SelectOption("[name=Size]", "XL")
                .Sleep(1.Seconds());

            //Nama Bangunan Jika tidak wujud dalam list sedia ada
            driver.ClickFirst("a", e => e.Text == "Add a field")
                .Sleep(1.Seconds())
                .ClickFirst("a", e => e.Text == "Single line text")
                .Sleep(1.Seconds())
                .ClickFirst("a", e => e.Text == "Fields settings")
                .Sleep(1.Seconds())
                .Value("[name=Label]", "Nama Bangunan")
                .Sleep(1.Seconds())
                .Value("[name=Path]", "BuildingName")
                .Sleep(1.Seconds())
                .Value("[name=Visible]", "BuildingId()===0")
                .Sleep(1.Seconds())
                .SelectOption("[name=Size]", "XL")
                .Sleep(1.Seconds());

            // alamat
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Address")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Alamat")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "Address")
                  .Sleep(1.Seconds());

            // show map button
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Map")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Peta Lokasi")
                  .Sleep(1.Seconds())
                  .Value("[name=CssClass]", "btn btn-success")
                  .Sleep(1.Seconds())
                  .Value("[name=Visible]", "SpaceId > 0")
                  .Sleep(1.Seconds())
                  .Value("[name=Icon]", "icon-globe")
                  .Sleep(1.Seconds());

            // sewa
            driver.ClickFirst("a", e => e.Text == "Add a field")
                .Sleep(1.Seconds())
                .ClickFirst("a", e => e.Text == "Single line text")
                .Sleep(1.Seconds())
                .ClickFirst("a", e => e.Text == "Fields settings")
                .Sleep(1.Seconds())
                .Value("[name=Label]", "Kadar Sewa (RM)")
                .Sleep(1.Seconds())
                .Value("[name=Path]", "RentalRate")
                .SelectOption("[name=Size]", "XL")
                .Sleep(1.Seconds());

            //Jenis Sewa
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Select list")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Jenis Sewa")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "RentalType")
                  .Sleep(1.Seconds())
                  .SelectOption("[name=Size]", "XL")
                  .Sleep(1.Seconds())
                  ;
            //isikan Jenis Kategori Sewa
            driver
                  .Value(".input-combobox-value", "[Sila Pilih]")
                  .Value(".input-combobox-caption", "[Sila Pilih]")
                  .Value(".input-combobox-value", "Bulanan", 1)
                  .Value(".input-combobox-caption", "Bulanan", 1)
                  .Value(".input-combobox-value", "Tahunan", 2)
                  .Value(".input-combobox-caption", "Tahunan", 2)
                  .Sleep(1.Seconds())
                  ;

            // contact
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Single line text")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Pegawai Untuk Dihubungi")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "ContactPerson")
                  .Sleep(1.Seconds())
                  .SelectOption("[name=Size]", "XL")
                  .Sleep(1.Seconds())
                  ;


            // peralatan / perkakas yang tersedia
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "List")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "+ Peralatan / Perkakasan")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "EquipmentCollection")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                  .Sleep(1.Seconds())
                  .Value(".custom-list-name", "Nama")
                  .Sleep(1.Seconds())
                  .SelectOption(".custom-list-type", "String")
                  .Sleep(1.Seconds());
            //add 2nd list
            driver
                  .ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                  .Value(".custom-list-name", "Jenis", 1)
                  .SelectOption(".custom-list-type", "String", 1);
            //add 3rd list
            driver
                  .ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                  .Value(".custom-list-name", "Kuantiti", 2)
                  .SelectOption(".custom-list-type", "Int", 2)
                  .Sleep(1.Seconds());

           
            // feature
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Features")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Feature Collection")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "FeatureCollection")
                  .Sleep(1.Seconds())
                  ;

            // HTML
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "HTML")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=html-text]", "Sila [/] pilihan di bawah jika anda ingin memasarkan ruang untuk disewa dan sila pastikan maklumat ruang lengkap sebelum klik butang SIMPAN")
                  .Sleep(3.Seconds(),"Click Bold color")
                  ;

            // IsAvailable
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Checkboxes")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Permohonan Dibuka")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "IsAvailable")
                  .Sleep(1.Seconds())
                  ;

            // IsOnline
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Checkboxes")
                  .Sleep(1.Seconds())
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Sleep(1.Seconds())
                  .Value("[name=Label]", "Permohonan Online")
                  .Sleep(1.Seconds())
                  .Value("[name=Path]", "IsOnline")
                  .Sleep(1.Seconds())
                  ;


            driver.Click("#save-button");

            driver.Sleep(5.Seconds());


            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([SpaceTemplateId]) FROM [Sph].[SpaceTemplate]");
            Assert.IsTrue(max < latest);

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#space.template.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }

        [Test]
        public void _002_AddNewSpace()
        {

            this.ExecuteNonQuery("DELETE FROM [Sph].[Space] WHERE [RegistrationNo] =@No", new SqlParameter("@No", CS_REGISTRATION_NO));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([SpaceId]) FROM [Sph].[Space]");
            var templateId = this.GetDatabaseScalarValue<int>("SELECT [SpaceTemplateId] FROM [Sph].[SpaceTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", CS_TEMPLATE_NAME));

            var building = this.GetDatabaseScalarValue<int>("SELECT COUNT(*) FROM [Sph].[Building] WHERE [Name] = @Name",
                    new SqlParameter("@Name", _100_BuildingTest.BUILDING_NAME));
            var permohonanId =
                this.GetDatabaseScalarValue<int>(
                    "SELECT [ApplicationTemplateId] FROM [Sph].[ApplicationTemplate] WHERE [Name] = @Name",
                    new SqlParameter("@Name", RentalApplicationTest.APP_TEMPLATE_NAME));

            Assert.AreEqual(1, building, "You'll need to run the AddBuildingTest");

            var driver = this.InitiateDriver();
            driver.Login(m_spaceAdmin);
            driver.NavigateToUrl("/#/space.list", 2.Seconds());
            driver.NavigateToUrl(String.Format("/#/space.detail-templateid.{0}/{0}/0/-/0", templateId), 3.Seconds());
            driver
                .SelectOption("[name=Category]", "Kedai Runcit")
                .Value("[name=RegistrationNo]", CS_REGISTRATION_NO)
                .SelectOption("[name=buildingName]", BUILDING_NAME)
                .Sleep(2.Seconds(),"Map supposed to be loaded and dumpt on screen");

            driver.Value("[name='RentalRate']", "2500")
                .SelectOption("[name='RentalType']", "Tahunan")
                .Value("[name='ContactPerson']", "Mohd Razali");
            
            driver.ClickFirst("input", e => e.GetAttribute("data-bind") == "click : addCustomListItem('List')")
                .Sleep(20.Seconds(),"Add information of that Custom List manually [Hos Api],[Kecemasan],[1]");

            driver.Click("#add-feature-button")
                  .Sleep(1.Seconds(), "Add Features");

            driver.Value(".input-feature-name", "Lot A-1-3")
                  .Value(".input-feature-description", "Satu parking percuma")
                  .Value(".input-feature-category", "Parking")
                  .Click(".input-feature-isrequired")
                ;
            driver.Click("#add-feature-button");


            driver.Value(".input-feature-name", "Kabinet",1)
                  .Value(".input-feature-description", "Kabinet disediakan jika pemohon membuat permohonan kepada pihak pengurusan", 1)
                  .Value(".input-feature-category", "Perabot", 1)
                  .Value(".input-feature-charge", "50", 1)
                  .Value(".input-feature-available-quantity", "3", 1)
                  .Value(".input-feature-occurence", "1", 1)
                  .SelectOption(".input-feature-occurencetimespan", "Sekali", 1)
                ;

            driver.Click("#add-feature-button");
            driver.Value(".input-feature-name", "Kabinet Dapur Kayu", 2)
                  .Value(".input-feature-description", "Kabinet disediakan jika di apply", 2)
                  .Value(".input-feature-category", "Perabot", 2)
                  .Value(".input-feature-charge", "50", 2)
                  .Value(".input-feature-available-quantity", "5", 2)
                  .Value(".input-feature-occurence", "1", 2)
                  .Value(".input-feature-occurencetimespan", "Sekali", 2)
                ;

            //driver.Click("#add-feature-button");
            //driver.Value(".input-feature-name", "Oven", 3)
            //      .Value(".input-feature-description", "Oven disediakan jika di apply", 3)
            //      .Value(".input-feature-category", "Perabot", 3)
            //      .Value(".input-feature-charge", "100", 3)
            //      .Value(".input-feature-available-quantity", "10", 3)
            //      .Value(".input-feature-occurence", "1", 3)
            //      .Value(".input-feature-occurencetimespan", "Tahun", 3)
            //    ;

            //driver.Click("#add-feature-button");
            //driver.Value(".input-feature-name", "Parking Berbayar", 4)
            //      .Value(".input-feature-description", "Parking tambahan disediakan jika anda ingin menambah jumlah parking", 4)
            //      .Value(".input-feature-category", "Parking", 4)
            //      .Value(".input-feature-charge", "100", 4)
            //      .Value(".input-feature-available-quantity", "2", 4)
            //      .Value(".input-feature-occurence", "6", 4)
            //      .Value(".input-feature-occurencetimespan", "Bulan", 4)
            //    ;
            //driver.Click("#add-feature-button");
            //driver.Click(".btn-remove-feature",5);
           
            
            driver.Click("[name='IsOnline']")
            .Click("[name='IsAvailable']");

            driver
            .ClickFirst("input[type=checkbox]", e => e.GetAttribute("value") == permohonanId.ToString(CultureInfo.InvariantCulture) && e.GetAttribute("data-bind") == "checked: ApplicationTemplateOptions")
            .Click("#save-button")
            .Sleep(3.Seconds());


            //var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([SpaceId]) FROM [Sph].[Space]");
            //Assert.IsTrue(max < latest);

            driver.NavigateToUrl("/#/space.list", 5.Seconds())
                .AssertElementExist("td", e => e.Text == CS_REGISTRATION_NO, "We should get 1 space with ref " + CS_REGISTRATION_NO);
            
            driver.Sleep(5.Seconds(), "See the result").Quit();
        }
    }
}
