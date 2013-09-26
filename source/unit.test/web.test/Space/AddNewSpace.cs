using NDbUnit.Core.SqlClient;
using NUnit.Framework;

namespace web.test.Space
{
    [TestFixture]
    public class AddNewSpace : SpaceBaseTest
    {
        private TestUser m_spaceAdmin;
        private const string SPACE_REGISTRATION_NO = "BSPK/999999";
        
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
        public void Add()
        {
            CreateNewSpace(m_spaceAdmin, SPACE_REGISTRATION_NO);
        }

        [TearDown]
        public void Clear()
        {
            //this.DeleteSpace(SPACE_REGISTRATION_NO);
            this.DeleteUser(m_spaceAdmin);
        }
    }
}