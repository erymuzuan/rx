using System;
using System.Data.SqlClient;
using System.Globalization;
using FluentDate;
using NUnit.Framework;

namespace web.test
{
    [TestFixture]
    public class RentalApplicationFlowTest : BrowserTest
    {
        private TestUser m_admindashboard;

        [SetUp]
        public void Init()
        {
            m_admindashboard = new TestUser
                {
                    UserName = "admindashboard",
                    FullName = "Admin Permohonan",
                    Email = "admindashboard@gmail.com",
                    Password = "123456",
                    Roles = new[] { "admin_dashboard", "can_edit_contract_template" },
                    StartModule = "admindashboard",
                    Department = "Unit Hartanah",
                    Designation = "Pegawai Permohonan",
                    Telephone = "03-8881111"
                };
            this.AddUser(m_admindashboard);
        }
        [Test]
        public void ApplicationFlowTest()
        {
            _001_MoveNewApplicationIntoWaitingList();
            _002_ApproveWaitingListApplication();
            _003_PrepareOffer();
            _004_PrepareContract();
            _005_MarkApplicationAsComplete();
            _006_CreateTenantForSuccessApplication();
        }

        // ReSharper disable InconsistentNaming
        [Test]
        public void _001_MoveNewApplicationIntoWaitingList()
        {
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([RentalApplicationId]) FROM [Sph].[RentalApplication] WHERE [Status]='Baru'");
            Assert.AreNotEqual(0, max, "run SubmitApplicationFLowTest first ");

            var driver = this.InitiateDriver();
            driver.NavigateToUrl("/Account/Login");
            driver.Login(m_admindashboard);
            driver.NavigateToUrl(string.Format("/#/rentalapplication.verify/{0}",max))
                  .Sleep(2.Seconds());
            driver.ClickFirst("button", e => e.Text == "Masuk Senarai Menunggu");
            driver.Sleep(TimeSpan.FromSeconds(3));


            var status = this.GetDatabaseScalarValue<string>("SELECT [Status] FROM [Sph].[RentalApplication] WHERE [RentalApplicationId] = @Id",new SqlParameter("@Id",max));
            Assert.AreEqual("Menunggu", status);

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#/applicationlist/Menunggu");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }

        [Test]
        public void _002_ApproveWaitingListApplication()
        {
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([RentalApplicationId]) FROM [Sph].[RentalApplication] WHERE [Status]='Menunggu'");
            Assert.AreNotEqual(0, max, "run _001_MoveNewApplicationIntoWaitingList first ");

            var driver = this.InitiateDriver();
            driver.NavigateToUrl("/Account/Login");
            driver.Login(m_admindashboard);
            driver.NavigateToUrl(string.Format("/#/rentalapplication.verify/{0}", max))
                  .Sleep(2.Seconds());
            driver.ClickFirst("button", e => e.Text == "Luluskan");
            driver.Sleep(3.Seconds());


            var status = this.GetDatabaseScalarValue<string>("SELECT [Status] FROM [Sph].[RentalApplication] WHERE [RentalApplicationId] = @Id", new SqlParameter("@Id", max));
            Assert.AreEqual("Diluluskan", status);

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#/applicationlist/Diluluskan");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }


        [Test]
        public void _003_PrepareOffer()
        {
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([RentalApplicationId]) FROM [Sph].[RentalApplication] WHERE [Status]='Diluluskan'");
            Assert.AreNotEqual(0, max, "run _002_ApproveWaitingListApplication first ");

            var driver = this.InitiateDriver();
            driver.NavigateToUrl("/Account/Login");
            driver.Login(m_admindashboard);
            driver.NavigateToUrl(string.Format("/#/rentalapplication.verify/{0}", max))
                  .Sleep(2.Seconds());
            driver.ClickFirst("button", e => e.Text.Contains("Sediakan Tawaran"));
            driver.Sleep(TimeSpan.FromSeconds(3));
            driver.Value("[name=offer-Deposit]", "7000")
                  .Value("[name=offer-Date]", DateTime.Today.ToString(CultureInfo.InvariantCulture))
                  .Value("[name=offer-ExpiryDate]", DateTime.Today.AddDays(7).ToString(CultureInfo.InvariantCulture))
                  .Value("[name=offer-Period]", "2")
                  .SelectOption("[name=offer-periodunit]", "Tahun")
                ;

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: $parent.addConditionCommand")
                .Sleep(200.Milliseconds())
                .Value(".input-offer-condition-title", "Pembayaran Deposit")
                .Value(".input-offer-condition-description", "Mesti dijelaskan sebelom / semasa menandatangani kontrak")
                .Value(".input-offer-condition-note", "Mesti dijelaskan sebelom / semasa menandatangani kontrak")
                .Click(".input-offer-condition-isrequired")
                ;

            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: $parent.addConditionCommand")
               .Sleep(200.Milliseconds())
               .Value(".input-offer-condition-title", "Pembayaran Sewa",1)
               .Click(".remove-offer-condition-button",1)
               ;
            driver.ClickFirst("button", e => e.Text == "Simpan");
            var current = this.GetDatabaseScalarValue<int>("SELECT MAX([RentalApplicationId]) FROM [Sph].[RentalApplication]");
            Assert.IsTrue(max == current);
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }

        [Test]
        public void _004_PrepareContract()
        {
            var rentalId = this.GetDatabaseScalarValue<int>("SELECT MAX([RentalApplicationId]) FROM [Sph].[RentalApplication] WHERE [Status]='Diluluskan'");
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([ContractId]) FROM [Sph].[Contract]");
            Assert.AreNotEqual(0, max, "run _002_ApproveWaitingListApplication first ");

            var templateId = this.GetDatabaseScalarValue<int>("SELECT [ContractTemplateId] FROM [Sph].[ContractTemplate] WHERE [Type] = @TemplateType ", new SqlParameter("@TemplateType", ContractTest.CONTRACT_TEMPLATE_TYPE));
            Assert.AreNotEqual(0, templateId, "run the _001_AddContractTemplate test first");
            
            var driver = this.InitiateDriver();
            driver.NavigateToUrl("/Account/Login");
            driver.Login(m_admindashboard);
            driver.NavigateToUrl(string.Format("/#/rentalapplication.verify/{0}", rentalId))
                  .Sleep(2.Seconds()); driver.NavigateToUrl(string.Format("/#/contract.create/{0}", rentalId))
                  .Sleep(3.Seconds());

            driver.SelectOption("[name=contract-Type]", ContractTest.CONTRACT_TEMPLATE_TYPE)
                .ClickFirst("button", e => e.Text == "Generate")
                .ClickFirst("button", e => e.Text == "Simpan")
                ;
            var current = this.GetDatabaseScalarValue<int>("SELECT MAX([ContractId]) FROM [Sph].[Contract]");
            Assert.IsTrue(max < current);
            driver.NavigateToUrl("/#/contract.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }

        [Test]
        public void _005_MarkApplicationAsComplete()
        {
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([RentalApplicationId]) FROM [Sph].[RentalApplication] WHERE [Status]='Diluluskan'");
            Assert.AreNotEqual(0, max, "run _002_ApproveWaitingListApplication first ");

            var driver = this.InitiateDriver();
            driver.NavigateToUrl("/Account/Login");
            driver.Login(m_admindashboard);
            driver.NavigateToUrl(string.Format("/#/rentalapplication.verify/{0}", max))
                  .Sleep(2.Seconds());
            driver.ClickFirst("button", e => e.Text.Contains("Selesai"));
            driver.Sleep(TimeSpan.FromSeconds(3));


            var current = this.GetDatabaseScalarValue<string>("SELECT [Status] FROM [Sph].[RentalApplication] WHERE [RentalApplicationId] = @Id",new SqlParameter("@Id",max));
            Assert.AreEqual("Selesai",current);

            driver.NavigateToUrl("/#/applicationlist/Selesai");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        } 
        
        [Test]
        public void _006_CreateTenantForSuccessApplication()
        {
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([RentalApplicationId]) FROM [Sph].[RentalApplication] WHERE [Status]='Selesai'");
            Assert.AreNotEqual(0, max, "run _005_MarkApplicationAsComplete first ");
            
            var contractId = this.GetDatabaseScalarValue<int>("SELECT MAX([ContractId]) FROM [Sph].[Contract]");
            var username = this.GetDatabaseScalarValue<string>("SELECT [TenantIdSsmNo] FROM [Sph].[Contract] WHERE [ContractId]=@Id", new SqlParameter("@Id", contractId));
           
            var driver = this.InitiateDriver();
            driver.NavigateToUrl("/Account/Login");
            driver.Login(m_admindashboard);
            driver.NavigateToUrl(string.Format("/#/rentalapplication.verify/{0}", max))
                  .Sleep(1.Seconds());
            driver.ClickFirst("button", e => e.Text == "Sediakan Maklumat Penyewa");
            driver.Sleep(TimeSpan.FromSeconds(3));


            var userId = this.GetDatabaseScalarValue<int>("SELECT [UserProfileId] FROM [Sph].[UserProfile] WHERE [UserName] = @Username", new SqlParameter("@Username", username));
            Assert.AreNotEqual(0, userId);

            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }
    }
}
