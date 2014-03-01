using System.Windows;
using System.Windows.Controls;
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
            if(null != vm)
                vm.EditConnectionCommand.Execute(((Button)sender).DataContext);
        }
    }
}
