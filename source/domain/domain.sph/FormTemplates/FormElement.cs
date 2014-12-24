using System;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Xml.Serialization;
using Newtonsoft.Json;


namespace Bespoke.Sph.Domain
{
    public partial class FormElement : DomainObject
    {
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
                var css = string.Format("col-md-{0}", this.LabelColMd ?? 4);
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
                var css = string.Format("col-md-{0}", this.InputColMd ?? 8);
                if (this.InputColMd.HasValue) css += " col-lg-" + this.InputColLg;
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
            return new BuildError[] { };
        }
        public virtual BuildError[] ValidateBuild(EntityDefinition ed)
        {
            return new BuildError[] { };
        }
        public virtual BuildError[] ValidateBuild(EntityForm form)
        {
            return new BuildError[] { };
        }

        public virtual Bitmap GetPngIcon()
        {
            return null;
        }

        public virtual string GetEditorViewModel()
        {
            return string.Empty;
        }

        public virtual string GetEditorView()
        {
            return string.Empty;
        }
        public virtual string GetDesignSurfaceElement()
        {
            return "<span class=\"error\">No design surface avaliable for " + this.GetType().GetShortAssemblyQualifiedName() + "</span>";
        }
        public virtual string GetPropertyToolbox()
        {
            return string.Empty;
        }

        /// <summary>
        /// The unique typename for each activity, should be overriden if you wish to have different name to avoid conflict
        /// </summary>
        public virtual string TypeName
        {
            get { return this.GetType().Name; }
        }

        public virtual string GenerateDisplayTemplate(string compiler)
        {
            return "<span class=\"error\">No display template avaliable for " + this.GetType().GetShortAssemblyQualifiedName() + "</span>";

        }
        [ImportMany(FormCompilerMetadataAttribute.CONTRACT, typeof(FormElementCompiler), AllowRecomposition = true)]
        public Lazy<FormElementCompiler, IFormCompilerMetadata>[] Compilers { get; set; }

        public virtual string GenerateEditorTemplate(string compiler)
        {
            ObjectBuilder.ComposeMefCatalog(this);
            var fc = this.Compilers.FirstOrDefault(c => c.Metadata.Name == compiler
                && c.Metadata.Type == this.GetType());
            if (null == fc)
            {
                var message = string.Format("Cannot find {0} compiler for {1} element", compiler, this.GetType().GetShortAssemblyQualifiedName());
                return message;
            }
            return fc.Value.GenerateEditorTemplate(this);
        }
    }
}