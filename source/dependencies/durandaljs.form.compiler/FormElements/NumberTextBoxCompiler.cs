using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs.FormElements
{
    [Export(FormCompilerMetadataAttribute.FORM_ELEMENT_COMPILER_CONTRACT, typeof (FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.DURANDAL_JS, Type = typeof (NumberTextBox))]
    public class NumberTextBoxCompiler : DurandalJsElementCompiler<NumberTextBox>
    {
        public string GetKnockoutBindingExpression()
        {
            var textbox = this.Element;
            var path = textbox.Path.ConvertJavascriptObjectToFunction();

            return string.Format("value: {0}, visible :{1}, enable :{2}",
                path,
                textbox.Visible,
                textbox.Enable ?? "true");
        }


    }
}