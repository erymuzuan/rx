using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "EmailFormElement",Order = 6d, FontAwesomeIcon = "envelope",TypeName = "EmailFormElement", Description = "Creates an input for email address")]
    public partial class EmailFormElement : FormElement
    {

        public override string GetKnockoutBindingExpression()
        {
            var path = this.Path.ConvertJavascriptObjectToFunction();
         
            return string.Format("value: {0}, visible :{1}, enable: {2}",
                path,
                this.Visible,
                this.Enable ?? "true");
        }
    }
}