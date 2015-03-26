using System.Collections.Specialized;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
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
            ((INotifyCollectionChanged)logListView.Items).CollectionChanged += ListView_CollectionChanged;

        }

        private void ListView_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var vm = (SetupViewModel)this.DataContext;
            logListView.SelectedIndex = vm.LogCollection.Count - 1;
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
                    if (status == "success")
                        MessageBox.Show("Congratulations.. you now can start building your app", "Reactive Developer", MessageBoxButton.OK, MessageBoxImage.Information);
                    else
                        MessageBox.Show("Unfortunately there are errors, Please verify that your configuration is successful, you can always run this again by deleting the project.json", "Reactive Developer", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.DialogResult = true;
                    this.Close();
                });
            });
        }

    }
}
