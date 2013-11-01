using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Xml.Linq;
using FluentDate;
using NUnit.Framework;

namespace web.test
{
    [TestFixture]
    public class RebateTest : BrowserTest
    {
        public const string COMPLAINT_TEMPLATE_NAME = "Aduan Kerosakan";
        private TestUser m_cashier;

        [SetUp]
        public void Init()
        {
            m_cashier = new TestUser
            {
                UserName = "cashier",
                Password = "2324323",
                FullName = "Cashier1",
                Department = "Kewangan",
                Designation = "Juruwang",
                Email = "cashier@hotmail.com",
                Roles = new[] { "can_edit_payment" },
                StartModule = "payment",
                Telephone = "03-80008000"
            };
            this.AddUser(m_cashier);
        }

        // ReSharper disable InconsistentNaming
        [Test]
        public void _001_AddRebate()
        {
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([RebateId]) FROM [Sph].[Rebate]");
            var contractNo = this.GetDatabaseScalarValue<string>("SELECT MAX([ReferenceNo]) FROM [Sph].[Contract]");

            //seed data for contract
            var xml = XElement.Load(@".\rebate.xml");
            var sql = string.Format("INSERT INTO [Sph].[Contract]([ReferenceNo],[Title],[Status],[TenantName],[TenantIdSsmNo]" +
                        ",[TenantRegistrationNo],[StartDate],[EndDate],[SpaceId],[SpaceRegistrationNo],[Data]," +
                                    "[CreatedDate],[CreatedBy],[ChangedDate],[ChangedBy])  " +
                        "VALUES('BSPB/2013/2013000001','Kontrak sewaan WAN HUDA BIN WAN ALI dan Bahagian Pengurusan Hartanah','Active','WAN HUDA BIN WAN ALI','800212-02-9651','2013000001'" +
                                    ",'2013-09-27 00:00:00','2014-09-27 00:00:00','28','BSPK/999999',@Data" +
                                    ",'2013-09-18 16:45:00','test','2013-09-18 16:45:00','test')");
            this.ExecuteNonQuery(sql, new SqlParameter("@Data", xml.ToString()));

            var driver = this.InitiateDriver();
            driver.NavigateToUrl("/Account/Logoff")
                .Login(m_cashier)
                .NavigateToUrl("/#/rebate")
                .Sleep(3.Seconds());

            driver.Click("#add-rebate").Sleep(3.Seconds());
            driver.SelectOption("[name=rebate-ContractNo]", contractNo);
            driver.Value("[name=rebate-Amount]", "100");
            driver.Value("[name=rebate-StartDate]", DateTime.Today.ToString(CultureInfo.InvariantCulture));
            driver.Value("[name=rebate-EndDate]", DateTime.Today.AddMonths(1).ToString(CultureInfo.InvariantCulture));

            driver.ClickFirst("button", e => e.GetAttribute("data-bind") == "click:saveCommand");

            driver.Sleep(TimeSpan.FromSeconds(3));


            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([RebateId]) FROM [Sph].[Rebate]");
            Assert.IsTrue(max < latest);

            driver.Sleep(TimeSpan.FromSeconds(5), "Rebate berjaya ditambah");
            driver.Quit();
        }
    }
}
