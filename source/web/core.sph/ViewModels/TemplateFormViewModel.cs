using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.ViewModels
{
    public class TemplateFormViewModel
    {
        public TemplateFormViewModel()
        {
            this.FormElements.Add(new TextBox { Name = "Single Line Text", ToolboxIconClass = "fa fa-text-width" });
            this.FormElements.Add(new Button { Name = "Button", ToolboxIconClass = "fa fa-square" });
            this.FormElements.Add(new ComboBox { Name = "Select List", ToolboxIconClass = "fa fa-chevron-down" });
            this.FormElements.Add(new TextAreaElement{Name = "Paragrapah text", ToolboxIconClass = "fa fa-desktop"});
            this.FormElements.Add(new CheckBox { Name = "Checkboxes", ToolboxIconClass = "fa fa-check" });
            this.FormElements.Add(new DatePicker { Name = "Date", ToolboxIconClass = "fa fa-calendar" });
            this.FormElements.Add(new DateTimePicker { Name = "DateTime", ToolboxIconClass = "fa fa-clock-o" });
            this.FormElements.Add(new ListView { Name = "List", ToolboxIconClass = "fa fa-list-ul" });
            this.FormElements.Add(new ChildEntityListView { Name = "Child Entity Table", ToolboxIconClass = "fa fa-list-alt" });
            this.FormElements.Add(new EntityLookupElement { Name = "Entity Lookup", ToolboxIconClass = "fa fa-search" });
            this.FormElements.Add(new FileUploadElement { Name = "Upload file", ToolboxIconClass = "fa fa-cloud-upload" });
            this.FormElements.Add(new DownloadLink { Name = "Download file", ToolboxIconClass = "fa fa-cloud-download" });
            this.FormElements.Add(new ImageElement { Name = "Image", ToolboxIconClass = "fa fa-picture-o" });

            this.FormElements.Add(new WebsiteFormElement { Name = "Url", ToolboxIconClass = "fa fa-link" });
            this.FormElements.Add(new EmailFormElement { Name = "Email", ToolboxIconClass = "fa fa-envelope" });
            this.FormElements.Add(new NumberTextBox { Name = "Number", ToolboxIconClass = "fa fa-xing" });
            this.FormElements.Add(new FormElement{Name = "", ToolboxIconClass = ""});
            this.FormElements.Add(new SectionFormElement { Name = "HTML Section", ToolboxIconClass = "fa fa-bold" });
            this.FormElements.Add(new HtmlElement { Name = "HTML rich", ToolboxIconClass = "fa fa-html5" });

        }
        private readonly ObjectCollection<FormElement> m_nameCollection = new ObjectCollection<FormElement>();
        public ObjectCollection<FormElement> FormElements
        {
            get { return m_nameCollection; }
        }

        public bool IsImportVisible { get; set; }
    }
}