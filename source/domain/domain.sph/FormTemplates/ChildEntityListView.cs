using System.ComponentModel.Composition;

namespace Bespoke.Sph.Domain
{
    [Export("FormDesigner", typeof(FormElement))]
    [DesignerMetadata(Name = "Child Entity Table", FontAwesomeIcon = "list-alt", Order = 14d, TypeName = "ChildEntityListView", Description = "Creates a table for child item entry")]
    public partial class ChildEntityListView : FormElement
    {

    }
}