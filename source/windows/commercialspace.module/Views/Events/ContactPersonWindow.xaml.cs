using System.Windows;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    public partial class ContactPersonWindow 
    {
        public ContactPerson ContactPerson { get; set; }
        public RideViewModel ViewModel { get; set; }

        public ContactPersonWindow()
        {
            InitializeComponent();
            ContactPerson = new ContactPerson();
            this.DataContext = ContactPerson;
        }
        
        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Ride.Organizer.ContactPersonCollection.Add(ContactPerson);
            this.DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

