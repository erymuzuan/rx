using System;
using System.Data.SqlClient;
using System.Globalization;
using System.Xml.Linq;
using NUnit.Framework;
using FluentDateTime;

namespace web.test
{
    [TestFixture]
    public class PaymentTest : BrowserTest
    {
        public const string REGISTRATION_NO = "2013000046";
        public const string INVOICE_NO = "BSPB/2013/2013000037/082013";
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
        public void DepositAndRentalPaymentTest()
        {
            _001_CreateDeposit();
            _002_DepositPayment();
            _003_CreateRental();
            _004_RentalPayment();
        }

        // ReSharper disable InconsistentNaming
        [Test]
        public void _001_CreateDeposit()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[Deposit] WHERE [RegistrationNo] = @RegistrationNo", new SqlParameter("@RegistrationNo", REGISTRATION_NO));
          
            var x =
               string.Format(
                   "<Deposit xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" " +
                    "xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns=\"http://www.bespoke.com.my/\" " +
                   "WebId=\"{0}\" CreatedBy=\"{1}\" CreatedDate=\"2013-08-29T16:02:53.8586914+08:00\" " +
                   "ChangedBy=\"{1}\" ChangedDate=\"2013-08-29T16:02:53.8586914+08:00\" " +
                   "DepositId=\"0\" " +
                   "DateTime=\"2013-08-29T16:02:53.7716914+08:00\" " +
                   "Name=\"CEKAL RESOURCES\" " +
                   "IDNumber=\"001395454-U\" " +
                   "RegistrationNo=\"2013000046\" " +
                   "Amount=\"2000\" " +
                   "IsPaid=\"false\" " +
                   "IsRefund=\"false\" " +
                   "IsVoid=\"false\">" +
                   "<DepositPaymentCollection />" +
                   "<PaymentDateTime xsi:nil=\"true\" />" +
                   "<RefundDateTime xsi:nil=\"true\" />" +
                   "<DueDate>2013-09-06T00:00:00</DueDate>" +
                   "</Deposit>",
                   Guid.NewGuid(),
                   "test"
               
                   );
              this.ExecuteNonQuery("INSERT INTO [Sph].[Deposit]([DateTime],[Data],[Name],[IDNumber],[RegistrationNo],[Amount]," +
                                                      "[IsPaid],[IsRefund],[IsVoid]," +
                                                      "[CreatedDate],[ChangedDate],[CreatedBy],[ChangedBy]) " +
                                                      "VALUES(@DateTime, @Data,@Name, @IDNumber, @RegistrationNo,@Amount," +
                                                      "'false','false','false'," +
                                                      " '2013-01-01','2013-01-01','test','test')",
                new SqlParameter("@DateTime", DateTime.Today),
                new SqlParameter("@Data", x),
                new SqlParameter("@Name", "CEKAL RESOURCES"),
                new SqlParameter("@IDNumber", "001395454-U"),
                new SqlParameter("@RegistrationNo", "2013000046"),
                new SqlParameter("@Amount", "2000")
                );

            Console.WriteLine("Deposit amount 2000 is successfully created");
        }

     
        [Test]
        public void _002_DepositPayment()
        {
            var max = this.GetDatabaseScalarValue<string>("SELECT MAX([RegistrationNo]) FROM [Sph].[Deposit]");
            var driver = this.InitiateDriver();
            
            driver.NavigateToUrl("/Account/Logoff")
                .Login(m_cashier)
                .NavigateToUrl("/#/deposit",3.Seconds());

            driver.ClickFirst("a",e => e.Text == max.ToString(CultureInfo.InvariantCulture))
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

            driver.ClickFirst("a", e => e.Text == max.ToString(CultureInfo.InvariantCulture))
               .Sleep(2.Seconds());
            driver.Sleep(TimeSpan.FromSeconds(3));


            var latest = this.GetDatabaseScalarValue<string>("SELECT MAX([RegistrationNo]) FROM [Sph].[Deposit]");
            Assert.AreEqual(max,latest);

            driver.Sleep(TimeSpan.FromSeconds(5), "Bayaran deposit berjaya");
            driver.Quit();
        }

        [Test]
        public void _003_CreateRental()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[Invoice] WHERE [No] = @No", new SqlParameter("@No", INVOICE_NO));
            var contractNo = this.GetDatabaseList<string>("SELECT MAX([ReferenceNo]) FROM [Sph].[Contract]");
            var idSsm = this.GetDatabaseList<string>("SELECT [TenantIdSsmNo] FROM [Sph].[Contract] WHERE [ReferenceNo] = @ReferenceNo", new SqlParameter("@ReferenceNo",contractNo));
            var xml = XElement.Load(@".\invoice.xml");
            this.ExecuteNonQuery("INSERT INTO [Sph].[Invoice]([TenantIdSsmNo],[Data],[Type],[ContractNo],[No],[Amount]," +
                                                    "[Date]," +
                                                    "[CreatedDate],[ChangedDate],[CreatedBy],[ChangedBy]) " +
                                                    "VALUES(@TenantIdSsmNo,@Data,@Type,@ContractNo,@No,@Amount," +
                                                    "@Date," +
                                                    " '2013-01-01','2013-01-01','test','test')",
              new SqlParameter("@TenantIdSsmNo", idSsm),
              new SqlParameter("@Data", xml.ToString()),
              new SqlParameter("@Type", "Rental"),
              new SqlParameter("@ContractNo", contractNo),
              new SqlParameter("@No", "BSPB/2013/2013000037/082013"),
              new SqlParameter("@Amount", "2500"),
              new SqlParameter("@Date", DateTime.Today)
              );

            Console.WriteLine("Rental amount 2500 is successfully created");
        }

        [Test]
        public void _004_RentalPayment()
        {
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([InvoiceId]) FROM [Sph].[Invoice] WHERE [Type] = 'Rental'");
            var driver = this.InitiateDriver();

            driver.NavigateToUrl("/Account/Logoff")
                .Login(m_cashier)
                .NavigateToUrl("/#/payment", 3.Seconds());

            driver.Click(".set-payment-button")
                .Sleep(2.Seconds());
           
            driver.Value("[name=ReceiptNo]","801291")
                   .Value("[name=Amount]", "200");
            driver.Sleep(TimeSpan.FromSeconds(3));

            driver.Click(".save-rental-payment-button");
            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([InvoiceId]) FROM [Sph].[Invoice] WHERE [Type] = 'Rental'");
            Assert.AreEqual(max, latest);

            driver.Sleep(TimeSpan.FromSeconds(5), "Bayaran sewa berjaya");
            driver.Quit();
        }
    }
}
