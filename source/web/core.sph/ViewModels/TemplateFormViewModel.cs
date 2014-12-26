using System;
using System.ComponentModel.Composition;
using System.Linq;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.Web.ViewModels
{
    public class TemplateFormViewModel
    {
        public TemplateFormViewModel()
        {
            ObjectBuilder.ComposeMefCatalog(this);
        }

        [ImportMany(FormCompilerMetadataAttribute.FORM_COMPILER_CONTRACT, typeof(FormCompiler), AllowRecomposition = true)]
        public Lazy<FormCompiler, IFormCompilerMetadata>[] Compilers { get; set; }

        [ImportMany(Strings.FORM_ELEMENT_CONTRACT, typeof(FormElement), AllowRecomposition = true)]
        public Lazy<FormElement, IDesignerMetadata>[] LazyFormElements { get; set; }

        public ObjectCollection<FormElement> FormElements
        {
            get { return  this.LazyFormElements.Select(x => x.Value).ToObjectCollection(); }
        }

        public bool IsImportVisible { get; set; }
    }
}