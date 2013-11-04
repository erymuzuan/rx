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

        private void CreateAddNewSpaceSeedData(TestUser spaceAdmin, string no= null)
        {
            throw new System.NotImplementedException();
        }

        [Test]
        public void AddNew()
        {
            CreateNewSpace(m_spaceAdmin, SPACE_REGISTRATION_NO,null);
        }

        [TearDown]
        public void Clear()
        {
            //this.DeleteSpace(SPACE_REGISTRATION_NO);
            //this.DeleteUser(m_spaceAdmin);
        }
    }
}