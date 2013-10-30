﻿using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.ViewModels
{
    public class TemplateFormViewModel
    {
        public TemplateFormViewModel()
        {
            this.FormElements.Add(new FormElement{Name = "", CssClass = ""});
            this.FormElements.Add(new SectionFormElement { Name = "HTML Section", CssClass = "fa fa-reorder" });
            this.FormElements.Add(new HtmlElement { Name = "HTML rich", CssClass = "fa fa-html5" });
            this.FormElements.Add(new TextBox { Name = "Single Line Text", CssClass = "fa fa-text-width" });
            this.FormElements.Add(new ComboBox { Name = "Paragrapah text", CssClass = "fa fa-chevron-down" });
            this.FormElements.Add(new WebsiteFormElement { Name = "Url", CssClass = "fa fa-link" });
            this.FormElements.Add(new EmailFormElement { Name = "Email", CssClass = "fa fa-envelope" });
            this.FormElements.Add(new NumberTextBox { Name = "Number", CssClass = "fa fa-xing" });
            this.FormElements.Add(new CheckBox { Name = "Checkboxes", CssClass = "fa fa-check" });
            this.FormElements.Add(new TextAreaElement{Name = "Paragrapah text", CssClass = "fa fa-desktop"});
            this.FormElements.Add(new DatePicker { Name = "Date", CssClass = "fa fa-calendar" });
            this.FormElements.Add(new CustomListDefinitionElement{Name = "", CssClass = ""});

        }
        private readonly ObjectCollection<FormElement> m_nameCollection = new ObjectCollection<FormElement>();
        public string Entity { get; set; }
        public ObjectCollection<FormElement> FormElements
        {
            get { return m_nameCollection; }
        }
    }
}