using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{

    [Export("RideViewContract", typeof(UserControl))]
    [RideViewMetadata(Caption = "Itinerary", Order = 3)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ItineraryPanel
    {
        public ItineraryPanel()
        {
            InitializeComponent();
        }

        [Import]
        public RideViewModel ViewModel
        {
            get { return this.DataContext as RideViewModel; }
            set { this.DataContext = value; }
        }
        private void EditLocationClick(object sender, RoutedEventArgs args)
        {
            var it = (Itinerary)((Button)sender).Tag;
            var cm = new GeoLocationViewModel{ SelectedLocation = it.Location};
            var ad = new GeoLocationDialog { DataContext = cm };
            ad.Closed += delegate
                             {
                                 if(ad.DialogResult ?? false)
                                 {
                                     it.Location = cm.SelectedLocation;
                                 }
                             };
            ad.Show();
        }
    }
}
