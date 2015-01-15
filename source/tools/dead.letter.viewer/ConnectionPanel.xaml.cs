using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Bespoke.Station.Windows.RabbitMqDeadLetter.Models;
using Bespoke.Station.Windows.RabbitMqDeadLetter.ViewModels;

namespace Bespoke.Station.Windows.RabbitMqDeadLetter
{
    public partial class ConnectionPanel
    {
        public ConnectionPanel()
        {
            InitializeComponent();
        }

        private void EditButtonClick(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as ConnectionViewModel;
            if (null != vm)
                vm.EditConnectionCommand.Execute(((Button)sender).DataContext);
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var conn = ((Grid)sender).DataContext as RabbitMqConnection;
            var vm = this.DataContext as ConnectionViewModel;
            if (e.ClickCount == 2 && null != conn && null != vm)
            {
                if (!vm.IsConnected)
                    vm.ConnectCommand.Execute(conn);
                else
                    vm.DisconnectCommand.Execute(conn);
            }
        }
    }
}
