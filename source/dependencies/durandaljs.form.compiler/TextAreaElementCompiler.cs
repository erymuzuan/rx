using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.CONTRACT, typeof (FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.COMPILER_NAME, Type = typeof (TextAreaElement))]
    public class TextAreaElementCompiler : DurandalJsElementCompiler<TextAreaElement>
    {

        public string GetKnockoutBindingExpression()
        {
            var text = this.Element;
            var path = Element.Path;

            return string.Format("{2}: {0}, visible :{1}, enable :{3}",
                path,
                text.Visible,
                text.IsHtml ? "kendoEditor" : "value",
                text.Enable);
        }

    }
}