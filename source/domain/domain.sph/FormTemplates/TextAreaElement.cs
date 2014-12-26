using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Paragrapah text", Order = 8d, TypeName = "TextAreaElement", FontAwesomeIcon = "desktop", Description = "Creates a munltiline text input")]
    public partial class TextAreaElement : FormElement
    {
       public  string GetKnockoutBindingExpression()
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

