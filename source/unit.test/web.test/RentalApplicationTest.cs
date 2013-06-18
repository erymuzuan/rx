using System;
using System.Data.SqlClient;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using System.Linq;

namespace web.test
{
    [TestFixture]
    public class RentalApplicationTest : BrowserTest
    {
        public const string RA_COMPANYREGISTRATION_NO = "B19999";

        [Test]
        public void _001_New()
        {
            const string sphDatabase = "sph";
            sphDatabase.ExecuteNonQuery("DELETE FROM [Sph].[RentalApplication] WHERE [CompanyRegistrationNo] =@No", new SqlParameter("@No", RA_COMPANYREGISTRATION_NO));
            var max = sphDatabase.GetDatabaseScalarValue<int>("SELECT MAX([RentalApplicationId]) FROM [Sph].[RentalApplication]");

            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + "/#/rentalapplication.selectspace");
            driver.Sleep(TimeSpan.FromSeconds(3));

            driver
                .Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + "/#/rentalapplication/9");
            driver.Sleep(TimeSpan.FromSeconds(3));
            driver
                .Click("[name='rentalapplication.IsCompany']")
                .Value("[name='rentalapplication.Contact.Title']","Cik")
                .Value("[name='rentalapplication.Contact.Name']","Ruzzaima")
                .Value("[name='rentalapplication.Contact.IcNo']","860908-29-5604")
                .Value("[name='rentalapplication.Contact.MobileNo']","013-6312237")
                .Value("[name='rentalapplication.Contact.OfficeNo']","03-7724568")
                .Value("[name='rentalapplication.Contact.Email']","ruzzaima@hotmail.com")
                .Value("[name='rentalapplication.CompanyName']", "Bespoke Technology Sdn Bhd")
                .Value("[name='rentalapplication.CompanyRegistrationNo']", "B19999")
                .SelectOption("[name='rentalapplication.CompanyType']", "Partnership")
                .Value("[name='rentalapplication.Address.Street']", "Jalan SS20/27")
                .Value("[name='rentalapplication.Address.City']", "Petaling Jaya")
                .Value("[name='rentalapplication.Address.Postcode']", "47300")
                .SelectOption("[name='rentalapplication.Address.State']", "Selangor")
                .Value("[name='rentalapplication.Purpose']", "Menjalankan perniagaan cafeteria")
                .Value("[name='rentalapplication.Experience']", "Pernah menjalankan perniagaan cafeteria")
                .Click("#save-application-button")
                .Sleep(TimeSpan.FromSeconds(5))
                ;


            var id = sphDatabase.GetDatabaseScalarValue<int>("SELECT [RentalApplicationId] FROM [Sph].[RentalApplication] WHERE [CompanyRegistrationNo] =@No", new SqlParameter("@No", RA_COMPANYREGISTRATION_NO));
            Assert.IsTrue(max < id);
            driver.Sleep(TimeSpan.FromSeconds(5), "See the result");
            driver.Quit();
        }
    }
}
