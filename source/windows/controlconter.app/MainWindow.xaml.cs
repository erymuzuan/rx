using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;
using Bespoke.Sph.ControlCenter.Helpers;
using Bespoke.Sph.ControlCenter.ViewModel;

namespace Bespoke.Sph.ControlCenter
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var vm = this.DataContext as MainViewModel;
            vm.TextWriter = new TextBoxStreamWriter(OutputTextBox);
            Console.SetOut(vm.TextWriter);
            Console.WriteLine(@"[SPH Control Panel ready]");
        }

        private void Navigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
    }
}
