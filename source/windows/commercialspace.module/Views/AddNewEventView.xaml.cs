using System;
using System.ComponentModel.Composition;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    public partial class AddNewEventView 
    {
        public AddNewEventView()
        {
            InitializeComponent();
        }

        [Import]
        public RideViewModel ViewModel
        {
            get { return this.DataContext as RideViewModel; }
            set
            {
                this.DataContext = value;
            }
        }
        // Executes when the user navigates to this page.
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //}

        void ViewModelNavigateHome(object sender, EventArgs e)
        {
            //this.NavigationService.Navigate(new Uri("/MyEventsView", UriKind.RelativeOrAbsolute));
        }

        public void OnImportsSatisfied()
        {
            this.ViewModel.NavigateHome += ViewModelNavigateHome;
            this.ViewModel.Ride.PropertyChanged += (s, a) => this.ViewModel.AddCommand.RaiseCanExecuteChanged();
        }
    }
}
