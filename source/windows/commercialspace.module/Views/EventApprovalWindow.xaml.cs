using System.Windows;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    public partial class EventApprovalWindow 
    {
        public EventApprovalWindow()
        {
            InitializeComponent();
        }

        private void NoButtonClick(object sender, RoutedEventArgs e)
        {
            this.IsApproved = false;
            this.DialogResult = true;
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            this.IsApproved = true;
            this.DialogResult = true;
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.IsApproved = false;
            this.DialogResult = false;
        }

        public bool IsApproved
        {
            get { return (bool)GetValue(IsApprovedProperty); }
            set { SetValue(IsApprovedProperty, value); }
        }

        public static readonly DependencyProperty IsApprovedProperty =
            DependencyProperty.Register("IsApproved", typeof(bool), typeof(EventApprovalWindow), new PropertyMetadata(false));

    }
        
}

