using System.Windows;
using Bespoke.Station.Windows.RabbitMqDeadLetter.ViewModels;

namespace Bespoke.Station.Windows.RabbitMqDeadLetter
{
    public partial class ConnectionDialog
    {
        public ConnectionDialog()
        {
            InitializeComponent();
            this.Loaded += ConnectionDialog_Loaded;
        }

        void ConnectionDialog_Loaded(object sender, RoutedEventArgs e)
        {
            var context = this.DataContext as ConnectionViewModel;
            if (null != context)
            {
                passwordBox.Password = context.SelectedConnection.Password;
            }
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
