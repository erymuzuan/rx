using System;
using System.Data.SqlClient;
using System.Globalization;
using FluentDateTime;
using NUnit.Framework;

namespace web.test.Space
{
    public class SpaceBaseTest : BrowserTest
    {
        public void CreateNewSpace(TestUser user, string templateName, string spaceRegistrationKey)
        {
            var templateId = this.GetDatabaseScalarValue<int>("SELECT [SpaceTemplateId] FROM [Sph].[SpaceTemplate] WHERE [Name] =@Name",
                new SqlParameter("@Name", templateName));

            var building = this.GetDatabaseScalarValue<int>("SELECT COUNT(*) FROM [Sph].[Building] WHERE [Name] = @Name",
                    new SqlParameter("@Name", _100_BuildingTest.BUILDING_NAME));
            var permohonanId =
                this.GetDatabaseScalarValue<int>(
                    "SELECT [ApplicationTemplateId] FROM [Sph].[ApplicationTemplate] WHERE [Name] = @Name",
                    new SqlParameter("@Name", RentalApplicationTest.APP_TEMPLATE_NAME));

            Assert.AreEqual(1, building, "You'll need to run the AddBuildingTest");

            var driver = this.InitiateDriver();
            driver.Login(user);
            driver.NavigateToUrl("/#/space.list", 2.Seconds());
            driver.NavigateToUrl(String.Format("/#/space.detail-templateid.{0}/{0}/0/-/0", templateId), 3.Seconds());
            driver
                .Value("[name=RegistrationNo]", spaceRegistrationKey)
                .Click("#select-lot-button")
                .Sleep(2.Seconds());

            driver.SelectOption("[name=selectedBuilding]", _100_BuildingTest.BUILDING_NAME)
                .Sleep(1.Seconds())
                .SelectOption("[name=selectedFloor]", "1st Floor")
                .Sleep(1.Seconds())
                .SelectOption("[name=selectedLots]", "Lot 1")
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


            driver.Value(".input-feature-name", "Kabinet", 1)
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
            driver.Click(".btn-remove-feature", 5);

            driver.Value("[name='RentalRate']", "2500")
            .Value("[name='ContactPerson']", "Mohd Razali")
            .Click("[name='IsOnline']")
            .Click("[name='IsAvailable']");

            driver
            .ClickFirst("input[type=checkbox]", e => e.GetAttribute("value") == permohonanId.ToString(CultureInfo.InvariantCulture) && e.GetAttribute("data-bind") == "checked: ApplicationTemplateOptions")
            .Click("#save-button")
            .Sleep(3.Seconds());


            var count = this.GetDatabaseScalarValue<int>("SELECT COUNT([RegistrationNo]) FROM [Sph].[Space] WHERE [RegistrationNo] = @No",
                new SqlParameter("@No", spaceRegistrationKey));
            Assert.AreEqual(count, 1);

            driver.NavigateToUrl("/#/space.list", 5.Seconds())
                .AssertElementExist("td", e => e.Text == spaceRegistrationKey, "We should get 1 space with ref " + spaceRegistrationKey);

            driver.Sleep(5.Seconds(), "See the result").Quit();

            Console.WriteLine("Space [ID:{0}] added", spaceRegistrationKey);
        }

        public void CreateSpaceTemplate(TestUser user, string templateName)
        {
            var driver = this.InitiateDriver();
            driver.Login(user);

            driver.NavigateToUrl("/#space.template.list", 2.Seconds())
                   .NavigateToUrl("/#/template.space-id.0/0", 3.Seconds());

            // add elements
            driver.Value("[name=Space-template-category]", templateName)
                  .Value("[name=Space-template-name]", templateName)
                  .Click("[id=template-isactive]")
                  .Value("[id=form-design-name]", templateName)
                  .Value("[id=form-design-description]", templateName);

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
                  .Value("[name=Path]", "Address");

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
            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
                  .Value(".custom-list-name", "Jenis", 1)
                  .SelectOption(".custom-list-type", "String", 1);

            //add 3rd list
            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: addCustomField")
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
                  .Value("[name=Path]", "ContactPerson");

            // contact
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Features")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Feature Collection")
                  .Value("[name=Path]", "FeatureCollection");

            // IsAvailable
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Checkboxes")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Permohonan Dibuka")
                  .Value("[name=Path]", "IsAvailable");

            // IsOnline
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Checkboxes")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Permohonan Online")
                  .Value("[name=Path]", "IsOnline");

            // HTML
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "HTML")
                  .Value("[name=html-text]", "Sila pastikan maklumat ruang lengkap sebelum klik butang SIMPAN");

            //save
            driver.Click("#save-button");
            driver.Sleep(5.Seconds());

            var count = this.GetDatabaseScalarValue<int>("SELECT COUNT([SpaceTemplateId]) FROM [Sph].[SpaceTemplate] WHERE [Name] = @Name", new SqlParameter("@Name", templateName));
            Assert.AreEqual(count, 1);

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#space.template.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }

        public void DeleteSpace(string spaceRegistrationNo)
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[Space] WHERE [RegistrationNo] = @No",
                new SqlParameter("@No", spaceRegistrationNo));
            Console.WriteLine("Space deleted.");
        }

        public void DeleteSpaceTemplate(string templateName)
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[SpaceTemplate] WHERE [Name] = @Name",
                new SqlParameter("@Name", templateName));
            Console.WriteLine("Space template deleted.");

        }

        public void DeleteBuildingTemplate(string templateName)
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[BuildingTemplate] WHERE [Name] = @Name",
                new SqlParameter("@Name", templateName));
            Console.WriteLine("Building template [{0}] successfully deleted.", templateName);
        }

        public void DeleteBuilding(string buildingName)
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[Building] WHERE [Name] = @Name",
                new SqlParameter("@Name", buildingName));
            Console.WriteLine("Building [{0}] successfully deleted.", buildingName);
        }

        public void CreateTestBuilding(TestUser user, string templateName, string buildingName)
        {
            CreateBuildingTemplate(user, templateName);
            CreateBuilding(user, templateName, buildingName);
        }

        private void CreateBuildingTemplate(TestUser user, string templateName)
        {
            var driver = this.InitiateDriver();
            driver.Login(user);
            driver.NavigateToUrl("/#building.template.list", 2.Seconds())
                  .NavigateToUrl("/#/template.building-id.0/0", 4.Seconds());

            // add elements
            driver.Value("[name=Building-template-category]", templateName)
                  .Value("[name=Building-template-name]", templateName)
                  .Click("[id=template-isactive]")
                  .Value("[id=form-design-name]", templateName)
                  .Value("[id=form-design-description]", templateName)
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
                  .Value(".input-combobox-value", "Indonesia", 1)
                  .Value(".input-combobox-caption", "Indonesia", 1)
                  .Value(".input-combobox-value", "Brunei", 2)
                  .Value(".input-combobox-caption", "Brunei", 2)
                  ;

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: $root.addComboBoxOption");
            driver.Value(".input-combobox-value", "Thailand", 3)
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
                  .Value("[name=Label]", "No. Lot")
                  .Value("[name=Path]", "LotNo");

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

            // floor collection
            driver.ClickFirst("a", e => e.Text == "Add a field")
                  .ClickFirst("a", e => e.Text == "Floors Table")
                  .ClickFirst("a", e => e.Text == "Fields settings")
                  .Value("[name=Label]", "Senarai Tingkat")
                  .Value("[name=Path]", "FloorCollection")
                  ;
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
                .SelectOption("select.custom-list-type", "Decimal", 4);

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

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#building.template.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();

            Console.WriteLine("Building template [{0}] successfully added", templateName);
        }

        private void CreateBuilding(TestUser user, string templateName, string buildingName)
        {
            var templateId = this.GetDatabaseScalarValue<int>("SELECT [BuildingTemplateId] FROM [Sph].[BuildingTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", templateName));
            var driver = this.InitiateDriver();
            driver.Login(user);
            driver.NavigateToUrl(String.Format("/#/building.detail-templateid.{0}/{0}/0", templateId), 5.Seconds());

            driver.Value("[name='ConsessionName']", "Putrajaya Holding");

            driver
                .Value("[name='Name']", buildingName)
                .Value("[name='LotNo']", "12-001")
                .Value("[name='Size']", "112991.02")
                .Value("[name='address.Street']", "Jalan Cempaka")
                .Value("[name='address.City']", "KB")
                .Value("[name='address.Postcode']", "15210")
                .SelectOption("[name='address.State']", "Kelantan")
                .Value("[name='Floors']", "2");

            driver.Click("[name='add-floor-button']")
            .Sleep(200.Milliseconds(), "Add floor")
            .Value(".input-floor-no", "G1")
            .Value(".input-floor-name", "1st Floor")
            .Value(".input-floor-size", "48500");

            driver.Click("[name='add-floor-button']")
            .Sleep(200.Milliseconds(), "Add floor")
            .Value(".input-floor-no", "G2", 1)
            .Value(".input-floor-name", "2nd Floor", 1)
            .Value(".input-floor-size", "48500", 1);

            driver.Click("#save-button")
            .Sleep(TimeSpan.FromSeconds(2))
            ;
           
            driver.NavigateToUrl("/#/building.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");

            driver.Quit();

            Console.WriteLine("Building [{0}] successfully added", buildingName);
        }
    }
}
