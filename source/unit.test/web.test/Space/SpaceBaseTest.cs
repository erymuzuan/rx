using System;
using System.Data.SqlClient;
using FluentDateTime;
using NDbUnit.Core;
using NDbUnit.Core.SqlClient;
using NUnit.Framework;

namespace web.test.Space
{
    public class SpaceBaseTest : BrowserTest
    {
        public const string SPACE_TEMPLATE_NAME = "Cafeteria Template";
        public const string BUILDING_NAME = "Bangunan Komersil Di Putrajaya (UJIAN)";
        public const string BUILDING_TEMPLATE_NAME = "Bangunan Komersil";
        
        public void CreateNewSpace(TestUser user, string spaceRegistrationKey)
        {
            var spaceTemplateId = this.GetDatabaseScalarValue<int>("SELECT [SpaceTemplateId] FROM [Sph].[SpaceTemplate] WHERE [Name] =@Name",
                new SqlParameter("@Name", SPACE_TEMPLATE_NAME));

            var driver = this.InitiateDriver();
            driver.Login(user);

            driver.NavigateToUrl("/#/space.list", 2.Seconds());
            driver.NavigateToUrl(String.Format("/#/space.detail-templateid.{0}/{0}/0/-/0", spaceTemplateId), 2.Seconds());

            driver.Value("[name=RegistrationNo]", spaceRegistrationKey);

            driver.Click("#select-lot-button")
                .Sleep(1.Seconds())
                .SelectOption("[name=selectedBuilding]", BUILDING_NAME)
                .Sleep(1.Seconds())
                .SelectOption("[name=selectedFloor]", "G")
                .Sleep(1.Seconds())
                .SelectOption("[name=selectedUnits]", "Empty Lot")
                .Click("#add-lot-button");

            driver.Value("[name='SpaceName']", "Kafeteria Matyie");
            driver.Value("[name='address.Street']", "Jalan Permata Yang Hilang")
                .Value("[name='address.City']", "Kota Bharu")
                .Value("[name='address.Postcode']", "15100")
                .Value("[name='address.State']", "Kelantan");

            driver.Click("#add-feature-button");

            driver.Value(".input-feature-name", "Lot A-1-3")
                  .Value(".input-feature-description", "Satu parking percuma")
                  .Value(".input-feature-category", "Parking")
                  .Click(".input-feature-isrequired");

            driver.Click("#add-feature-button");


            driver.Value(".input-feature-name", "Kabinet", 1)
                  .Value(".input-feature-description", "Kabinet disediakan jika di apply", 1)
                  .Value(".input-feature-category", "Perabot", 1)
                  .Value(".input-feature-charge", "50", 1)
                  .Value(".input-feature-available-quantity", "3", 1)
                  .Value(".input-feature-occurence", "1", 1)
                  .SelectOption(".input-feature-occurencetimespan", "Sekali", 1);

            driver.Click("#add-feature-button");
            driver.Value(".input-feature-name", "Kabinet Dapur Kayu", 2)
                  .Value(".input-feature-description", "Kabinet disediakan jika di apply", 2)
                  .Value(".input-feature-category", "Perabot", 2)
                  .Value(".input-feature-charge", "50", 2)
                  .Value(".input-feature-available-quantity", "5", 2)
                  .Value(".input-feature-occurence", "1", 2)
                  .Value(".input-feature-occurencetimespan", "Sekali", 2);

            driver.Click("#add-feature-button");
            driver.Value(".input-feature-name", "Oven", 3)
                  .Value(".input-feature-description", "Oven disediakan jika di apply", 3)
                  .Value(".input-feature-category", "Perabot", 3)
                  .Value(".input-feature-charge", "100", 3)
                  .Value(".input-feature-available-quantity", "10", 3)
                  .Value(".input-feature-occurence", "1", 3)
                  .Value(".input-feature-occurencetimespan", "Tahun", 3);

            driver.Click("#add-feature-button");
            driver.Value(".input-feature-name", "Parking Berbayar", 4)
                  .Value(".input-feature-description", "Parking tambahan disediakan jika anda ingin menambah jumlah parking", 4)
                  .Value(".input-feature-category", "Parking", 4)
                  .Value(".input-feature-charge", "100", 4)
                  .Value(".input-feature-available-quantity", "2", 4)
                  .Value(".input-feature-occurence", "6", 4)
                  .Value(".input-feature-occurencetimespan", "Bulan", 4);
            driver.Click("#add-feature-button");
            driver.Click(".btn-remove-feature", 5);

            driver.Value("[name='RentalRate']", "2500")
            .Value("[name='ContactOfficer']", "Mohd Razali");

            driver
            .Click("[name='IsOnline']")
            .Click("[name='IsAvailable']");

            driver
            .Click("#save-button")
            .Sleep(1.Seconds());

            var count = this.GetDatabaseScalarValue<int>("SELECT COUNT([RegistrationNo]) FROM [Sph].[Space] WHERE [RegistrationNo] = @No",
                new SqlParameter("@No", spaceRegistrationKey));
            Assert.AreEqual(count, 1);

            driver
                .NavigateToUrl("/#/space.list", 2.Seconds())
                .Sleep(2.Seconds(), "See the result")
                .Quit();

            Console.WriteLine("Space [ID:{0}] added", spaceRegistrationKey);
        }

        public void DeleteSpace(string spaceRegistrationNo)
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[Space] WHERE [RegistrationNo] = @No",
                new SqlParameter("@No", spaceRegistrationNo));
            Console.WriteLine("Space deleted.");
        }

        public void CreateAddNewSpaceSeedData(TestUser user)
        {
            //
            Console.WriteLine("Load seed data..."); 
            var db = new SqlDbUnitTest(this.ConnectionString);

            db.ReadXmlSchema(@"..\..\Space\AddNewSpaceSchema.xsd");
            db.ReadXml(@"..\..\Space\AddNewSpaceData.xml");
            db.PerformDbOperation(DbOperationFlag.CleanInsertIdentity);
            this.ExecuteNonQuery("dbcc checkident ([Sph.BuildingTemplate], reseed, 0)");
            this.ExecuteNonQuery("dbcc checkident ([Sph.Building], reseed, 0)");
            this.ExecuteNonQuery("dbcc checkident ([Sph.SpaceTemplate], reseed, 0)");
            var buildingTemplateCount = this.GetDatabaseScalarValue<int>("SELECT COUNT([BuildingTemplateId]) FROM [Sph].[BuildingTemplate]");
            Assert.AreEqual(1, buildingTemplateCount);
            
            var buildingTemplateId = this.GetDatabaseScalarValue<int>("SELECT [BuildingTemplateId] FROM [Sph].[BuildingTemplate] WHERE [Name] = @Name",
                new SqlParameter("@Name", BUILDING_TEMPLATE_NAME));
            Console.WriteLine("Found building template ID:{0}", buildingTemplateId);
            Assert.IsTrue(buildingTemplateId > 0, "No building template found");

            var buildingId = this.GetDatabaseScalarValue<int>("SELECT [BuildingId] FROM [Sph].[Building] WHERE [Name] = @Name",
                new SqlParameter("@Name", BUILDING_NAME));
            Console.WriteLine("Found building ID:{0}", buildingId);
            Assert.IsTrue(buildingTemplateId > 0, "No building found");


            var spaceTemplateId = this.GetDatabaseScalarValue<int>("SELECT [SpaceTemplateId] FROM [Sph].[SpaceTemplate] WHERE [Name] = @Name",
                new SqlParameter("@Name", SPACE_TEMPLATE_NAME));
            Console.WriteLine("Found space template ID:{0}", spaceTemplateId);
            Assert.IsTrue(spaceTemplateId > 0, "No space template found");

        }
    }
}
