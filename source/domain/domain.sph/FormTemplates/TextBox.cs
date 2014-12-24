using System;
using System.ComponentModel.Composition;
using System.Linq;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Single Line Text", IsEnabled = true, TypeName = "TextBox", FontAwesomeIcon = "text-width", Order = 1d, Description = "Creates na input for single line text")]
    public partial class TextBox : FormElement
    {
        public override string GetDesignSurfaceElement()
        {
            return @"<input type=""text"" data-bind=""attr: { 'title': Tooltip, 
'class': CssClass() + ' form-control ' + Size(),
'placeholder' : ' [ ' + Path() + ' ] ' }"" />
";
        }

    }
}