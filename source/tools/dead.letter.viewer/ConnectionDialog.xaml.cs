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
            if (this.DataContext is ConnectionViewModel context)
            {
                var conn = context.SelectedConnection;
                passwordBox.Password = conn.Password;
                if (conn.Port == 0)
                    conn.Port = 5672;
                if (conn.ApiPort == 0)
                    conn.ApiPort = 15672;
                if (string.IsNullOrWhiteSpace(conn.HostName))
                    conn.HostName = ConfigurationManager.RabbitMqHost;
                if (string.IsNullOrWhiteSpace(conn.UserName))
                    conn.UserName = ConfigurationManager.RabbitMqUserName;
                if (string.IsNullOrWhiteSpace(conn.Password))
                    conn.Password = ConfigurationManager.RabbitMqPassword;
                if (string.IsNullOrWhiteSpace(conn.VirtualHost))
                    conn.VirtualHost = ConfigurationManager.RabbitMqVirtualHost;
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
