using System;
using System.Data.SqlClient;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using FluentDateTime;

namespace web.test
{
    [TestFixture]
    public class PaymentTest : BrowserTest
    {
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

        [Test]
        public void _001_DepositPayment()
        {
            var max = this.GetDatabaseScalarValue<string>("SELECT MAX([RegistrationNo]) FROM [Sph].[Deposit]");
            var driver = this.InitiateDriver();
            
            driver.NavigateToUrl("/Account/Logoff")
                .Login(m_cashier)
                .NavigateToUrl("/#/deposit",3.Seconds());

            driver.ClickFirst("a",e => e.Text == max.ToString())
                .Sleep(2.Seconds());
            driver
                .Click("#add-deposit-button")
                .Value(".input-deposit-receiptno","9898989")
                .Value(".input-deposit-amount","2000")
                ;

           driver
                .Click("#add-deposit-button")
                .Value(".input-deposit-receiptno", "878787",1)
                .Value(".input-deposit-amount", "3000",1)
                .Sleep(3.Seconds())
                ;

            driver.Click("#remove-deposit-payment", 1);
            driver.Click(".save-deposit-payment-button");

            driver.ClickFirst("a", e => e.Text == max.ToString())
               .Sleep(2.Seconds());
            driver.Sleep(TimeSpan.FromSeconds(3));


            var latest = this.GetDatabaseScalarValue<string>("SELECT MAX([RegistrationNo]) FROM [Sph].[Deposit]");
            Assert.AreEqual(max,latest);

            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }


    }
}
