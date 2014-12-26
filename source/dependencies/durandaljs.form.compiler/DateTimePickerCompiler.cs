using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.FORM_ELEMENT_COMPILER_CONTRACT, typeof(FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.DURANDAL_JS, Type = typeof(DateTimePicker))]
    public class DateTimePickerCompiler : DurandalJsElementCompiler<DateTimePicker>
    {

        public string GetKnockoutBindingExpression()
        {
            var picker = this.Element;
            return string.Format("kendoDateTime: {0}, visible :{1}, enable :{2}",
                picker.Path.ConvertJavascriptObjectToFunction(),
                picker.Visible, picker.Enable);
        }

    }
}