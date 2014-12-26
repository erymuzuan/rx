using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Section divider", Order = 19d, TypeName = "SectionFormElement", FontAwesomeIcon = "bold", Description = "creates a dividier for different section")]
    public partial class SectionFormElement : FormElement
    {


        public override bool IsPathIsRequired
        {
            get { return false; }
        }


    }
}