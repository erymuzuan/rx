using System.ComponentModel.Composition;
using System.Windows.Controls;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    [Export("RideViewContract", typeof(UserControl))]
    [RideViewMetadata(Caption = "Downloadable documents", Order = 9)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class _DocumentListPanel
    {
        public _DocumentListPanel()
        {
            InitializeComponent();
        }
    }
}
