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
            CreateNewSpace(m_spaceAdmin, SPACE_TEMPLATE_NAME, SPACE_REGISTRATION_NO);
        }

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