using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.CONTRACT, typeof (FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.COMPILER_NAME, Type = typeof (NumberTextBox))]
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