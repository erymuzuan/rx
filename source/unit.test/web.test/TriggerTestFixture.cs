using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebTests.Helpers;
using Humanizer;
using OpenQA.Selenium;
using Xunit;

namespace Bespoke.Sph.WebTests
{
    [Collection(RxIdeCollection.RX_WEB_COLLECTION)]
    public class TriggerTestFixture 
    {
        public RxIdeFixture Fixture { get; }

        public TriggerTestFixture(RxIdeFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public async Task _200_CreateRegistration()
        {
           // File.Copy($@"{ConfigurationManager.SphSourceDirectory}\Patient.json", $@"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition\\Patient.json", true);

            var json = $"{ConfigurationManager.SphSourceDirectory}\\Trigger\\patient-register.json";
            if (File.Exists(json))
                File.Delete(json);
            Fixture.Driver.NavigateToUrl("/sph#trigger.setup/0", 10.Seconds())
                .WaitUntil(By.Id("Entity"), 15.Seconds())
                .Value("#Entity", "Patient")
                .Value("#Name", "Register")
                .Value("#Note", "patients registration")
                .ClickFirst("input", x => null != x.GetAttribute("value") && x.GetAttribute("value") == "Register");

            Fixture.Driver.ActivateTabItem("#rules-panel")
                .ClickFirst("a", x => x.GetAttribute("data-bind") != null && x.GetAttribute("data-bind").Contains("addRule"))
                .Wait(10.Seconds());

            // actions
            Fixture.Driver.ClickFirst("a.dropdown-toggle", x => true)
                .WaitUntil(By.Id("a.btn-link"), 3.Seconds())
                .ClickFirst("a.btn-link", x => x.Text.Contains("Email"))
                .Wait(10.Seconds())
                .ClikOkDialog();
                
             Fixture.Driver.Click("#save-button")
                .Wait(5.Seconds());

            await Task.Delay(5.Seconds());
            Assert.True(File.Exists(json));

        }
    }
}