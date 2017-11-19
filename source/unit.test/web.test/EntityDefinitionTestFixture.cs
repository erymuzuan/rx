﻿using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bespoke.Sph.Domain;
using Bespoke.Sph.WebTests.Helpers;
using Humanizer;
using OpenQA.Selenium;
using Xunit;

namespace Bespoke.Sph.WebTests
{
    [Collection(RxIdeCollection.RX_WEB_COLLECTION)]
    public class EntityDefinitionTestFixture 
    {
        public RxIdeFixture Fixture { get; }

        public EntityDefinitionTestFixture(RxIdeFixture fixture)
        {
            Fixture = fixture;
        }

        [Fact]
        public async Task _100_DeveloperLogin()
        {
            Fixture.Driver.Login(wait: 3000);
            await Task.Delay(2.Seconds());
            Assert.Equal(ConfigurationManager.BaseUrl + "/sph#dev.home", Fixture.Driver.Url);

        }


        [Fact]
        public async Task _200_CreatePatient()
        {
            var json = $"{ConfigurationManager.SphSourceDirectory}\\EntityDefinition\\patient.json";
            if (File.Exists(json))
                File.Delete(json);
            Fixture.Driver.NavigateToUrl("/sph#entity.details/0", 2.Seconds())
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
            Assert.True(File.Exists(json));

        }
        [Fact]
        public async Task _300_AddBusinessRules()
        {
            Fixture.Driver.Navigate().Refresh();
            Fixture.Driver
                .NavigateToUrl("/sph#entity.details/patient", 1.Seconds())
                .WaitUntil(By.Id("save-button"), 2.Seconds());


            Fixture.Driver
                .ActivateTabItem("#business-rules-tab")
                .Wait(500.Milliseconds());

            Fixture.Driver
                .ClickFirst("a.btn-link", x => x.GetAttribute("data-bind") != null && x.GetAttribute("data-bind").Contains("addBusinessRule"))
                .Wait(2.Seconds())
                .Value("#Name", "Patient must be born before " + DateTime.Today.Year)
                .Value("#br-message", "Patient must be born before " + DateTime.Today.Year);



            _310_AddRuleFilter();
            _320_AddRule();
            Fixture.Driver
           .ClickLast("input[value=OK]", x => null != x.GetAttribute("data-bind"))
           .Wait(1.Seconds());

            Fixture.Driver.Click("#save-button");


            await Task.Delay(5.Seconds());

        }
        [Fact]
        public void _310_AddRuleFilter()
        {
            // add filter
            Fixture.Driver
                .ExecuteScript("$(\"a.btn-link[data-bind*=addFilter]:visible\").trigger(\"click\");")
                .Wait(1.Seconds());

            Fixture.Driver
                .ExecuteScript("$(\"a.btn-link.dropdown[data-bind*=Left]:visible\").trigger(\"click\");")
                .Wait(1.Seconds())// add document field
                .ExecuteScript("$(\"a.btn-link[data-bind*=Document]:visible\").trigger(\"click\");")
                .WaitUntil("#doc-field-path")
                .Value("#doc-field-path", "FullName")
                .Value("#doc-field-note", "FullName")
                .ClikOkDialog()
               .Wait(1.Seconds());

            // RIGHT
            Fixture.Driver
                .ExecuteScript("$(\"a.btn-link.dropdown[data-bind*=Right]:visible\").trigger(\"click\");")
                .Wait(1.Seconds());
            Fixture.Driver// RIGHT add constant field
                .ExecuteScript("$(\"a.btn-link[data-bind*=Constant]:visible\").trigger(\"click\");")
                .WaitUntil(By.Id("constant-field-value"), 1500.Milliseconds());
            Fixture.Driver // rule Right
                .Value("#constant-field-value", "Michael")
                .Value("#constant-field-note", "Michaels Schumaker")
                .ClikOkDialog()
                .Wait(1.Seconds());



        }

        [Fact]
        public void _320_AddRule()
        {
            // add a rule
            Fixture.Driver
                .ExecuteScript("$(\"a.btn-link[data-bind*=addRule]:visible\").trigger(\"click\");")
                .Wait(1.Seconds());

            // LEFT
            Fixture.Driver
                .ExecuteScript("$(\"a.btn-link.dropdown[data-bind*=Left]:visible\").trigger(\"click\");")
                .Wait(1.Seconds());

            Fixture.Driver// LEFT add document field
                .ExecuteScript("$(\"a.btn-link[data-bind*=Document]:visible\").trigger(\"click\");")
                .Wait(500.Milliseconds());
            Fixture.Driver
                .Value("#doc-field-path", "FullName")
                .Value("#doc-field-note", "FullName")
                .ClikOkDialog()
                .Wait(1.Seconds());

            // RIGHT
            Fixture.Driver
                .ExecuteScript("$(\"a.btn-link.dropdown[data-bind*=Right]:visible\").trigger(\"click\");")
                .Wait(1.Seconds());
            _321_AddConstantField("Michael Schumaker");

            // 2nd rule
            Fixture.Driver
                .ExecuteScript("$(\"a.btn-link.dropdown[data-bind*=Left]:visible\").trigger(\"click\");")
                .Wait(1.Seconds());

            _322_AddFunctionField("Michael Schumaker");



        }

        private void _321_AddConstantField(string value, string note = null)
        {
            Fixture.Driver// RIGHT add constant field
            .ExecuteScript("$(\"a.btn-link[data-bind*=Constant]:visible\").trigger(\"click\");")
            .Wait(500.Milliseconds());
            Fixture.Driver // rule Right
                .Value("#constant-field-value", value)
                .Value("#constant-field-note", note ?? value)
                .ClikOkDialog()
                .Wait(1.Seconds());
        }

        private void _322_AddFunctionField(string name, string note = null)
        {
            Fixture.Driver// RIGHT add constant field
            .ExecuteScript("$(\"a.btn-link[data-bind*=Function]:visible\").trigger(\"click\");")
            .Wait(500.Milliseconds());
            var parentHandle = Fixture.Driver.CurrentWindowHandle;
            Fixture.Driver // rule Right
                .Value("#function-field-name", name)
                .ExecuteScript("$(\"a[data-bind*=edit]:visible\").trigger(\"click\");")
                .Wait(5.Seconds());

            Thread.Sleep(1.Seconds());


            var wh = Fixture.Driver.WindowHandles.Last();
            Fixture.Driver.SwitchTo().Window(wh);
            Thread.Sleep(5.Seconds());
            
            Fixture.Driver.SendKeys("#editor textarea", "return DateTime.Today;");
            Thread.Sleep(5.Seconds());
            Fixture.Driver
                .ExecuteScript("$(\"a[data-bind*=saveAndClose]\").trigger(\"click\");")
                .SwitchTo()
                .Window(parentHandle); // switch back to the original window
            Thread.Sleep(10.Seconds());
            Fixture.Driver
                .Value("#function-field-note", note ?? name)
                .ClikOkDialog()
                .Wait(1.Seconds());
        }

        [Fact]
        public void _400_AddOperation()
        {
            Fixture.Driver.Navigate().Refresh();
            Fixture.Driver
                .NavigateToUrl("/sph#entity.details/patient", 500.Milliseconds())
                .WaitUntil(By.Id("save-button"), 2.Seconds());


            Fixture.Driver
                .ActivateTabItem("#operations-tab")
                .Wait(500.Milliseconds());

            Fixture.Driver
                .ClickFirst("a.btn-link", x => !string.IsNullOrWhiteSpace(x.Text) && x.Text.Contains("Add an operation"))
                .Wait(2.Seconds())
                .ClickLast("button.btn-default", x => x.Text == "No")
                .WaitUntil(By.Id("success-message"), 2.Seconds());

            Assert.Contains("operation", Fixture.Driver.Url);

            Fixture.Driver.Value("#name", "Register")
                .Value("#success-message", "The patient has been registered")
                .Value("#navigate-url", "#patient");
            Fixture.Driver
                .Click("input[value=developers]");

            Fixture.Driver
                .ClickLast("a.btn-link", x => null != x.GetAttribute("data-bind") && x.GetAttribute("data-bind").Contains("addChildAction"))
                .Wait(1.Seconds())
                .Value("#setter-action-child-path", "FullName");

            Fixture.Driver
                .Click("a.dropdown")
                .Wait(500.Milliseconds())
                .Click("a.btn-add-constant-setter-field");

            Fixture.Driver
                .Value("#constant-field-value", "Abc")
                .Value("#constant-field-name", "Abc")
                .Value("#constant-field-note", "ABC")
                .ClickLast("input[value=OK]", x => null != x.GetAttribute("data-bind"))
                .Wait(2.Seconds());

            Fixture.Driver.Click("#save-button");

        }
    }
}
