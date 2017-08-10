using System;
using System.ComponentModel.Composition;
using System.Windows.Threading;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Bespoke.Sph.Mangements.ViewModels
{
    [Export]
    public partial  class MainViewModel : ViewModelBase, IView
    {
        public RelayCommand LoadCommand { get; set; }

        public MainViewModel()
        {
            this.LoadCommand = new RelayCommand(Load, ()=> true);
        }

        private void Load()
        {
            throw new NotImplementedException();
        }

        public DispatcherObject View { get; set; }
    }
}
