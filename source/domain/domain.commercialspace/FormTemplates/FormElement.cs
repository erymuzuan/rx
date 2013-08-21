using System;
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

        public CustomField CustomField { get; set; }

        protected override void OnPropertyChanged(string propertyName = null)
        {
            Console.WriteLine(propertyName);
            base.OnPropertyChanged(propertyName);
        }
    }
}