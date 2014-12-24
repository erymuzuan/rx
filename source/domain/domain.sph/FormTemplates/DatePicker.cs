
using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "DatePicker",TypeName = "DatePicker", Order = 10d, FontAwesomeIcon = "calendar", Description = "Creates an input for date entry")]
    public partial class DatePicker : FormElement
    {
        public override string GetDesignSurfaceElement()
        {
            return @"   
 <div class=""input-group"">
    <input type=""text"" data-bind=""attr: { 'title': Tooltip, 'class': CssClass() + ' form-control ','placeholder':' [ ' + Path() + ' ] ' }"" />
            <span class=""input-group-addon"">
                <span class=""glyphicon glyphicon-calendar""></span>
            </span>
 </div>";
        }


    }
}