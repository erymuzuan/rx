using System.Xml.Serialization;
using Newtonsoft.Json;


namespace Bespoke.Sph.Domain
{
    [XmlInclude(typeof(AddressElement))]
    [XmlInclude(typeof(TextAreaElement))]
    [XmlInclude(typeof(TextBox))]
    [XmlInclude(typeof(NumberTextBox))]
    [XmlInclude(typeof(DatePicker))]
    [XmlInclude(typeof(DateTimePicker))]
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
    [XmlInclude(typeof(FileUploadElement))]
    [XmlInclude(typeof(ListView))]
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
                var css = string.Format("col-lg-{0}", this.LabelColLg ?? 4);
                if (this.LabelColMd.HasValue) css += " col-md-" + this.LabelColMd;
                if (this.LabelColSm.HasValue) css += " col-sm-" + this.LabelColSm;
                if (this.LabelColXs.HasValue) css += " col-xs-" + this.LabelColXs;
                return css;
            }
        }
        [JsonIgnore]
        [XmlIgnore]
        public string InputPanelCssClass
        {
            get
            {
                var css = string.Format("col-lg-{0}", this.InputColLg ?? 8);
                if (this.InputColMd.HasValue) css += " col-md-" + this.InputColMd;
                if (this.InputColSm.HasValue) css += " col-sm-" + this.InputColSm;
                if (this.InputColXs.HasValue) css += " col-xs-" + this.InputColXs;
                return css;
            }
        }

        [JsonIgnore]
        [XmlIgnore]
        public virtual bool IsPathIsRequired { get { return true; } }

        [JsonIgnore]
        [XmlIgnore]
        public bool IsCompact { get; set; }


        public virtual string GenerateMarkup()
        {
            return string.Empty;
        }

        public void SetDefaultLayout(FormDesign formDesign)
        {
            var fe = this;
            fe.LabelColLg = fe.LabelColLg ?? formDesign.LabelColLg;
            fe.LabelColMd = fe.LabelColMd ?? formDesign.LabelColMd;
            fe.LabelColSm = fe.LabelColSm ?? formDesign.LabelColSm;
            fe.LabelColXs = fe.LabelColXs ?? formDesign.LabelColXs;

            fe.InputColLg = fe.InputColLg ?? formDesign.InputColLg;
            fe.InputColMd = fe.InputColMd ?? formDesign.InputColMd;
            fe.InputColSm = fe.InputColSm ?? formDesign.InputColSm;
            fe.InputColXs = fe.InputColXs ?? formDesign.InputColXs;


        }

        public virtual BuildError[] ValidateBuild(WorkflowDefinition wd, ScreenActivity screen)
        {
            return new BuildError[]{};
        }
    }
}