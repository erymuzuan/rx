using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "NumberTextBox", Order = 4d, FontAwesomeIcon = "xing", TypeName = "NumberTextBox", Description = "Creates an input for numeric value")]
    public partial class NumberTextBox : FormElement
    {
       
        public  string GetKnockoutBindingExpression()
        {
            var path = this.Path.ConvertJavascriptObjectToFunction();

            return string.Format("value: {0}, visible :{1}, enable :{2}",
                path,
                this.Visible,
                this.Enable ?? "true");
        }
    }
}