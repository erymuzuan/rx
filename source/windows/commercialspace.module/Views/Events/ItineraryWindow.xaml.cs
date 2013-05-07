using System.Windows;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    public partial class ItineraryWindow
    {
        public RideViewModel ViewModel { get; set; }
        public Itinerary Itinerary { get; set; }
        
        public ItineraryWindow()
        {
            InitializeComponent();
            Itinerary = new Itinerary();
            this.DataContext = Itinerary;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Ride.ItineraryCollection.Add(Itinerary);
            this.DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void AddActivityClicked(object sender, RoutedEventArgs e)
        {
            //Itinerary.ItineraryItemCollection.Add(new ItineraryItem());
        }

        private void DeleteActivityClicked(object sender, RoutedEventArgs e)
        {
            //var item = dayDataGrid.SelectedItem as ItineraryItem;
            //Itinerary.ItineraryItemCollection.Remove(item);
        }
    }
}

