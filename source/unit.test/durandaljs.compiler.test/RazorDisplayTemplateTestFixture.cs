using System;
using System.Collections.Generic;
using Bespoke.Sph.Domain;
using Bespoke.Sph.FormCompilers.DurandalJs;
using Bespoke.Sph.Templating;
using NUnit.Framework;


namespace durandaljs.compiler.test
{
    [TestFixture]
    public class RazorDisplayTemplateTestFixture
    {
        private EntityDefinition m_patient;
        [TestFixtureSetUp]
        public void SetUp()
        {
            ObjectBuilder.AddCacheList<IDirectoryService>(new MockDirectoryService());
            ObjectBuilder.AddCacheList<ITemplateEngine>(new RazorEngine());
            m_patient = new EntityDefinition
            {
                Id = "patient",
                Name = "Patient",
                WebId = "patient-webid",
                Plural = "Patients",
                RecordName = "Mrn"
            };
            m_patient.MemberCollection.Add(new Member { Name = "Name", Type = typeof(string), IsNullable = true });
            m_patient.MemberCollection.Add(new Member { Name = "Mrn", Type = typeof(string), IsNullable = false });
            m_patient.MemberCollection.Add(new Member { Name = "MyKad", Type = typeof(string), IsNullable = true });
            m_patient.MemberCollection.Add(new Member { Name = "Age", Type = typeof(int), IsNullable = true });
            m_patient.MemberCollection.Add(new Member { Name = "Dob", Type = typeof(DateTime), IsNullable = true });
            m_patient.MemberCollection.Add(new Member { Name = "RegisteredDate", Type = typeof(DateTime), IsNullable = false });
            m_patient.MemberCollection.Add(new Member { Name = "IsMarried", Type = typeof(bool), IsNullable = false });

            var address = new Member { Name = "Address", Type = typeof(object) };
            address.Add(new Dictionary<string, Type>
            {
                {"Street",typeof(string)},
                {"Street2",typeof(string)},
                {"City",typeof(string)},
                {"Postcode",typeof(string)},
                {"State",typeof(string)}
            });
            m_patient.MemberCollection.Add(address);
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
            var html = compiler.GenerateDisplay(button, m_patient);
            StringAssert.Contains("display mode", html);
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
            var html = compiler.GenerateDisplay(checkbox, m_patient);
            StringAssert.Contains("<span", html);
        }

        [Test]
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
            listView.ViewColumnCollection.Add(new ViewColumn { Header = "Name", Path = "FullName" });
            var compiler = new ChildEntityListViewCompiler();
            var html = compiler.GenerateDisplay(listView, m_patient);
            StringAssert.Contains("<table", html);
            StringAssert.Contains("<th>Name</th>", html);
            StringAssert.Contains("text:FullName", html);
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
            var html = compiler.GenerateDisplay(dropdown, m_patient);
            StringAssert.Contains("<span", html);
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
            var html = compiler.GenerateDisplay(dropdown, m_patient);
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
            var html = compiler.GenerateDisplay(picker, m_patient);
            StringAssert.Contains("date:", html);
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
            var html = compiler.GenerateDisplay(picker, m_patient);
            StringAssert.Contains("date:", html);
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
            var html = compiler.GenerateDisplay(picker, m_patient);
            StringAssert.Contains("store", html);
        }
        [Test]
        public void EmailFormElement()
        {
            var picker = new EmailFormElement
            {
                Label = "Email",
                Name = "dobTextBox",
                ElementId = "emailBox",
                Path = "Email",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The date",
                Visible = "true",
                Enable = "false",
            };

            var compiler = new EmailFormElementCompiler();
            var html = compiler.GenerateDisplay(picker, m_patient);
            StringAssert.Contains("text: Email", html);
        }

        [Test]
        [Ignore]
        public void EntityLookupElement()
        {
            var picker = new EntityLookupElement
            {
                Label = "Districts",
                Name = "districtLookup",
                ElementId = "districtLookup",
                Path = "District",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The district",
                Visible = "true",
                Enable = "false",
                Entity = "District"
            };

            var compiler = new EntityLookupElementCompiler();
            var html = compiler.GenerateDisplay(picker, m_patient);
            StringAssert.Contains("entity: 'District'", html);
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
            var html = compiler.GenerateDisplay(picker, m_patient);
            StringAssert.Contains("Upload", html);
        }
        [Test]
        public void HtmlElement()
        {
            var raw = new HtmlElement
            {
                Label = "Custom object",
                Name = "rawTextBox",
                ElementId = "rawTextBox",
                Path = "Raw",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The raw",
                Visible = "true",
                Enable = "false",
                Text = "<this is my html>"
            };

            var compiler = new HtmlElementCompiler();
            var html = compiler.GenerateDisplay(raw, m_patient);
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
            };

            var compiler = new ImageElementCompiler();
            var html = compiler.GenerateDisplay(picker, m_patient);
            StringAssert.Contains("store", html);
        }
        [Test]
        public void ListView()
        {
            var lv = new ListView
            {
                Label = "Children",
                Name = "itemListView",
                ElementId = "itemListView",
                Path = "items",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "Your children list",
                Visible = "true",
                Enable = "true"
            };
            lv.ListViewColumnCollection.Add(new ListViewColumn
            {
                Label = "Name",
                Path = "Name",
                Input = new TextBox
                {
                    Path = "Name"
                }
            });
            lv.ListViewColumnCollection.Add(new ListViewColumn
            {
                Label = "Age",
                Path = "Age",
                Input = new TextBox
                {
                    Path = "Age"
                }
            });
            lv.ListViewColumnCollection.Add(new ListViewColumn
            {
                Label = "Photo",
                Path = "Photo",
                Input = new ImageElement
                {
                    Path = "Photo",
                    Enable = "true",
                    Visible = "true"
                }
            });
            lv.ListViewColumnCollection.Add(new ListViewColumn
            {
                Label = "Document",
                Path = "Document",
                Input = new DownloadLink
                {
                    Path = "Document",
                    Enable = "true",
                    Visible = "true"
                }
            });

            var compiler = new ListViewCompiler();
            var html = compiler.GenerateDisplay(lv, m_patient);
            StringAssert.Contains("<table", html);
            StringAssert.Contains("binarystore", html);
        }

        [Test]
        public void NumberTextBox()
        {
            var lv = new NumberTextBox
            {
                Label = "Amount",
                Name = "amountBox",
                ElementId = "amountBox",
                Path = "Amount",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "The amount",
                Visible = "true",
                Enable = "true"
            };

            var compiler = new NumberTextBoxCompiler();
            var html = compiler.GenerateDisplay(lv, m_patient);
            StringAssert.Contains("text: Amount", html);
        }
        [Test]
        public void SectionFormElement()
        {
            var lv = new SectionFormElement
            {
                Label = "Address",
                Name = "addressSection",
                ElementId = "addressSection",
                Path = ".",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "",
                Visible = "true",
                Enable = "trur"
            };

            var compiler = new SectionFormElementCompiler();
            var html = compiler.GenerateDisplay(lv, m_patient);
            StringAssert.Contains("<h2", html);
        }
        [Test]
        public void TextAreaElement()
        {
            var text = new TextAreaElement
            {
                Label = "Summary",
                Name = "summaryTextBox",
                ElementId = "summaryTextBox",
                Path = "Summary",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "Short descriptions",
                Visible = "true",
                Enable = "true"
            };

            var compiler = new TextAreaElementCompiler();
            var html = compiler.GenerateDisplay(text, m_patient);
            StringAssert.Contains("<p data-bind=\"text: Summary\"", html);
        }
        [Test]
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
            var html = compiler.GenerateDisplay(lv, m_patient);
            StringAssert.Contains("text", html);
        }
        [Test]
        public void WebsiteFormElement()
        {
            var lv = new WebsiteFormElement
            {
                Label = "Url",
                Name = "urlBox",
                ElementId = "urlBox",
                Path = "Url",
                WebId = Guid.NewGuid().ToString(),
                Tooltip = "Website address",
                Visible = "true",
                Enable = "true"
            };

            var compiler = new WebsiteFormElementCompiler();
            var html = compiler.GenerateDisplay(lv, m_patient);
            StringAssert.Contains("text: Url", html);
        }
    }
}