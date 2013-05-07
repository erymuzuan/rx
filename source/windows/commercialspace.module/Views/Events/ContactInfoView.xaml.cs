using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    [Export("RideViewContract", typeof(UserControl))]
    [RideViewMetadata(Caption = "Contacts Information", Order = 4)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class ContactInfoView 
    {
        public ContactInfoView()
        {
            InitializeComponent();
        }


        private void AdContactClicked(object sender, RoutedEventArgs e)
        {
            var contactWindow = new ContactPersonWindow {ViewModel = this.ViewModel };
            contactWindow.Show(); 
        }

        private void DeleteContactClicked(object sender, RoutedEventArgs e)
        {
            var contact = contactsDataGrid.SelectedItem as ContactPerson;
            this.ViewModel.Ride.Organizer.ContactPersonCollection.Remove(contact);
        }


        [Import]
        public RideViewModel ViewModel
        {
            get { return this.DataContext as RideViewModel; }
            set { this.DataContext = value; }
        }
    }
}
