using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    public partial class RegistrationDetailsWindow
    {

        public RegistrationDetailsWindow(RegistrationViewModel viewModel)
        {
            InitializeComponent();
            this.ViewModel = viewModel;
        }

        public RegistrationViewModel ViewModel
        {
            get { return this.DataContext as RegistrationViewModel; }
            set { this.DataContext = value; }
        }

        private void OkButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!this.ViewModel.Rider.Validate())
            {
                return;
            }
            this.ViewModel.SaveCommand.Execute(null);
            this.DialogResult = true;
        }

        private void CancelButtonClick(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DialogResult = false;
        }




    }
}
