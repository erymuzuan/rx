using System.ComponentModel.Composition;
using System.Windows.Controls;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{

    [Export("RideViewContract", typeof(UserControl))]
    [RideViewMetadata(Caption = "Publish", Order = 10)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class PublishRidePanel
    {
        public PublishRidePanel()
        {
            InitializeComponent();
        }

        [Import]
        public RideViewModel ViewModel
        {
            get { return this.DataContext as RideViewModel; }
            set { this.DataContext = value; }
        }
    }
}
