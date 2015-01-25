using System;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs.FormElements;
using Bespoke.Sph.Templating;
using NUnit.Framework;

namespace durandaljs.compiler.test
{
    [TestFixture]
    public class ScreenActivityRazorEditorTemplateTestFixture
    {
        private WorkflowDefinition m_wd;

        [TestFixtureSetUp]
        public void Init()
        {
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
            m_wd = new WorkflowDefinition
            {
                Id = "simple-wf",
                Name = "Simple Workflow",
                WebId = "simple-webid"
            };
            var start = new ExpressionActivity { IsInitiator = true, WebId = "FireMe", Name = "FireMe", NextActivityWebId = "b" };
            var end = new EndActivity { Name = "End", WebId = "End" };
            m_wd.ActivityCollection.Add(start);
            m_wd.ActivityCollection.Add(end);

        }


        [Test]
        public void Button()
        {
            var button = new Button
            {
                Label = "Click Me",
                CommandName = "Register",
                Name = "clickMeButton",
                ElementId = "clickMeButton",
                Path = ".",
                WebId = "guid",
                Tooltip = "Click me to find out",
                Visible = "true",
                Enable = "false"
            };
            var compiler = new ButtonCompiler();
            var html = compiler.GenerateEditor(button, m_wd);
            StringAssert.Contains("<button", html);
        }

        [Test]
        public void CheckBox()
        {
            var checkbox = new CheckBox
            {
                Label = "Click Me",
                Name = "clickMeButton",
                ElementId = "clickMeButton",
                Path = "IsSomething",
                WebId = "guid",
                Tooltip = "Click me to find out",
                Visible = "true",
                Enable = "false"
            };
            var compiler = new CheckBoxCompiler();
            var html = compiler.GenerateEditor(checkbox, m_wd);
            StringAssert.Contains("checked : IsSomething", html);
        }

        [Test]
        public void ChildEntityListView()
        {
            var checkbox = new ChildEntityListView
            {
                Label = "State list",
                Name = "clickMeButton",
                ElementId = "clickMeButton",
                Path = "IsSomething",
                WebId = "guid",
                Tooltip = "Click me to find out",
                Visible = "true",
                Enable = "false",
                Entity = "State",
                Query = "status eq 'ok'"
            };
            var compiler = new ChildEntityListViewCompiler();
            var html = compiler.GenerateEditor(checkbox, m_wd);
            StringAssert.Contains("<table", html);
        }

        [Test]
        public void ComboBox()
        {
            var dropdown = new ComboBox
            {
                Label = "Gender",
                Name = "genderSelect",
                ElementId = "genderSelect",
                Path = "Gender",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "select your preferred gender",
                Visible = "true",
                Enable = "false",
            };
            dropdown.ComboBoxItemCollection.Add(new ComboBoxItem { Caption = "Male", Value = "Male" });
            dropdown.ComboBoxItemCollection.Add(new ComboBoxItem { Caption = "Female", Value = "Female" });
            var compiler = new ComboBoxCompiler();
            var html = compiler.GenerateEditor(dropdown, m_wd);
            StringAssert.Contains("<option", html);
        }


        [Test]
        public void CurrencyElement()
        {
            var dropdown = new CurrencyElement
            {
                Label = "Amount",
                Name = "amountTextBox",
                ElementId = "amountTextBox",
                Path = "Amount",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The amount",
                Visible = "true",
                Enable = "false",
            };

            var compiler = new CurrencyElementCompiler();
            var html = compiler.GenerateEditor(dropdown, m_wd);
            StringAssert.Contains("money:", html);
        }

        [Test]
        public void DatePicker()
        {
            var picker = new DatePicker
            {
                Label = "Dob",
                Name = "dobTextBox",
                ElementId = "dobTextBox",
                Path = "Dob",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The date",
                Visible = "true",
                Enable = "false",
            };

            var compiler = new DatePickerCompiler();
            var html = compiler.GenerateEditor(picker, m_wd);
            StringAssert.Contains("kendoDate:", html);
        }
        [Test]
        public void DateTimePicker()
        {
            var picker = new DateTimePicker
            {
                Label = "Dob",
                Name = "dobTextBox",
                ElementId = "dobTextBox",
                Path = "Dob",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The date",
                Visible = "true",
                Enable = "false",
            };

            var compiler = new DateTimePickerCompiler();
            var html = compiler.GenerateEditor(picker, m_wd);
            StringAssert.Contains("kendoDateTime:", html);
        }


        [Test]
        public void DownloadLink()
        {
            var picker = new DownloadLink
            {
                Label = "Download",
                Name = "dobTextBox",
                ElementId = "dobTextBox",
                Path = "Dob",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The date",
                Visible = "true",
                Enable = "false",
            };

            var compiler = new DownloadLinkCompiler();
            var html = compiler.GenerateEditor(picker, m_wd);
            StringAssert.Contains("store", html);
        }
        [Test]
        public void EmailFormElement()
        {
            var picker = new EmailFormElement
            {
                Label = "Download",
                Name = "dobTextBox",
                ElementId = "dobTextBox",
                Path = "Dob",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The date",
                Visible = "true",
                Enable = "false",
            };

            var compiler = new EmailFormElementCompiler();
            var html = compiler.GenerateEditor(picker, m_wd);
            StringAssert.Contains("type=\"email\"", html);
        }

        [Test]
        public void EntityLookupElement()
        {
            var picker = new EntityLookupElement
            {
                Label = "Download",
                Name = "dobTextBox",
                ElementId = "dobTextBox",
                Path = "Dob",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The date",
                Visible = "true",
                Enable = "false",
                Entity = "State"
            };

            var compiler = new EntityLookupElementCompiler();
            var html = compiler.GenerateEditor(picker, m_wd);
            StringAssert.Contains("entity: 'State'", html);
        }

        [Test]
        public void FileUploadElement()
        {
            var picker = new FileUploadElement
            {
                Label = "Download",
                Name = "dobTextBox",
                ElementId = "dobTextBox",
                Path = "Dob",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The date",
                Visible = "true",
                Enable = "false"
            };

            var compiler = new FileUploadElementCompiler();
            var html = compiler.GenerateEditor(picker, m_wd);
            StringAssert.Contains("Upload", html);
        }
        [Test]
        public void HtmlElement()
        {
            var picker = new HtmlElement
            {
                Label = "Download",
                Name = "dobTextBox",
                ElementId = "dobTextBox",
                Path = "Dob",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The date",
                Visible = "true",
                Enable = "false",
                Text = "<thi is my html>"
            };

            var compiler = new HtmlElementCompiler();
            var html = compiler.GenerateEditor(picker, m_wd);
            StringAssert.Contains("html", html);
        }
        [Test]
        public void ImageElement()
        {
            var picker = new ImageElement
            {
                Label = "Download",
                Name = "dobTextBox",
                ElementId = "dobTextBox",
                Path = "Dob",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The date",
                Visible = "true",
                Enable = "true"
            };

            var compiler = new ImageElementCompiler();
            var html = compiler.GenerateEditor(picker, m_wd);
            StringAssert.Contains("store", html);
        }
        [Test]
        public void ListView()
        {
            var lv = new ListView
            {
                Label = "Download",
                Name = "dobTextBox",
                ElementId = "dobTextBox",
                Path = "Dob",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The date",
                Visible = "true",
                Enable = "true"
            };
            lv.ListViewColumnCollection.Add(new ListViewColumn
            {
                Label = "Test",
                Path = "Name",
                Input = new TextBox
                {
                    Path = "Test"
                }
            });

            var compiler = new ListViewCompiler();
            var html = compiler.GenerateEditor(lv, m_wd);
            StringAssert.Contains("<table", html);
        }
        [Test]
        public void NumberTextBox()
        {
            var nt = new NumberTextBox
            {
                Label = "Download",
                Name = "dobTextBox",
                ElementId = "dobTextBox",
                Path = "Dob",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The date",
                Visible = "true",
                Enable = "true"
            };

            var compiler = new NumberTextBoxCompiler();
            var html = compiler.GenerateEditor(nt, m_wd);
            StringAssert.Contains("<input", html);
        }
        [Test]
        public void SectionFormElement()
        {
            var lv = new SectionFormElement
            {
                Label = "Download",
                Name = "dobTextBox",
                ElementId = "dobTextBox",
                Path = "Dob",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The date",
                Visible = "true",
                Enable = "true"
            };

            var compiler = new SectionFormElementCompiler();
            var html = compiler.GenerateEditor(lv, m_wd);
            StringAssert.Contains("<h2", html);
        }
        [Test]
        public void TextAreaElement()
        {
            var lv = new TextAreaElement
            {
                Label = "Download",
                Name = "dobTextBox",
                ElementId = "dobTextBox",
                Path = "Dob",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The date",
                Visible = "true",
                Enable = "true"
            };

            var compiler = new TextAreaElementCompiler();
            var html = compiler.GenerateEditor(lv, m_wd);
            StringAssert.Contains("<textarea", html);
        }
        [Test]
        public void TextBox()
        {
            var text = new TextBox
            {
                Label = "Download",
                Name = "dobTextBox",
                ElementId = "dobTextBox",
                Path = "Dob",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The date",
                Visible = "true",
                Enable = "true"
            };

            var compiler = new TextBoxCompiler();
            var html = compiler.GenerateEditor(text, m_wd);
            StringAssert.Contains("text", html);
        }
        [Test]
        public void WebsiteFormElement()
        {
            var lv = new WebsiteFormElement
            {
                Label = "Download",
                Name = "dobTextBox",
                ElementId = "dobTextBox",
                Path = "Dob",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The date",
                Visible = "true",
                Enable = "true"
            };

            var compiler = new WebsiteFormElementCompiler();
            var html = compiler.GenerateEditor(lv, m_wd);
            StringAssert.Contains("url", html);
        }
    }
}