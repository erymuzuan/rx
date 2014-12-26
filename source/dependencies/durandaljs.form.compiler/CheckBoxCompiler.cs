using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.CONTRACT, typeof (FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.COMPILER_NAME, Type = typeof (CheckBox))]
    public class CheckBoxCompiler : DurandalJsElementCompiler<CheckBox>
    {
        public string GetKnockoutBindingExpression()
        {
            return string.Format("checked : {0}, enable :{1}", this.Element.Path, this.Element.Enable);
        }

    }
}