using System;
using System.IO;
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
    }
}
