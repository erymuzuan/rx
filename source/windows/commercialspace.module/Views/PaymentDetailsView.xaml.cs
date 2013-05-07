using System.ComponentModel.Composition;
using System.Windows;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    [Export]
    public partial class PaymentDetailsView 
    {
        public PaymentDetailsView()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty RegistrationIdProperty =
            DependencyProperty.Register("RegistrationId", typeof(int), typeof(PaymentDetailsView), new PropertyMetadata(0));

        public int RegistrationId
        {
            get { return (int)GetValue(RegistrationIdProperty); }
            set { SetValue(RegistrationIdProperty, value); }
        }

    }
}
