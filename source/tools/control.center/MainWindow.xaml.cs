﻿using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
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
            Loaded += MainWindowLoaded;
            this.Closing += MainWindowClosing;

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

        void MainWindowClosing(object sender, CancelEventArgs e)
        {

            dynamic vm = this.DataContext ;
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
    }
}
