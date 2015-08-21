using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.WebTests.Helpers;
using Humanizer;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Bespoke.Sph.WebTests
{
    [TestFixture]
    public class TriggerTestFixture : BrowserTest
    {
        

        [Test]
        public async Task _200_CreateRegistration()
        {
            File.Copy(@"C:\project\work\sph\bin\sources\EntityDefinition\Patient.json", $@"{ProjectDirectory}\\sources\\EntityDefinition\\Patient.json", true);

            var json = $"{ProjectDirectory}\\sources\\Trigger\\patient-register.json";
            if (File.Exists(json))
                File.Delete(json);
            this.Driver.NavigateToUrl("/sph#trigger.setup/0", 10.Seconds())
                .WaitUntil(By.Id("Entity"), 15.Seconds())
                .Value("#Entity", "Patient")
                .Value("#Name", "Register")
                .Value("#Note", "patients registration")
                .ClickFirst("input", x => null != x.GetAttribute("value") && x.GetAttribute("value") == "Register");

            this.Driver.ActivateTabItem("#rules-panel")
                .ClickFirst("a", x => x.GetAttribute("data-bind") != null && x.GetAttribute("data-bind").Contains("addRule"))
                .Wait(10.Seconds());

            // actions
            this.Driver.ClickFirst("a.dropdown-toggle", x => true)
                .WaitUntil(By.Id("a.btn-link"), 3.Seconds())
                .ClickFirst("a.btn-link", x => x.Text.Contains("Email"))
                .Wait(10.Seconds())
                .ClikOkDialog();
                
             this.Driver.Click("#save-button")
                .Wait(5.Seconds());

            await Task.Delay(5.Seconds());
            Assert.IsTrue(File.Exists(json));

        }
    }
}