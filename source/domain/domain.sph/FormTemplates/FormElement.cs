using System.Xml.Serialization;
using Newtonsoft.Json;


namespace Bespoke.Sph.Domain
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
    [XmlInclude(typeof(SpaceUnitElement))]
    [XmlInclude(typeof(HtmlElement))]
    [XmlInclude(typeof(CustomListDefinitionElement))]
    [XmlInclude(typeof(MaintenanceOfficerElement))]
    [XmlInclude(typeof(BuildingBlocksElement))]
    [XmlInclude(typeof(SpaceFeaturesElement))]
    public partial class FormElement : DomainObject
    {
        public virtual CustomField GenerateCustomField()
        {
            throw new System.NotImplementedException(this.GetType().Name + " does not implement GenerateCustomField");
        }

        public virtual string GetKnockoutBindingExpression()
        {
            return null;
        }
        public virtual string GetKnockoutDisplayBindingExpression()
        {
       
            var path = this.Path;
            if (null != this.CustomField)
                path = string.Format("CustomField('{0}')", this.Path);

            return string.Format("text: {0}", path);
        
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

        [JsonIgnore]
        [XmlIgnore]
        public string LabelCssClass
        {
            get
            {
                return string.Format("control-label col-lg-{0}", this.LabelColumnSpan);
            }
        }
        [JsonIgnore]
        [XmlIgnore]
        public string InputPanelCssClass
        {
            get
            {
                return string.Format("col-lg-{0}", this.InputColumnSpan);
            }
        }

        
    }
}