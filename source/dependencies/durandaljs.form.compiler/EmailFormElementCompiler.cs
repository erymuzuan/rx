using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.FORM_ELEMENT_COMPILER_CONTRACT, typeof(FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.DURANDAL_JS, Type = typeof(EmailFormElement))]
    public class EmailFormElementCompiler : DurandalJsElementCompiler<EmailFormElement>
    {

        public string GetKnockoutBindingExpression()
        {
            var path = this.Element.Path.ConvertJavascriptObjectToFunction();

            return string.Format("value: {0}, visible :{1}, enable: {2}",
                path,
                this.Element.Visible,
                this.Element.Enable ?? "true");
        }



    }
}