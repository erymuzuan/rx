using System.Drawing;
using System.Xml.Serialization;
using Newtonsoft.Json;


namespace Bespoke.Sph.Domain
{
    public partial class FormElement : DomainObject
    {
        public virtual string GetKnockoutBindingExpression()
        {
            return null;
        }
        public virtual string GetKnockoutDisplayBindingExpression()
        {
            var path = this.Path;
            return $"text: {path}";
        }

        public string GetNormalizedName()
        {
            if (string.IsNullOrWhiteSpace(this.Path)) return this.ElementId;
            return this.Path;
        }

        [JsonIgnore]
        [XmlIgnore]
        public string LabelCssClass
        {
            get
            {
                var css = $"col-md-{this.LabelColMd ?? 4}";
                if (this.LabelColMd.HasValue) css += " col-lg-" + this.LabelColLg;
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
                var css = $"col-md-{this.InputColMd ?? 8}";
                if (this.InputColMd.HasValue) css += " col-lg-" + this.InputColLg;
                if (this.InputColSm.HasValue) css += " col-sm-" + this.InputColSm;
                if (this.InputColXs.HasValue) css += " col-xs-" + this.InputColXs;
                return css;
            }
        }

        [XmlIgnore]
        public virtual bool IsPathIsRequired => true;

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
        public virtual BuildError[] ValidateBuild(EntityDefinition ed)
        {
            return new BuildError[]{};
        }

        public virtual Bitmap GetPngIcon()
        {
            return null;
        }

        public string GetEditorViewModel()
        {
            return "";
        }

        public string GetEditorView()
        {
            return "";
        }

        /// <summary>
        /// The unique typename for each activity, should be overriden if you wish to have different name to avoid conflict
        /// </summary>
        public virtual string TypeName => this.GetType().Name;
    }
}