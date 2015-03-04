using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Paragraph text", Order = 8d, TypeName = "TextAreaElement", FontAwesomeIcon = "desktop", Description = "Creates a multiline text input")]
    public partial class TextAreaElement : FormElement
    {
       public override string GetKnockoutBindingExpression()
        {
            var path = this.Path;

            return string.Format("{2}: {0}, visible :{1}, enable :{3}",
                path,
                this.Visible,
                this.IsHtml ? "kendoEditor" : "value",
                this.Enable);
        }
    }
}

