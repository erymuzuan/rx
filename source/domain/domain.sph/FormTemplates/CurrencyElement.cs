using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Currency", TypeName = "Currency", Order = 3d, FontAwesomeIcon = "dollar", Description = "Creates an input for currency/decimal")]
    public partial class CurrencyElement : FormElement
    {
    }
}