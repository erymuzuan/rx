using System;
using System.Collections.Specialized;
using System.Linq;
using System.Windows.Threading;
using Bespoke.Sph.ControlCenter.Model;
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

        void UpdaterViewLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var vm = new UpdaterViewModel {View = this};
            this.DataContext = vm;
        }
        
    }
}
