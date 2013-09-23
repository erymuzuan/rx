using NUnit.Framework;

namespace web.test.Space
{
    [TestFixture]
    public class AddNewSpace : SpaceBaseTest
    {
        private TestUser m_spaceAdmin;
        private const string SPACE_REGISTRATION_NO = "BSPK/999999";
        private const string SPACE_TEMPLATE_NAME = "Cafeteria Template"; 
        public const string BUILDING_NAME = "Bangunan Komersil Di Putrajaya (UJIAN)";
        public const string BUILDING_TEMPLATE_NAME = "Bangunan Komersil";

        [SetUp]
        public void Init()
        {
            //create test user
            m_spaceAdmin = TestHelper.CreateSpaceAdmin();
            this.AddUser(m_spaceAdmin);

            //need building to run test
            this.CreateTestBuilding(m_spaceAdmin, BUILDING_TEMPLATE_NAME, BUILDING_NAME);
        }

        [Test]
        public void Add()
        {
            CreateSpaceTemplate(m_spaceAdmin, SPACE_TEMPLATE_NAME);
            //CreateNewSpace();
        }

        

        //private void CreateNewSpace(string templateName)
        //{
        //    var templateId = this.GetDatabaseScalarValue<int>("SELECT [SpaceTemplateId] FROM [Sph].[SpaceTemplate] WHERE [Name] = @Name", new SqlParameter("@Name", templateName));

        //    var building = this.GetDatabaseScalarValue<int>("SELECT COUNT(*) FROM [Sph].[Building] WHERE [Name] = @Name",
        //            new SqlParameter("@Name", _100_BuildingTest.BUILDING_NAME));
        //    var permohonanId =
        //        this.GetDatabaseScalarValue<int>(
        //            "SELECT [ApplicationTemplateId] FROM [Sph].[ApplicationTemplate] WHERE [Name] = @Name",
        //            new SqlParameter("@Name", RentalApplicationTest.APP_TEMPLATE_NAME));

        //    Assert.AreEqual(1, building, "You'll need to run the AddBuildingTest");

        //    var driver = this.InitiateDriver();
        //    driver.Login(m_spaceAdmin);
        //    driver.NavigateToUrl("/#/space.list", 2.Seconds());
        //    driver.NavigateToUrl(String.Format("/#/space.detail-templateid.{0}/{0}/0/-/0", templateId), 3.Seconds());
        //    driver
        //        .Value("[name=RegistrationNo]", CS_REGISTRATION_NO)
        //        .Click("#select-lot-button")
        //        .Sleep(2.Seconds());

        //    driver.SelectOption("[name=selectedBuilding]", _100_BuildingTest.BUILDING_NAME)
        //        .Sleep(1.Seconds())
        //        .SelectOption("[name=selectedFloor]", "1st Floor")
        //        .Sleep(1.Seconds())
        //        .SelectOption("[name=selectedLots]", "Lot 1")
        //        .Click("#add-lot-button");

        //    driver.Value("[name='Cafe Name']", "Cafe ABC");
        //    driver.Value("[name='address.Street']", "Jalan Permata")
        //        .Value("[name='address.City']", "Putrajaya")
        //        .Value("[name='address.Postcode']", "62502")
        //        .Value("[name='address.State']", "Selangor");

        //    driver.ClickFirst("input", e => e.GetAttribute("data-bind") == "click : addCustomListItem('List')")
        //        .Sleep(2.Seconds());

        //    driver.Click("#add-feature-button");

        //    driver.Value(".input-feature-name", "Lot A-1-3")
        //          .Value(".input-feature-description", "Satu parking percuma")
        //          .Value(".input-feature-category", "Parking")
        //          .Click(".input-feature-isrequired")
        //        ;
        //    driver.Click("#add-feature-button");


        //    driver.Value(".input-feature-name", "Kabinet", 1)
        //          .Value(".input-feature-description", "Kabinet disediakan jika di apply", 1)
        //          .Value(".input-feature-category", "Perabot", 1)
        //          .Value(".input-feature-charge", "50", 1)
        //          .Value(".input-feature-available-quantity", "3", 1)
        //          .Value(".input-feature-occurence", "1", 1)
        //          .SelectOption(".input-feature-occurencetimespan", "Sekali", 1)
        //        ;

        //    driver.Click("#add-feature-button");
        //    driver.Value(".input-feature-name", "Kabinet Dapur Kayu", 2)
        //          .Value(".input-feature-description", "Kabinet disediakan jika di apply", 2)
        //          .Value(".input-feature-category", "Perabot", 2)
        //          .Value(".input-feature-charge", "50", 2)
        //          .Value(".input-feature-available-quantity", "5", 2)
        //          .Value(".input-feature-occurence", "1", 2)
        //          .Value(".input-feature-occurencetimespan", "Sekali", 2)
        //        ;

        //    driver.Click("#add-feature-button");
        //    driver.Value(".input-feature-name", "Oven", 3)
        //          .Value(".input-feature-description", "Oven disediakan jika di apply", 3)
        //          .Value(".input-feature-category", "Perabot", 3)
        //          .Value(".input-feature-charge", "100", 3)
        //          .Value(".input-feature-available-quantity", "10", 3)
        //          .Value(".input-feature-occurence", "1", 3)
        //          .Value(".input-feature-occurencetimespan", "Tahun", 3)
        //        ;

        //    driver.Click("#add-feature-button");
        //    driver.Value(".input-feature-name", "Parking Berbayar", 4)
        //          .Value(".input-feature-description", "Parking tambahan disediakan jika anda ingin menambah jumlah parking", 4)
        //          .Value(".input-feature-category", "Parking", 4)
        //          .Value(".input-feature-charge", "100", 4)
        //          .Value(".input-feature-available-quantity", "2", 4)
        //          .Value(".input-feature-occurence", "6", 4)
        //          .Value(".input-feature-occurencetimespan", "Bulan", 4)
        //        ;
        //    driver.Click("#add-feature-button");
        //    driver.Click(".btn-remove-feature", 5);

        //    driver.Value("[name='RentalRate']", "2500")
        //    .Value("[name='ContactPerson']", "Mohd Razali")
        //    .Click("[name='IsOnline']")
        //    .Click("[name='IsAvailable']");

        //    driver
        //    .ClickFirst("input[type=checkbox]", e => e.GetAttribute("value") == permohonanId.ToString(CultureInfo.InvariantCulture) && e.GetAttribute("data-bind") == "checked: ApplicationTemplateOptions")
        //    .Click("#save-button")
        //    .Sleep(3.Seconds());


        //    var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([SpaceId]) FROM [Sph].[Space]");
        //    Assert.IsTrue(max < latest);

        //    driver.NavigateToUrl("/#/space.list", 5.Seconds())
        //        .AssertElementExist("td", e => e.Text == CS_REGISTRATION_NO, "We should get 1 space with ref " + CS_REGISTRATION_NO);

        //    driver.Sleep(5.Seconds(), "See the result").Quit();
        //}

        [TearDown]
        public void Clear()
        {

            this.DeleteSpace(SPACE_REGISTRATION_NO);
            this.DeleteSpaceTemplate(SPACE_TEMPLATE_NAME);
            this.DeleteBuilding(BUILDING_NAME);
            this.DeleteBuildingTemplate(BUILDING_TEMPLATE_NAME);
            this.DeleteUser(m_spaceAdmin);
        }
    }
}