using System.Windows;
using Bespoke.Sph.ControlCenter.ViewModel;

namespace Bespoke.Sph.ControlCenter
{

    public partial class SetupWizardWindow
    {
        public SetupWizardWindow()
        {
            InitializeComponent();
            this.Loaded += SetupWizardWindow_Loaded;
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
            if (e.PropertyName == "IsBusy")
            {
                var vm = (SetupViewModel) this.DataContext;
                if (vm.IsBusy == false)
                {
                    this.DialogResult = true;
                    this.Close();
                }
            }
        }
    }
}
