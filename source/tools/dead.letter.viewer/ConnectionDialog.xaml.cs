using System.Windows;
using Bespoke.Station.Windows.RabbitMqDeadLetter.ViewModels;

namespace Bespoke.Station.Windows.RabbitMqDeadLetter
{
    public partial class ConnectionDialog
    {
        public ConnectionDialog()
        {
            InitializeComponent();
            this.Loaded += ConnectionDialogLoaded;
        }

        void ConnectionDialogLoaded(object sender, RoutedEventArgs e)
        {
            var context = this.DataContext as ConnectionViewModel;
            if (null != context)
            {
                passwordBox.Password = context.SelectedConnection.Password;
                if (context.SelectedConnection.Port == 0)
                    context.SelectedConnection.Port = 5672;
                if (context.SelectedConnection.ApiPort == 0)
                    context.SelectedConnection.ApiPort = 15672;
            }
            this.hostTextBox.Focus();

        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            var context = this.DataContext as ConnectionViewModel;
            if (null != context)
            {
                context.SelectedConnection.Password = passwordBox.Password;
            }
            this.DialogResult = true;
            this.Close();
        }
    }
}
