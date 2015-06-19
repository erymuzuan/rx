using System.Threading;
using System.Windows;
using Bespoke.Sph.ControlCenter.ViewModel;

namespace Bespoke.Sph.ControlCenter
{
    public partial class UpdaterView
    {
        public UpdaterView()
        {
            InitializeComponent();
            this.Loaded += UpdaterViewLoaded;
        }

        void UpdaterViewLoaded(object sender, RoutedEventArgs e)
        {
            var vm = new UpdaterViewModel { View = this };
            this.DataContext = vm;
            this.QueueUserWorkItem(() =>
            {
                Thread.Sleep(3000);
                this.Post(() =>
                {
                    vm.CheckUpdateCommand.Execute(true);

                });
            });
        }

    }
}
