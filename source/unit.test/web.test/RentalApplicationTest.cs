using System;
using System.Data.SqlClient;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace web.test
{
    [TestFixture]
    public class RentalApplicationTest : BrowserTest
    {
        public const string RA_COMPANYREGISTRATION_NO = "B29999";
        public const string APP_TEMPLATE_NAME = "B29999";

        [Test]
        public void _001_New()
        {
            const string sphDatabase = "sph";
            sphDatabase.ExecuteNonQuery("DELETE FROM [Sph].[RentalApplication] WHERE [CompanyRegistrationNo] =@No", new SqlParameter("@No", RA_COMPANYREGISTRATION_NO));
            var max = sphDatabase.GetDatabaseScalarValue<int>("SELECT MAX([RentalApplicationId]) FROM [Sph].[RentalApplication]");

            var templateId = sphDatabase.GetDatabaseScalarValue<int>("SELECT [ApplicationTemplateId] FROM [Sph].[ApplicationTemplate] WHERE [Name] =@Name", new SqlParameter("@Name", APP_TEMPLATE_NAME));
            IWebDriver driver = new FirefoxDriver();
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + "/Account/Login");
            driver
                .Sleep(TimeSpan.FromSeconds(2))
                  .Value("[name='UserName']", "ruzzaima")
                  .Value("[name='Password']", "123456")
                  .Click("[name='submit']");
            driver
                .Sleep(TimeSpan.FromSeconds(5))
                ;
            driver.Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + "/#/rentalapplication.selectspace");
            driver.Sleep(TimeSpan.FromSeconds(3));
            driver
                .Navigate().GoToUrl(WEB_RUANG_KOMERCIAL_URL + "/#/application.detail-templateid.1/19");
            driver.Sleep(TimeSpan.FromSeconds(3));
            driver
                .Value("[name='rentalapplication.Contact.Name']","Zaima")
                .Value("[name='rentalapplication.Contact.IcNo']","860908-03-5604")
                .Value("[name='rentalapplication.Contact.MobileNo']","013-6312237")
                .Value("[name='rentalapplication.Contact.Email']","zaima@hotmail.com")
                .Click("[name='rentalapplication.IsCompany']")
                .Value("[name='rentalapplication.CompanyName']", "Yunaz Sdn Bhd")
                .Value("[name='rentalapplication.CompanyRegistrationNo']", "B29999")
                .SelectOption("[name='rentalapplication.CompanyType']", "Partnership")
                .Value("[name='rentalapplication.Contact.OfficeNo']", "03-7724568")
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
