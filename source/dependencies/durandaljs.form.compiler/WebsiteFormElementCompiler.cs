using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.FORM_ELEMENT_COMPILER_CONTRACT, typeof(FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.DURANDAL_JS, Type = typeof(WebsiteFormElement))]
    public class WebsiteFormElementCompiler : DurandalJsElementCompiler<WebsiteFormElement>
    {
        public string GetKnockoutBindingExpression()
        {
            var textBox = this.Element;
            if (string.IsNullOrWhiteSpace(textBox.Enable))
                textBox.Enable = "true";


            var path = textBox.Path.ConvertJavascriptObjectToFunction();
            const string BINDING = "value";

            var unique = textBox.IsUniqueName ? ",uniqueName:true" : "";
            return string.Format("{2}: {0}, visible :{1}, enable :{3} {4}", path, textBox.Visible, BINDING, textBox.Enable, unique);
        }

    }
}