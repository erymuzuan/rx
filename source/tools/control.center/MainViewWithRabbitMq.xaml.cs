using System;
using System.Diagnostics;
using System.Windows.Navigation;
using Bespoke.Sph.ControlCenter.ViewModel;

namespace Bespoke.Sph.ControlCenter
{
    public partial class MainViewWithRabbitMq
    {
        public MainViewWithRabbitMq()
        {
            InitializeComponent();
        }

        private void Navigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
        private void NavigateApp(object sender, RequestNavigateEventArgs e)
        {
            var vm = this.DataContext as MainViewModel;
            if (null == vm) throw new InvalidOperationException("The DataContext is not MainViewModel");

            Process.Start(new ProcessStartInfo($"http://localhost:{vm.Settings.WebsitePort}/"));
            e.Handled = true;
        }
    }
}
