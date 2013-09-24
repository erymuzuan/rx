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

        public void CreateTestBuilding(TestUser user, string buildingTemplateName, string buildingName)
        {
            var driver = this.InitiateDriver();
            driver.Login(user);

            //
            Console.WriteLine("Creating building templates...");
            TestHelper.CreateBuildingTemplate(driver, buildingTemplateName);
            driver.LogOff();

            //
            Console.WriteLine("Creating test building...");
            var templateId = this.GetDatabaseScalarValue<int>("SELECT [BuildingTemplateId] FROM [Sph].[BuildingTemplate] WHERE [Name] = @Name",
                new SqlParameter("@Name", buildingTemplateName));
            driver.Login(user);
            TestHelper.CreateBuilding(driver, templateId, buildingName);
            driver.LogOff();

            //
            Console.WriteLine("Creating building lots...");
            var buildingId = this.GetDatabaseScalarValue<int>("SELECT [BuildingId] FROM [Sph].[Building] WHERE [Name] = @Name",
                new SqlParameter("@Name", buildingName));
            driver.Login(user);
            TestHelper.CreateBuildingLots(driver, templateId, buildingId);

            driver.Quit();

        }
    }
}
