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
            this.Closing += SetupWizardWindow_Closing   ;
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
            vm.PropertyChanged += Vm_PropertyChanged    ;
            vm.Load();
            this.DataContext = vm;
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Status")
            {
                var vm = (SetupViewModel) this.DataContext;
                if (vm.Status == "success")
                {
                    this.DialogResult = true;
                    this.Close();
                }
            }
        }

        private void TextBoxBase_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var txt = (TextBox) sender;
            txt.ScrollToEnd();
        }
    }
}
