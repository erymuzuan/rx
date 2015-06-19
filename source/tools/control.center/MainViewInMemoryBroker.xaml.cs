using System.Diagnostics;
using System.Windows.Navigation;

namespace Bespoke.Sph.ControlCenter
{
    public partial class MainViewInMemoryBroker
    {
        public MainViewInMemoryBroker()
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
            dynamic vm = this.DataContext;

            Process.Start(new ProcessStartInfo($"http://localhost:{vm.Settings.WebsitePort}/"));
            e.Handled = true;
        }
    }
}
