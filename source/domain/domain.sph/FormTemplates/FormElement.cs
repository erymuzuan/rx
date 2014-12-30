using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.ComponentModel.Composition;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Serialization;
using Bespoke.Sph.Domain.Properties;
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

        [JsonIgnore]
        [XmlIgnore]
        public virtual bool IsPathIsRequired { get { return true; } }




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
            var html = FormElementDesigner.ResourceManager.GetString(this.GetType().Name);
            if (!string.IsNullOrWhiteSpace(html))
                return html;
            return "<span class=\"error\">No design surface avaliable for " + this.GetType().GetShortAssemblyQualifiedName() + "</span>";
        }
        public virtual string GetDesignPropertyGrid()
        {
            var html = FormElementPropertyGrid.ResourceManager.GetString("form_element_property_grid_" + this.GetType().Name);
            return !string.IsNullOrWhiteSpace(html) ? html : string.Empty;
        }
        public virtual string GetPropertyToolbox()
        {
            return string.Empty;
        }



        /// <summary>
        /// When override and set to true, it;s the element responsibility to render the label on their owne
        /// </summary>
        public virtual bool RenderOwnLabel
        {
            get { return false; }
        }

        /// <summary>
        /// The unique typename for each activity, should be overriden if you wish to have different name to avoid conflict
        /// </summary>
        public virtual string TypeName
        {
            get { return this.GetType().Name; }
        }

        [JsonIgnore]
        [ImportMany(FormCompilerMetadataAttribute.FORM_ELEMENT_COMPILER_CONTRACT, typeof(FormElementCompiler), AllowRecomposition = true)]
        public Lazy<FormElementCompiler, IFormCompilerMetadata>[] Compilers { get; set; }

        public virtual string GenerateEditorTemplate(string compiler, EntityDefinition entity)
        {
            ObjectBuilder.ComposeMefCatalog(this);
            var fc = this.Compilers.FirstOrDefault(c => c.Metadata.Name == compiler
                && c.Metadata.Type == this.GetType());

            if (string.IsNullOrWhiteSpace(this.Visible))
                this.Visible = "true";
            if (string.IsNullOrWhiteSpace(this.Enable))
                this.Enable = "true";

            if (null == fc)
            {
                var message = string.Format("Cannot find {0} compiler for {1} element", compiler, this.GetType().GetShortAssemblyQualifiedName());
                return message;
            }
            return fc.Value.GenerateEditorTemplate(this, entity);
        }

        public virtual string GenerateDisplayTemplate(string compiler, EntityDefinition entity)
        {
            ObjectBuilder.ComposeMefCatalog(this);
            var fc = this.Compilers.FirstOrDefault(c => c.Metadata.Name == compiler
                && c.Metadata.Type == this.GetType());
            if (null == fc)
            {
                var message = string.Format("Cannot find {0} compiler for {1} element", compiler, this.GetType().GetShortAssemblyQualifiedName());
                return message;
            }
            return fc.Value.GenerateDisplayTemplate(this, entity);
        }

        public virtual ImmutableList<ExpressionDescriptor> CodeExpressions()
        {
            var list = new List<ExpressionDescriptor>
            {
                new ExpressionDescriptor(x => x.Enable, typeof (bool)),
                new ExpressionDescriptor(x => x.Visible, typeof (bool))
            };
            return list.ToImmutableList();
        }

    }

    public class ExpressionDescriptor
    {
        public ExpressionDescriptor(Expression<Func<FormElement, string>> field, Type type)
        {
            this.Field = field;
            this.ReturnType = type;

        }
        public Expression<Func<FormElement, string>> Field { get; set; }
        public Type ReturnType { get; set; }
        public bool AllowAsync { get; set; }
    }
}