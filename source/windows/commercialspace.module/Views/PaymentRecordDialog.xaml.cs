using System.ComponentModel;
using System.Windows;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    public partial class PaymentRecordDialog 
    {
        public PaymentRecordDialog()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this)) return;
        }


        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
        private void OkButtonClicked(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
