using System;
using System.Data.SqlClient;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using FluentDateTime;

namespace web.test
{
    [TestFixture]
    public class ContractTest : BrowserTest
    {
        public const string CONTRACT_NAME = "Bangunan Komersil Di KB";
        public const string CONTRACT_TEMPLATE_TYPE = "Kontrak Baru";


        private TestUser m_contractAdmin;

        [SetUp]
        public void Init()
        {
            m_contractAdmin = new TestUser
            {
                UserName = "contractAdmin",
                FullName = "Contract Admin",
                Email = "contractadmin@bespoke.com.my",
                Department = "Test",
                Designation = "Boss",
                Password = "abcad12334535",
                StartModule = "admindashboard",
                Roles = new[] { "can_edit_contract_template", "admin_dashboard" }
            };
            this.AddUser(m_contractAdmin);
        }

        [Test]
        public void ContractFlowTest()
        {
            _001_AddContractTemplate();
            _002_CreateContractFromApplication();
        }

        [Test]
        public void _001_AddContractTemplate()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[ContractTemplate] WHERE [Type] =@Type", new SqlParameter("@Type", CONTRACT_TEMPLATE_TYPE));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([ContractTemplateId]) FROM [Sph].[ContractTemplate]");


            IWebDriver driver = new FirefoxDriver();
            driver.NavigateToUrl("/Account/Login");
            driver.Login("ruzzaima");
            driver.NavigateToUrl("/#contract.template.list")
                  .NavigateToUrl("/#/contract.template/0", 1.Seconds());
            
            driver.Sleep(TimeSpan.FromSeconds(3));


            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([ContractTemplateId]) FROM [Sph].[ContractTemplate]");
            Assert.IsTrue(max < latest);

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#/contract.template.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }

        [Test]
        public void _002_CreateContractFromApplication()
        {

            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([ContractId]) FROM [Sph].[Contract]");
            var templateId = this.GetDatabaseScalarValue<int>("SELECT [ContractTemplateId] FROM [Sph].[ContractTemplate] WHERE [Type] =@Type", new SqlParameter("@Type", CONTRACT_TEMPLATE_TYPE));


            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + "/Account/Login");
            driver.Login(m_contractAdmin);
            driver.NavigateToUrl("/#/admindashboard");
            driver.NavigateToUrl("/#/applicationlist/Diluluskan");
            driver.Sleep(TimeSpan.FromSeconds(2));
           var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([ContractId]) FROM [Sph].[Contract]");
            Assert.IsTrue(max < latest);

            driver.NavigateToUrl("/#/contract.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");

            driver.Quit();
        }
    }
}
