using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.AdminModule.Contants;
using Bespoke.Cycling.Windows.AdminModule.ViewModels;
using Bespoke.Cycling.Windows.Infrastructure;

namespace Bespoke.Cycling.Windows.AdminModule.Views
{
    [Export("ViewContract", typeof(UserControl))]
    [ViewMetadata(Group = ViewGroup.Setting, Image = "/Images/customer.png", Name = ViewNames.EVENT_LIST_VIEW, Caption = "Current Events", Order = 4)]
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

        private void ApproveButtonClick(object sender, RoutedEventArgs e)
        {
            var ride = ((Button) sender).Tag as Ride;
            this.ViewModel.ApproveCommand.Execute(ride);
        }
    }
}
