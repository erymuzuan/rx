using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Bespoke.Cycling.Windows.Infrastructure;
using Bespoke.Cycling.Windows.RideOrganizerModule.Contants;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    [Export("ViewContract", typeof(UserControl))]
    [ViewMetadata(Group = ViewGroup.Home, Image = "/Images/customer.png", Name = ViewNames.EVENT_LIST_VIEW, Caption = "Current Events", Order = 4)]
    public partial class EventListView
    {
        public EventListView()
        {
            InitializeComponent();
        }

        [Import]
        public RideListViewModel ViewModel
        {
            get { return this.DataContext as RideListViewModel; }
            set { this.DataContext = value; }
        }

        private void OpenButtonClick(object sender, RoutedEventArgs e)
        {
            this.ViewModel.OpenCommand.Execute(((Button)sender).Tag);
        }

        private void OpenRegistrationButtonClick(object sender, RoutedEventArgs e)
        {
            this.ViewModel.OpenRegistrationCommand.Execute(((Button)sender).Tag);
        }
    }
}
