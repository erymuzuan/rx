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
                  .Value("[name=Space-template-name]", CS_TEMPLATE_NAME)
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
                  .ClickFirst("a", e => e.Text == "Unit")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Unit")
                  .Value("[name=Path]", "BuildingUnit");

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
                  .Value(".custom-list-name", "Jenis", 1)
                  .SelectOption(".custom-list-type", "String", 1);
            //add 3rd list
            driver
                  .ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                  .Value(".custom-list-name", "Kuantiti", 2)
                  .SelectOption(".custom-list-type", "Int", 2);

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

            // contact
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Features")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Feature Collection")
                  .Value("[name=Path]", "FeatureCollection")
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
                .Value("[name=RegistrationNo]", CS_REGISTRATION_NO)
                .Click("#select-lot-button")
                .Sleep(2.Seconds());

            driver.SelectOption("[name=selectedBuilding]", _100_BuildingTest.BUILDING_NAME)
                .Sleep(1.Seconds())
                .SelectOption("[name=selectedFloor]", "1st Floor")
                .Sleep(1.Seconds())
                .SelectOption("[name=selectedUnits]", "Lot 1")
                .Click("#add-lot-button");

            driver.Value("[name='Cafe Name']", "Cafe ABC");
            driver.Value("[name='address.Street']", "Jalan Permata")
                .Value("[name='address.City']", "Putrajaya")
                .Value("[name='address.Postcode']", "62502")
                .Value("[name='address.State']", "Selangor");

            driver.ClickFirst("input", e => e.GetAttribute("data-bind") == "click : addCustomListItem('List')")
                .Sleep(2.Seconds());

            driver.Click("#add-feature-button");

            driver.Value(".input-feature-name", "Lot A-1-3")
                  .Value(".input-feature-description", "Satu parking percuma")
                  .Value(".input-feature-category", "Parking")
                  .Click(".input-feature-isrequired")
                ;
            driver.Click("#add-feature-button");


            driver.Value(".input-feature-name", "Kabinet",1)
                  .Value(".input-feature-description", "Kabinet disediakan jika di apply", 1)
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

            driver.Click("#add-feature-button");
            driver.Value(".input-feature-name", "Oven", 3)
                  .Value(".input-feature-description", "Oven disediakan jika di apply", 3)
                  .Value(".input-feature-category", "Perabot", 3)
                  .Value(".input-feature-charge", "100", 3)
                  .Value(".input-feature-available-quantity", "10", 3)
                  .Value(".input-feature-occurence", "1", 3)
                  .Value(".input-feature-occurencetimespan", "Tahun", 3)
                ;

            driver.Click("#add-feature-button");
            driver.Value(".input-feature-name", "Parking Berbayar", 4)
                  .Value(".input-feature-description", "Parking tambahan disediakan jika anda ingin menambah jumlah parking", 4)
                  .Value(".input-feature-category", "Parking", 4)
                  .Value(".input-feature-charge", "100", 4)
                  .Value(".input-feature-available-quantity", "2", 4)
                  .Value(".input-feature-occurence", "6", 4)
                  .Value(".input-feature-occurencetimespan", "Bulan", 4)
                ;
            driver.Click("#add-feature-button");
            driver.Click(".btn-remove-feature",5);
           
            driver.Value("[name='RentalRate']", "2500")
            .Value("[name='ContactPerson']", "Mohd Razali")
            .Click("[name='IsOnline']")
            .Click("[name='IsAvailable']");

            driver
            .ClickFirst("input[type=checkbox]", e => e.GetAttribute("value") == permohonanId.ToString(CultureInfo.InvariantCulture) && e.GetAttribute("data-bind") == "checked: ApplicationTemplateOptions")
            .Click("#save-button")
            .Sleep(3.Seconds());


            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([SpaceId]) FROM [Sph].[Space]");
            Assert.IsTrue(max < latest);

            driver.NavigateToUrl("/#/space.list",5.Seconds())
                .AssertElementExist("td", e => e.Text == CS_REGISTRATION_NO, "We should get 1 space with ref " + CS_REGISTRATION_NO);
            
            driver.Sleep(5.Seconds(), "See the result").Quit();
        }
    }
}
