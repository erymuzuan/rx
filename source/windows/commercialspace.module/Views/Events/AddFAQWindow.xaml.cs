using System.Windows;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    public partial class AddFaqWindow 
    {
        public AddFaqWindow()
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

