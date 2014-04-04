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
            this.DataContext = new UpdaterViewModel();
        }
    }
}
