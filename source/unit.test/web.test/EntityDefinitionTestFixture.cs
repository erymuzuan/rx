using System;
using System.IO;
using System.Threading.Tasks;
using Bespoke.Sph.WebTests.Helpers;
using Humanizer;
using NUnit.Framework;

namespace Bespoke.Sph.WebTests
{
    [TestFixture]
    public class EntityDefinitionTestFixture : BrowserTest
    {

        [Test]
        public async Task _001_DeveloperLogin()
        {
            this.Driver.Login(wait: 3000);

            await Task.Delay(2.Seconds());
            Assert.AreEqual(BaseUrl + "/sph#dev.home", this.Driver.Url);

        }
        [Test]
        public async Task _002_GoToNewEntityDefinition()
        {
            var json = $"{ProjectDirectory}\\sources\\EntityDefinition\\patient.json";
            if (File.Exists(json))
                File.Delete(json);
            this.Driver.NavigateToUrl("/sph#entity.details/0", 2.Seconds())
                .Value("#ent-name", "Patient")
                .Value("#record-name", "Mrn")
                .Value("#plural", "patients")
                .SelectOption("#ev-template", "Dashboard Standard")
                //mrn
                .ContextClick("a.jstree-clicked")
                .Wait(2.Seconds())
                .ClickFirst("a", x => x.Text.Contains("Add Child"))

                .Wait(10.Seconds())
                .Click("#save-button")

                .Wait(5.Seconds());
            await Task.Delay(5.Seconds());
            Assert.IsTrue(File.Exists(json));

        }
        [Test]
        public async Task _003_AddBusinessRules()
        {
            this.Driver.LogOff()
                .Wait(2.Seconds())
                .Login()
                .Wait(5.Seconds());

            this.Driver
                .NavigateToUrl("/sph#entity.details/patient", 5.Seconds());

          
            this.Driver
                .ActivateTabItem("#business-rules-tab")
                .Wait(500.Milliseconds());

            this.Driver
                .ClickFirst("a.btn-link", x => x.GetAttribute("data-bind") != null && x.GetAttribute("data-bind").Contains("addBusinessRule"))
                .Wait(2.Seconds())
                .Value("#Name", "Patient must be born before " + DateTime.Today.Year)
                .Value("#br-message", "Patient must be born before " + DateTime.Today.Year);

            // add filter
            this.Driver
                .ClickFirst("a.btn-link", x => x.GetAttribute("data-bind") != null && x.GetAttribute("data-bind").Contains("addFilter"))
                .Wait(1.Seconds());

            this.Driver 
                .ClickFirst("a.btn-link", x => x.GetAttribute("data-bind") != null && x.GetAttribute("data-bind").Contains("Left"))
                .Wait(1.Seconds())// add document field
                .ClickFirst("a.btn-link",
                    x => x.GetAttribute("data-bind") != null && x.GetAttribute("data-bind").Contains("Document"))
                .Wait(2.Seconds())
                .Value("#doc-field-path", "FullName")
                .Value("#doc-field-note", "FullName")
                .ClickLast("input.btn-default",
                    x => x.GetAttribute("data-bind") != null && x.GetAttribute("data-bind").Contains("okClick"))
                .Wait(1.Seconds());

            //this.Driver // add contant field
            //    .ClickFirst("a.btn-link", x => x.GetAttribute("data-bind") != null && x.GetAttribute("data-bind").Contains("Right"))
            //    .Wait(1.Seconds());
            //this.Driver
            //    .ClickFirst("a.btn-link",
            //        x => x.GetAttribute("data-bind") != null && x.GetAttribute("data-bind").Contains("Constant"))
            //    .Wait(2.Seconds());

            //    this.Driver
            //    .Value("#constant-field-value", "Michael")
            //    .Value("#constant-field-note", "Michael Corleone")
            //    .ClickLast("input.btn-default", x => x.GetAttribute("data-bind") != null && x.GetAttribute("data-bind").Contains("okClick"))
            //    .Wait(1.Seconds())
            //    ;


            await Task.Delay(5.Seconds());

        }
    }
}
