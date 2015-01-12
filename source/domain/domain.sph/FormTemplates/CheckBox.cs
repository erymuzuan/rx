using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "CheckBox",TypeName = "CheckBox", Order = 9d, FontAwesomeIcon = "check", Description = "Creates a yes/no choice")]
    public partial class CheckBox : FormElement
    {
        public override string GetKnockoutBindingExpression()
        {
            return string.Format("checked : {0}, enable :{1}", this.Path, this.Enable);
        }

    }
}