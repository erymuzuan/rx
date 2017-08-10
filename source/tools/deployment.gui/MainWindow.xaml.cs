using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Windows;
using Bespoke.Sph.Mangements.ViewModels;

namespace deployment.gui
{
    [Export("MainWindow", typeof(Window))]
    public partial class MainWindow : IPartImportsSatisfiedNotification
    {
        [Import]
        public MainViewModel MainViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
        }

        public void OnImportsSatisfied()
        {
            this.MainViewModel.View = this;
            this.DataContext = this.MainViewModel;
            this.MainViewModel.PropertyChanged += MainViewModelPropertyChanged;
        }

        private void MainViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            //
        }
    }
}
