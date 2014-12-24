using System.ComponentModel.Composition;
using Bespoke.Sph.Domain;

namespace Bespoke.Sph.FormCompilers.DurandalJs
{
    [Export(FormCompilerMetadataAttribute.CONTRACT, typeof(FormElementCompiler))]
    [FormCompilerMetadata(Name = Constants.COMPILER_NAME, Type = typeof(DatePicker))]
    public class DatePickerCompiler : DurandalJsElementCompiler<DatePicker>
    {
        public override string GenerateEditor(DatePicker picker)
        {
            return
                string.Format(
                    @"     <input {0}
    class=""{1} form-control  {2}"" 
    title=""{3}""
    data-bind=""{4}""
    id=""{5}"" 
    type=""text"" 
    name=""{6}"" />",
                    picker.IsRequired ? "required" : string.Empty,
                    picker.CssClass,
                    picker.Size,
                    picker.Tooltip,
                    this.GetKnockoutBindingExpression(picker),
                    picker.ElementId,
                    picker.Path
                    );
        }



        public string GetKnockoutBindingExpression(DatePicker picker)
        {
            if (picker.IsCompact)
                return string.Format("kendoDate: {0}, visible :{1}, enable :{2}",
                    picker.Path,
                    picker.Visible, picker.Enable);
            return string.Format("kendoDate: {0}, enable :{1}", picker.Path, picker.Enable);
        }
    }
}