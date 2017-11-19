using System;
using System.Security.Principal;
using System.Threading;
using Bespoke.Sph.Domain;
using Humanizer;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Xunit;

namespace Bespoke.Sph.WebTests.Helpers
{
    [CollectionDefinition(RX_WEB_COLLECTION)]
    public class RxIdeCollection : ICollectionFixture<RxIdeFixture>
    {
        public const string RX_WEB_COLLECTION = "RX IDE collection";
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }

    public class RxIdeFixture :  IDisposable
    {
        public  RxIdeFixture()
        {
            Driver = InitiateDriver();
            Driver.Login()
                .WaitUntil(By.ClassName("page-logo"), 2.Seconds());
        }
      

        public IWebDriver Driver { get; private set; }

        public static readonly string BaseUrl = ConfigurationManager.BaseUrl;
        public static readonly string ProjectDirectory = ConfigurationManager.Home;
        public string ConnectionString => ConfigurationManager.SqlConnectionString;


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


        public void Dispose()
        {
            Driver.LogOff();
            Driver?.Close();
            Driver?.Dispose();
            Driver = null;
        }
    }
}