using NUnit.Framework;
using OpenQA.Selenium;

namespace web.test
{
    public class BrowserTest
    {

        public const string WEB_RUANG_KOMERCIAL_URL = "http://localhost:4436";
        public const string WEB_DRIVER_PATH = @"D:\project\work\quarters\webdrivers";
        public const string DUMMY_USER_NAME = "[Test] Dummy User";

        protected void PayWithMigs(IWebDriver driver)
        {
            Assert.IsTrue(driver.PageSource.Contains("Sila pilih cara bayaran"));
            driver
                .Click("#migLinkButton")
                .Sleep(10000, "MIGS website to show up");
        }

        protected void InsertMasterCardDetails(IWebDriver driver)
        {
            Assert.IsTrue(driver.PageSource.Contains("TEST MODE"));
            driver.FindElement(By.Name("MasterCard")).Click();

            driver
                .Wait(200, "Selecting mastercard for test mode")
                .SetText("#CardNumber", "5123456789012346")
                .SetText("#CardMonth", "05")
                .SetText("#CardYear", "13")
                .SetText("#Securecode", "100")
                .Click("#Paybutton")
                .Wait(5000, "Paying the bill... at MIGS");
        }

    }
}