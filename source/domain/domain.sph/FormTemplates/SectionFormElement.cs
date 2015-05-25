using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Section divider", Order = 19d, TypeName = "SectionFormElement", FontAwesomeIcon = "bold", Description = "Creates a divider for different section")]
    public partial class SectionFormElement : FormElement
    {

        public override string GetKnockoutBindingExpression()
        {
            if (!string.IsNullOrWhiteSpace(this.Visible))
                return $"visible : {this.Visible}";

            return null;
        }

        public override bool IsPathIsRequired => false;
    }
}