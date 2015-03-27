using System;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using Bespoke.Sph.ControlCenter.Model;
using Bespoke.Sph.ControlCenter.ViewModel;

namespace Bespoke.Sph.ControlCenter
{

    public partial class SetupWizardWindow
    {
        public SetupWizardWindow()
        {
            InitializeComponent();
            this.Loaded += SetupWizardWindow_Loaded;
            this.Closing += SetupWizardWindow_Closing;
        }

        private void SetupWizardWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var vm = (SetupViewModel)this.DataContext;
            if (vm.IsBusy)
            {
                MessageBox.Show("Please wait untill all the process is finished", "Reactive Developer",
                    MessageBoxButton.OK, MessageBoxImage.Warning);

                e.Cancel = true;
            }
        }

        private void SetupWizardWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = new SetupViewModel { View = this };
            vm.PropertyChanged += Vm_PropertyChanged;
            vm.Load();
            this.DataContext = vm;
            vm.LogCollection.CollectionChanged += LogCollection_CollectionChanged;

        }

        private void LogCollection_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                var items = e.NewItems.Cast<LogEntry>();
                foreach (var t in items)
                {
                    if (t.Message.Trim() == ".")
                    {
                        logTextBox.AppendText(t.Message);
                    }
                    else
                    {
                        var now = DateTime.Now.ToShortTimeString();
                        var message = $"[{now}][{t.Severity}]  {t.Message}";
                        logTextBox.AppendText("\r\n" + message);

                        Delegate caret = new Action(() =>
                        {
                            //outputTextBox.Focus();
                            logTextBox.CaretIndex = logTextBox.Text.Length;
                            logTextBox.ScrollToEnd();

                        });
                        this.Dispatcher.BeginInvoke(caret, DispatcherPriority.ApplicationIdle);
                    }
                }

            }

        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName != "Status") return;
            var vm = (SetupViewModel)this.DataContext;
            var status = vm.Status;
            this.QueueUserWorkItem(() =>
            {
                Thread.Sleep(2000);
                this.Post(() =>
                {
                    var messageBoxText = status == "success"
                        ? "Congratulations.. you now can start building your app"
                        : "Unfortunately there are errors, Please verify that your configuration is successful, you can always run this again by deleting the project.json";

                    MessageBox.Show(messageBoxText,
                        "Reactive Developer",
                        MessageBoxButton.OK,
                        MessageBoxImage.Information);
                    closeButton.Visibility = Visibility.Visible;

                });
            });
        }

        private void CloseButtonClicked(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void ToSetupPanelClick(object sender, RoutedEventArgs e)
        {
            var vm = (SetupViewModel) this.DataContext;
            logTextBox.Text = vm.Settings.ToString();

        }

        private void SetupButtonClick(object sender, RoutedEventArgs e)
        {
            setupTitle.Text = "Please wait while we setup your Reactive Developer application....";
        }
    }
}
