using System.Windows;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    public partial class AddFaqSectionWindow
    {
        public Faq Faq { get; set; }
        public RideViewModel ViewModel { get; set; }

        public AddFaqSectionWindow()
        {
            InitializeComponent();

            Faq = new Faq();
            this.DataContext = Faq;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            ViewModel.Ride.FaqCollection.Add(Faq);
            this.DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}

