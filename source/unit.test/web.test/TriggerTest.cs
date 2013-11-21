using System;
using System.Data.SqlClient;
using Humanizer;
using NUnit.Framework;

namespace web.test
{
    [TestFixture]
    public class TriggerTest : BrowserTest
    {
        public const string EMAIL_TRIGGER_NAME = "Email Trigger (TEST)";
        public const string SETTER_TRIGGER_NAME = "Setter Trigger (TEST)";
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
        public void _001_AddNewEmailTrigger()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[Trigger] WHERE [Name]=@TriggerName", new SqlParameter("@TriggerName", EMAIL_TRIGGER_NAME));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([TriggerId]) FROM [Sph].[Trigger]");
            var driver = this.InitiateDriver();

            driver.NavigateToUrl("/Account/Logoff")
                .Login(m_admintrigger)
                .NavigateToUrl("/#/trigger.setup/0")
                .Sleep(3.Seconds());

            driver.Value("[name=Name]", EMAIL_TRIGGER_NAME);
            driver.SelectOption("[name=Entity]", "Aduan");
            driver.Value("[name=Note]", "Trigger email kepada penyewa atau orang awam yang hantar aduan")
              .Click("[name=IsActive]");

            //add rules - document and constant field
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
                  .ClickFirst("input", e => e.GetAttribute("data-bind") =="click: $root.saveEmail")
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

        [Test]
        public void _002_AddNewSetterTrigger()
        {
            this.ExecuteNonQuery("DELETE FROM [Sph].[Trigger] WHERE [Name]=@TriggerName", new SqlParameter("@TriggerName", SETTER_TRIGGER_NAME));
            var max = this.GetDatabaseScalarValue<int>("SELECT MAX([TriggerId]) FROM [Sph].[Trigger]");
            var driver = this.InitiateDriver();

            driver.NavigateToUrl("/Account/Logoff")
                .Login(m_admintrigger)
                .NavigateToUrl("/#/trigger.setup/0")
                .Sleep(3.Seconds());

            driver.Value("[name=Name]", SETTER_TRIGGER_NAME);
            driver.SelectOption("[name=Entity]", "Aduan");
            driver.Value("[name=Note]", "Kemaskini maklumat jabatan kepada Senggaraan jika aduan jenis kerosakan dihantar")
              .Click("[name=IsActive]")
              .Click("[name=IsFiredOnAdded]")
              .Value("[name=FireOnOperations]",".added")
              ;

            //add rules - document and constant field
            driver.Click("a", e => e.GetAttribute("data-bind") == "click: $parent.addRuleCommand")
                .Click("a", e => e.GetAttribute("data-bind") == "with : Left")
                  .Click("a", e => e.Text.Contains("Document Field"))
                  .Sleep(2.Seconds())
                ;

            driver.Value("[name=doc-field-name]", "Jenis Aduan")
                .Value("[name=doc-field-note]", "Jenis Aduan")
                .Value("[name=doc-field-path]", "Type")
                .ClickFirst("input", e => e.GetAttribute("data-bind") == "click: $root.saveField")
                ;

            driver.SelectOption("[name=rule-operator]", "Mengandungi")
                  .Click("a", e => e.GetAttribute("data-bind") == "with : Right")
                   .Click("a", e => e.Text.Contains("Constant Field"))
                  .Sleep(2.Seconds())
                  .Value("[name=constant-field-name]", "Kerosakan")
                  .Value("[name=constant-field-note]", "Kerosakan")
                  .Value("[name=constant-field-typename]", "Teks")
                  .Value("[name=constant-field-value]", "Kerosakan");
            driver.ClickFirst("button", e => e.GetAttribute("data-bind") == "click: $root.saveField")
            ;
            driver.Sleep(TimeSpan.FromSeconds(3));


            // add setter trigger action
            driver.ClickFirst("a", e => e.Text.Contains("Add Action"))
                .Click("a", e => e.GetAttribute("data-bind") == "click: $root.addSetterActionCommand")
                ;
            driver.Click("a", e => e.GetAttribute("data-bind") == "click : $root.startEditSetterAction")
                  .Value("[name=setter-action-title]", "Kemaskini jabatan")
                  .Value("[name=setter-action-note]", "Kemaskini jabatan kepada Senggaraan apabila aduan jenis kerosakan dihantar ke sph")
                  .ClickFirst("a", e => e.GetAttribute("data-bind") == "click: $root.addSetterActionChild")
                ;
            driver.Value(".input-setter-path", "Department")
                   .Click("a", e => e.GetAttribute("data-bind") == "with : Field")
                    .Click("a", e => e.Text.Contains("Constant Field"))
                  .Sleep(2.Seconds())
                ;

            driver.Value("[name=constant-field-name]", "Senggaraan")
                .Value("[name=constant-field-note]","Jabatan Senggaraan")
                .Value("[name=constant-field-typename]", "Teks")
                .Value("[name=constant-field-value]", "Senggaraan")
                .ClickFirst("a", e => e.GetAttribute("data-bind") == "click: saveSetter")
                ;

            driver.Value(".input-action-note", "Kemaskini kepada Jabatan Senggaraan")
                  .Click(".input-action-isactive")
                ;
            driver.Click("#save-button")
                .Sleep(3.Seconds());
            driver.NavigateToUrl("/#/trigger.list");
            var latest = this.GetDatabaseScalarValue<int>("SELECT MAX([TriggerId]) FROM [Sph].[Trigger]");
            Assert.IsTrue(max < latest);

            driver.Sleep(TimeSpan.FromSeconds(5), "Trigger baru berjaya ditambah");
            driver.Quit();
        }
    }
}
