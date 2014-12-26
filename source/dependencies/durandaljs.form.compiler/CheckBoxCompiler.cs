using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.FORM_ELEMENT_COMPILER_CONTRACT, typeof (FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.DURANDAL_JS, Type = typeof (CheckBox))]
    public class CheckBoxCompiler : DurandalJsElementCompiler<CheckBox>
    {
        public string GetKnockoutBindingExpression()
        {
            return string.Format("checked : {0}, enable :{1}", this.Element.Path, this.Element.Enable);
        }

    }
}