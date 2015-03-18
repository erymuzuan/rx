using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using Bespoke.Sph.ControlCenter.Helpers;
using Bespoke.Sph.ControlCenter.ViewModel;
using Microsoft.Win32;

namespace Bespoke.Sph.ControlCenter
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindowLoaded;
            this.Closing += MainWindowClosing;
        }

        void MainWindowClosing(object sender, System.ComponentModel.CancelEventArgs e)
        {

            var vm = this.DataContext as MainViewModel;
            if (null == vm) return;
            if (vm.CanExit())
            {
                MessageBox.Show("Please stop all the services before exit", Strings.Title,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                e.Cancel = true;
            }
        }

        async void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as MainViewModel;
            if (null == vm) throw new InvalidOperationException("The DataContext is not MainViewModel");
            vm.View = this;
            await vm.LoadAsync();
            outputTextBox.Clear();
            vm.TextWriter = new TextBoxStreamWriter(outputTextBox);
            Console.SetOut(vm.TextWriter);
            Console.WriteLine(Properties.Resources.ControlPanelIsReady);
            outputTextBox.TextChanged += OutputTextBoxTextChanged;

        }

        void OutputTextBoxTextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Delegate caret = new Action(() =>
            {
                //outputTextBox.Focus();
                outputTextBox.CaretIndex = outputTextBox.Text.Length;
                outputTextBox.ScrollToEnd();

            });
            this.Dispatcher.BeginInvoke(caret, DispatcherPriority.ApplicationIdle);
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

            Process.Start(new ProcessStartInfo(string.Format("http://localhost:{0}/", vm.Port)));
            e.Handled = true;
        }

        private void CleartOutputText(object sender, RoutedEventArgs e)
        {
            outputTextBox.Clear();
        }

        private void SaveOutputText(object sender, RoutedEventArgs e)
        {
            var text = outputTextBox.Text;
            var dlg = new SaveFileDialog
            {
                RestoreDirectory = true,
                InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                Filter = "text|*.txt;*.log|All Files|*.*",
                Title = "Save log"
            };

            if (dlg.ShowDialog() ?? false)
            {
                File.WriteAllText(dlg.FileName, text);
            }
        }
    }
}
