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
    [XmlInclude(typeof(BuildingElement))]
    [XmlInclude(typeof(EmailFormElement))]
    [XmlInclude(typeof(WebsiteFormElement))]
    [XmlInclude(typeof(SectionFormElement))]
    [XmlInclude(typeof(ComplaintCategoryElement))]
    [XmlInclude(typeof(RentalApplicationBanksElement))]
    [XmlInclude(typeof(RentalApplicationAttachmentsElement))]
    [XmlInclude(typeof(RentalApplicationContactElement))]
    [XmlInclude(typeof(CommercialSpaceLotsElement))]
    [XmlInclude(typeof(HtmlElement))]
    [XmlInclude(typeof(CustomListDefinitionElement))]
    public partial class FormElement : DomainObject
    {
        public virtual CustomField GenerateCustomField()
        {
            throw new System.NotImplementedException();
        }

        public virtual string GetKnockoutBindingExpression()
        {
            return null;
        }

        public string GetNormalizedName()
        {
            if (string.IsNullOrWhiteSpace(this.Path)) return this.ElementId;
            if (this.Path.StartsWith("CustomField"))
            {
                const string pattern = @"CustomField\('(?<field>.*?)'\)";
                var customField = Strings.RegexSingleValue(this.Path, pattern, "field");
                return customField;
            }

            return this.Path;
        }

        public CustomField CustomField { get; set; }

        
    }
}