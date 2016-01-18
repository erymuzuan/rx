using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using Bespoke.Sph.ControlCenter.Helpers;
using Bespoke.Sph.ControlCenter.ViewModel;
using Microsoft.Win32;

namespace Bespoke.Sph.ControlCenter
{
    public partial class MainWindow
    {
        public static string ParseArg(string name)
        {
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name + ":"));
            return val?.Replace("/" + name + ":", string.Empty);
        }

        private static bool ParseArgExist(string name)
        {
            var args = Environment.CommandLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            var val = args.SingleOrDefault(a => a.StartsWith("/" + name));
            return null != val;
        }
        public MainWindow()
        {
            InitializeComponent();
            this.Height = Properties.Settings.Default.WindowHeight;
            this.Width = Properties.Settings.Default.WindowWidth;
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            topMostMenuItem.IsChecked = this.Topmost = Properties.Settings.Default.WindowTopMost;
            this.Left = Properties.Settings.Default.WindowLeft;
            this.Top = Properties.Settings.Default.WindowTop;

            Loaded += MainWindowLoaded;
            this.Closing += MainWindowClosing;
            this.Closed += MainWindow_Closed;

            if (ParseArgExist("in-memory-broker"))
            {
                this.DataContext = new InMemoryBrokerViewModel();
                var mc = new MainViewInMemoryBroker();
                this.controlPanelBox.Content = mc;
                mc.InitializeComponent();
            }
            else
            {
                this.DataContext = new MainViewModel();
                var vr = new MainViewWithRabbitMq();
                this.controlPanelBox.Content = vr;
                vr.InitializeComponent();
            }
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            Properties.Settings.Default.WindowHeight = this.Height;
            Properties.Settings.Default.WindowWidth = this.Width;
            Properties.Settings.Default.WindowTopMost = this.Topmost;
            Properties.Settings.Default.WindowLeft = this.Left;
            Properties.Settings.Default.WindowTop = this.Top;
            Properties.Settings.Default.Save();
        }

        void MainWindowClosing(object sender, CancelEventArgs e)
        {
            dynamic vm = this.DataContext;
            if (null == vm) return;
            if (!vm.CanExit())
            {
                MessageBox.Show("Please stop all the services before exit", Strings.Title,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                e.Cancel = true;
                return;
            }

            vm.Stop();
        }

        async void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            dynamic vm = this.DataContext;
            vm.View = this;
            await vm.LoadAsync();
            outputTextBox.Clear();
            vm.TextWriter = new TextBoxStreamWriter(outputTextBox);
            Console.SetOut(vm.TextWriter);
            Console.WriteLine(Properties.Resources.ControlPanelIsReady);
            outputTextBox.TextChanged += OutputTextBoxTextChanged;

            this.Title += " : " + vm.Settings.ApplicationName;
            ((ViewModelBase)vm).PropertyChanged += PropertyChanged;

        }

        void PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.Post(CommandManager.InvalidateRequerySuggested);
        }

        void OutputTextBoxTextChanged(object sender, TextChangedEventArgs e)
        {
            Delegate caret = new Action(() =>
            {
                //outputTextBox.Focus();
                outputTextBox.CaretIndex = outputTextBox.Text.Length;
                outputTextBox.ScrollToEnd();

            });
            this.Dispatcher.BeginInvoke(caret, DispatcherPriority.ApplicationIdle);
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

        private void WindowExit(object sender, RoutedEventArgs e)
        {
            dynamic vm = this.DataContext;
            if (null == vm) return;
            if (!vm.CanExit())
            {
                MessageBox.Show("Please stop all the services before exit", Strings.Title,
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            this.Close();
        }

        private void ForceWindowExit(object sender, RoutedEventArgs e)
        {
            dynamic vm = this.DataContext;
            if (null == vm) return;
            if (!vm.CanExit())
            {
                var result = MessageBox.Show("This action will close the control center without stopping any of the services, Are you sure you want to continue", Strings.Title, MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
                if (result != MessageBoxResult.Yes)
                {
                    return;
                }

                WebConsoleServer.Default.Stop();
                WebConsoleServer.Default.StopConsume();
            }
            this.Close();
        }

        private void AboutClicked(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Reactive Developer Controler Center for V1", "Reactive Developer", MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        private void HelpClicked(object sender, RoutedEventArgs e)
        {
            Process.Start("http://www.reactivedeveloper.com/docs/control-center");
        }

        private void RunDeadLetterViewerClicked(object sender, RoutedEventArgs e)
        {
            var dlv = AppDomain.CurrentDomain.BaseDirectory + "..\\tools\\dead.letter.viewer.exe";
            try
            {
                Process.Start(dlv);
            }
            catch (Exception ex)
            {
                throw new Exception("Cannot start dlv in " + dlv, ex);
            }
        }
        private void SettingsClicked(object sender, RoutedEventArgs e)
        {
            var setting = new ProjectSettingsWindow
            {
                DataContext = this.DataContext,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Owner = this
            };
            if (setting.ShowDialog() ?? false)
            {

            }

        }

        private void TopMostChecked2(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
        }
        private void TopMostUnchecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
        }

        private void OpenProjectDirectory(object sender, RoutedEventArgs e)
        {
            Process.Start(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\"));
        }

        private void StartPowershell(object sender, RoutedEventArgs e)
        {
            try
            {
                var ps = new ProcessStartInfo
                {
                    FileName = $@"{Environment.SystemDirectory}\WindowsPowerShell\v1.0\powershell.exe",
                    WorkingDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..\\"),
                    Arguments = "-NoExit -Command \"Import-Module .\\utils\\sqlcmd.dll\""
                };

                Process.Start(ps);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Rx Developer", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
