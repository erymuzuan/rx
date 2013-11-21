using System;
using System.Data.SqlClient;
using Humanizer;
using NUnit.Framework;

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
            _001_AddNewContractTemplate();
        }

        [Test]
        // ReSharper disable InconsistentNaming
        public void _001_AddNewContractTemplate()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[ContractTemplate] WHERE [Type] =@Type", new SqlParameter("@Type", CONTRACT_TEMPLATE_TYPE));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([ContractTemplateId]) FROM [Sph].[ContractTemplate]");


            var driver = this.InitiateDriver();
            driver.NavigateToUrl("/Account/Login");
            driver.Login(m_contractAdmin);
            driver.NavigateToUrl("/#/contract.template.list")
                  .NavigateToUrl("/#/contract.template/0", 1.Seconds());

            driver.Value("[name=template-Type]", CONTRACT_TEMPLATE_TYPE)
                  .Value("[name=template-Description]", "Templat kontrak untuk penyewa baru")
                  .ClickFirst("a", e => e.Text == "Topics dan Klaus")
                ;

            //add first topic
                  driver
                  .ClickFirst("a", e => e.GetAttribute("data-bind") == "click: startAddTopicCommand")
                  .Sleep(1.Seconds());

            driver.Value("[name=topic-Title]", "Kebersihan")
                .Value("[name=topic-Text]", "Kebersihan di premis prlu di jaga setiap hari")
                .Value("[name=topic-Description]", "Kebersihan di premis prlu di jaga setiap hari")
                .ClickFirst("input", e => e.GetAttribute("data-bind") == "click: topicDialogOk")
                .Sleep(200.Milliseconds())
                ;

            //add 2nd topic
            driver.ClickFirst("a", e => e.GetAttribute("data-bind") == "click: startAddTopicCommand")
                  .Sleep(1.Seconds());

            driver.Value("[name=topic-Title]", "Keharmonian")
               .Value("[name=topic-Text]", "Keharmonian di premis prlu di jaga")
               .Value("[name=topic-Description]", "Keharmonian di premis perlu di jaga")
               .ClickFirst("input", e => e.GetAttribute("data-bind") == "click: topicDialogOk")
               .Sleep(200.Milliseconds())
               ;
            driver.Click("#save-button")
                    .Sleep(TimeSpan.FromSeconds(3));
            
            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([ContractTemplateId]) FROM [Sph].[ContractTemplate]");
            Assert.IsTrue(max < latest);

            driver.Sleep(TimeSpan.FromSeconds(2));
            driver.NavigateToUrl("/#/contract.template.list");
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }
    }
}
