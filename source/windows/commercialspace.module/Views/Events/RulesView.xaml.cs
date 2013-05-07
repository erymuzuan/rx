using System.ComponentModel.Composition;
using System.Windows.Controls;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{

    [Export("RideViewContract", typeof(UserControl))]
    [RideViewMetadata(Caption = "Rules", Order = 6)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class RulesView 
    {
        public RulesView()
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
