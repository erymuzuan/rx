using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Html", Order = 20d, FontAwesomeIcon = "html5",TypeName = "HtmlElement", Description = "Allow developers to create a custom HTML element, NOTE : this is not compatible with other clients other than HTML")]
    public partial class HtmlElement : FormElement
    {
        public override bool IsPathIsRequired
        {
            get { return false; }
        }
    }
}