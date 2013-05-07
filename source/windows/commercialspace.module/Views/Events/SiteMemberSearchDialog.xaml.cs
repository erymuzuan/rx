using System.Windows;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views.Events
{
    public partial class SiteMemberSearchDialog
    {
        public SiteMemberSearchDialog()
        {
            InitializeComponent();
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}

