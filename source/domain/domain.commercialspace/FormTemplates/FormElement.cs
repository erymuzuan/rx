using System.Xml.Serialization;


namespace Bespoke.SphCommercialSpaces.Domain
{
    [XmlInclude(typeof(AddressElement))]
    [XmlInclude(typeof(TextAreaElement))]
    [XmlInclude(typeof(TextBox))]
    [XmlInclude(typeof(NumberTextBox))]
    [XmlInclude(typeof(DatePicker))]
    [XmlInclude(typeof(CheckBox))]
    [XmlInclude(typeof(ComboBox))]
    [XmlInclude(typeof(BuildingFloorsElement))]
    [XmlInclude(typeof(BuildingMapElement))]
    [XmlInclude(typeof(EmailFormElement))]
    [XmlInclude(typeof(WebsiteFormElement))]
    public partial class FormElement : DomainObject
    {
        public virtual CustomField GenerateCustomField()
        {
            throw new System.NotImplementedException();
        }

        public CustomField CustomField { get; set; }
    }
}