using System;
using System.Data.SqlClient;
using System.Globalization;
using FluentDateTime;
using NUnit.Framework;

namespace web.test
{
    [TestFixture]
    public class TriggerTest : BrowserTest
    {
        public const string TRIGGER_NAME = "Email Trigger (TEST)";
        private TestUser m_admintrigger;

        [SetUp]
        public void Init()
        {
            m_admintrigger = new TestUser
            {
                UserName = "admintrigger",
                Password = "979962wer",
                FullName = "Admin Trigger",
                Department = "Pentadbiran",
                Designation = "Pengurus",
                Email = "admintrigger@gmail.com",
                Roles = new[] { "can_add_trigger" },
                StartModule = "trigger.list",
                Telephone = "03-80008000"
            };
            this.AddUser(m_admintrigger);
        }

        // ReSharper disable InconsistentNaming
        [Test]
        public void _001_AddNewTrigger()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[Trigger] WHERE [Name]=@TriggerName", new SqlParameter("@TriggerName", TRIGGER_NAME));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([TriggerId]) FROM [Sph].[Trigger]");
            var driver = this.InitiateDriver();

            driver.NavigateToUrl("/Account/Logoff")
                .Login(m_admintrigger)
                .NavigateToUrl("/#/trigger.setup/0")
                .Sleep(3.Seconds());

            driver.Value("[name=Name]", TRIGGER_NAME);
            driver.SelectOption("[name=Entity]", "Aduan");
            driver.Value("[name=Note]", "Trigger email kepada penyewa atau orang awam yang hantar aduan")
              .Click("[name=IsActive]");

            //add first rules - document and constant field
            driver.Click("a", e => e.GetAttribute("data-bind") == "click: $parent.addRuleCommand")
                .Click("a", e => e.GetAttribute("data-bind") == "with : Left")
                  .Click("a", e => e.Text.Contains("Document Field"))
                  .Sleep(2.Seconds())
                ;

            driver.Value("[name=doc-field-name]", "ComplaintId")
                .Value("[name=doc-field-note]", "Id Aduan")
                .Value("[name=doc-field-path]", "ComplaintId")
                .ClickFirst("input", e => e.GetAttribute("data-bind") == "click: $root.saveField")
                ;

            driver.SelectOption("[name=rule-operator]", ">")
                  .Click("a", e => e.GetAttribute("data-bind") == "with : Right")
                   .Click("a", e => e.Text.Contains("Constant Field"))
                  .Sleep(2.Seconds())
                  .Value("[name=constant-field-name]", "Kosong")
                  .Value("[name=constant-field-note]", "Nilai kosong")
                  .Value("[name=constant-field-typename]", "Nombor bulat")
                  .Value("[name=constant-field-value]", "0");
                driver.ClickFirst("button", e => e.GetAttribute("data-bind") == "click: $root.saveField")
                ;
            driver.Sleep(TimeSpan.FromSeconds(3));

            // add email trigger action
            driver.ClickFirst("a", e => e.Text.Contains("Add Action"))
                .Click("a", e => e.GetAttribute("data-bind") == "click: $root.addEmailActionCommand")
                ;
            driver.Click("a", e => e.GetAttribute("data-bind") == "click : $root.startEditEmailAction")
                  .Value("[name=email-action-title]", "Aduan baru diterima")
                  .Value("[name=email-action-to]", "@Complaint.Tenant.Email")
                  .Value("[name=email-action-from]", "support@sph.gov.my")
                  .Value("[name=email-action-subject]", "Aduan baru diterima")
                  .Value("[name=email-action-body]", "Maklumat aduan anda adalah seperti berikut : ")
                ;
            driver.Value(".input-action-note", "Hantar email kepada penyewa")
                  .Click(".input-action-isactive")
                ;

            driver.Click("#save-button")
                .Sleep(2.Seconds());

            driver.NavigateToUrl("/#/trigger.list");
            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([TriggerId]) FROM [Sph].[Trigger]");
            Assert.IsTrue(max < latest);

            driver.Sleep(TimeSpan.FromSeconds(5), "Trigger baru berjaya ditambah");
            driver.Quit();
        }
    }
}
