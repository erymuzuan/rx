using System.Configuration;
using System.Security.Principal;
using System.Threading;
using Humanizer;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Bespoke.Sph.WebTests.Helpers
{
    [TestFixture]
    public class BrowserTest : ISqlConnectionConsumer
    {
        private IWebDriver m_driver;
        [TestFixtureSetUp]
        public void Setup()
        {
            m_driver = InitiateDriver();

            m_driver.Login()
                .WaitUntil(By.ClassName("page-logo"), 2.Seconds());
        }
        [TestFixtureTearDown]
        public void TearDown()
        {
            m_driver.LogOff();
            m_driver?.Close();
            m_driver?.Dispose();
            m_driver = null;
        }

        protected IWebDriver Driver => m_driver;

        public static readonly string BaseUrl = ConfigurationManager.AppSettings["BaseUrl"];
        public static readonly string ProjectDirectory = ConfigurationManager.AppSettings["ProjectDirectory"];
        public string ConnectionString => ConfigurationManager.ConnectionStrings["Sph"].ConnectionString;


        internal static void SetCurrentIdentity(string userName)
        {
            var id = new GenericIdentity(userName);
            Thread.CurrentPrincipal = new GenericPrincipal(id, new[] { "user" });
        }

        protected IWebDriver InitiateDriver(string agent = null)
        {
            IWebDriver driver = new FirefoxDriver();
            return driver;
        }



    }
}