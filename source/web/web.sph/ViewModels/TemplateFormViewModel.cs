using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.ViewModels
{
    public class TemplateFormViewModel
    {
        public TemplateFormViewModel()
        {
            this.FormElements.Add(new FormElement());
            this.FormElements.Add(new SectionFormElement());
            this.FormElements.Add(new HtmlElement());
            this.FormElements.Add(new TextBox());
            this.FormElements.Add(new ComboBox());
            this.FormElements.Add(new WebsiteFormElement());
            this.FormElements.Add(new EmailFormElement());
            this.FormElements.Add(new NumberTextBox());
            this.FormElements.Add(new CheckBox());
            this.FormElements.Add(new TextAreaElement());
            this.FormElements.Add(new DatePicker());
            this.FormElements.Add(new CustomListDefinitionElement());

        }
        private readonly ObjectCollection<FormElement> m_nameCollection = new ObjectCollection<FormElement>();
        public string Entity { get; set; }
        public ObjectCollection<FormElement> FormElements
        {
            get { return m_nameCollection; }
        }
    }
}