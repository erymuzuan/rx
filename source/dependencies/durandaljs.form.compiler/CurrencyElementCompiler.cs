using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.CONTRACT, typeof (FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.COMPILER_NAME, Type = typeof (CurrencyElement))]
    public class CurrencyElementCompiler : DurandalJsElementCompiler<CurrencyElement>
    {


        public string GetKnockoutBindingExpression()
        {
            var path = Element.Path.ConvertJavascriptObjectToFunction();

            return string.Format("money: {0}, visible :{1}, enable :{2}",
                path,
                Element.Visible,
                Element.Enable ?? "true");
        }

    }
}