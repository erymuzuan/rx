using System;
using NUnit.Framework;

namespace web.test
{
    [TestFixture]
    public class ComplaintTenantTest : BrowserTest
    {
        public const string COMPLAINT_TEMPLATE_NAME = "Aduan Kerosakan";
        private TestUser m_tenant;

        [SetUp]
        public void Init()
        {
            m_tenant = new TestUser
            {
                UserName = "Y-89999",
                Password = "122ewew323",
                FullName = "YUNAZ SDN BHD",
                Department = "",
                Designation = "Penyewa",
                Email = "yunaz@hotmail.com",
                Roles = new[] { "can_view_tenant_details" },
                StartModule = "tenant.portal",
                Telephone = "03-729182222"
            };
            this.AddUser(m_tenant);
        }

        [Test]
// ReSharper disable InconsistentNaming
        public void _001_TenantComplaint()
        {
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([ComplaintId]) FROM [Sph].[Complaint]");
            var driver = this.InitiateDriver();
            
            driver.NavigateToUrl("/Account/Logoff")
                .Login(m_tenant);

            driver.Click("#complainttab");
            driver.SelectOption("[name=complaint.CommercialSpace]", "Kerosakan Lampu di Precint 8");
            driver.SelectOption("[name=complaint.Type]", "Aduan Kerosakan");
            driver.SelectOption("[name=complaint.Category]", "Elektrik");
            driver.SelectOption("[name=complaint.SubCategory]", "Lampu");
            driver.SelectOption("[name=complaint.remarks]", "Kerosakan Lampu di premis kami");

            driver.Click("#submit-button");

            driver.Sleep(TimeSpan.FromSeconds(3));


            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([ComplaintId]) FROM [Sph].[Complaint]");
            Assert.IsTrue(max < latest);

            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }
    }
}
