using System;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;
using Bespoke.Sph.Templating;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace durandaljs.compiler.test
{
    [TestClass]
    public class RazorDisplayTemplateTestFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
        }


        [TestMethod]
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
            var html = compiler.GenerateDisplay(button);
            StringAssert.Contains(html,"display mode");
        }

        [TestMethod]
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
            var html = compiler.GenerateDisplay(checkbox);
            StringAssert.Contains(html,"<span");
        }

        [TestMethod]
        public void ChildEntityListView()
        {
            var listView = new ChildEntityListView
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
            listView.ViewColumnCollection.Add(new ViewColumn{Header = "Name",Path = "FullName"});
            var compiler = new ChildEntityListViewCompiler();
            var html = compiler.GenerateDisplay(listView);
            StringAssert.Contains(html,"<table");
            StringAssert.Contains(html,"<th>Name</th>");
            StringAssert.Contains(html,"text:FullName");
        }

        [TestMethod]
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
            dropdown.ComboBoxItemCollection.Add(new ComboBoxItem{Caption = "Male",Value = "Male"});
            dropdown.ComboBoxItemCollection.Add(new ComboBoxItem{Caption = "Female",Value = "Female"});
            var compiler = new ComboBoxCompiler();
            var html = compiler.GenerateDisplay(dropdown);
            StringAssert.Contains(html,"<span");
        }


        [TestMethod]
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
            var html = compiler.GenerateDisplay(dropdown);
            StringAssert.Contains(html,"money:");
        }

        [TestMethod]
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
            var html = compiler.GenerateDisplay(picker);
            StringAssert.Contains(html,"date:");
        }

        [TestMethod]
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
            var html = compiler.GenerateDisplay(picker);
            StringAssert.Contains(html, "date:");
        }


        [TestMethod]
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
            var html = compiler.GenerateDisplay(picker);
            StringAssert.Contains(html, "store");
        }
        [TestMethod]
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
            var html = compiler.GenerateDisplay(picker);
            StringAssert.Contains(html, "type=\"email\"");
        }

        [TestMethod]
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
            var html = compiler.GenerateDisplay(picker);
            StringAssert.Contains(html, "entity: 'State'");
        }

        [TestMethod]
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
            var html = compiler.GenerateDisplay(picker);
            StringAssert.Contains(html, "Upload");
        }
        [TestMethod]
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
            var html = compiler.GenerateDisplay(picker);
            StringAssert.Contains(html, "html");
        }
        [TestMethod]
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
            };

            var compiler = new ImageElementCompiler();
            var html = compiler.GenerateDisplay(picker);
            StringAssert.Contains(html, "store");
        }
        [TestMethod]
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
            var html = compiler.GenerateDisplay(lv);
            StringAssert.Contains(html, "<table");
        }
        [TestMethod]
        public void NumberTextBox()
        {
            var lv = new NumberTextBox
            {
                Label = "Download",
                Name = "dobTextBox",
                ElementId = "dobTextBox",
                Path = "Dob",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The date",
                Visible = "true",
            };

            var compiler = new NumberTextBoxCompiler();
            var html = compiler.GenerateDisplay(lv);
            StringAssert.Contains(html, "<input");
        }
        [TestMethod]
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
            };

            var compiler = new SectionFormElementCompiler();
            var html = compiler.GenerateDisplay(lv);
            StringAssert.Contains(html, "<h2");
        }
        [TestMethod]
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
            };

            var compiler = new TextAreaElementCompiler();
            var html = compiler.GenerateDisplay(lv);
            StringAssert.Contains(html, "<textarea");
        }
        [TestMethod]
        public void TextBox()
        {
            var lv = new TextBox
            {
                Label = "Download",
                Name = "dobTextBox",
                ElementId = "dobTextBox",
                Path = "Dob",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The date",
                Visible = "true",
            };

            var compiler = new TextBoxCompiler();
            var html = compiler.GenerateDisplay(lv);
            StringAssert.Contains(html, "text");
        }
        [TestMethod]
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
            };

            var compiler = new WebsiteFormElementCompiler();
            var html = compiler.GenerateDisplay(lv);
            StringAssert.Contains(html, "url");
        }
    }
}