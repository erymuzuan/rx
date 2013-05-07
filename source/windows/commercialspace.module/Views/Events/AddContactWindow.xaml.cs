using System.Windows;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    public partial class AddContactWindow
    {
        public AddContactWindow()
        {
            InitializeComponent();
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

