using NUnit.Framework;

namespace web.test.Space
{
    [TestFixture]
    public class AddNewSpaceTest : SpaceBaseTest
    {
        private TestUser m_spaceAdmin;
        public const string SPACE_REGISTRATION_NO = "BSPK/999999";
        
        [SetUp]
        public void Init()
        {
            //create test user
            m_spaceAdmin = TestHelper.CreateSpaceAdmin();
            this.AddUser(m_spaceAdmin);

            //need some seed data
            this.CreateAddNewSpaceSeedData(m_spaceAdmin);
        }

        [Test]
        public void AddNew()
        {
            CreateNewSpace(m_spaceAdmin, SPACE_REGISTRATION_NO);
        }

        [TearDown]
        public void Clear()
        {
            //this.DeleteSpace(SPACE_REGISTRATION_NO);
            //this.DeleteUser(m_spaceAdmin);
        }
    }
}