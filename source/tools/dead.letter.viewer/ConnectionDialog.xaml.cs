using System.Windows;

namespace Bespoke.Station.Windows.RabbitMqDeadLetter
{
    public partial class ConnectionDialog
    {
        public ConnectionDialog()
        {
            InitializeComponent();
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
