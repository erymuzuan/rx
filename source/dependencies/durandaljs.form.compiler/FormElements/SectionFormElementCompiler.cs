using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs.FormElements
{
    [Export(FormCompilerMetadataAttribute.FORM_ELEMENT_COMPILER_CONTRACT, typeof (FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.DURANDAL_JS, Type = typeof (SectionFormElement))]
    public class SectionFormElementCompiler : DurandalJsElementCompiler<SectionFormElement>
    {
        
        public string GetKnockoutBindingExpression()
        {
            if (!string.IsNullOrWhiteSpace(this.Element.Visible))
                return string.Format("visible : {0}", this.Element.Visible);

            return null;
        }
    }
}