using Telerik.Windows.Controls;

namespace permissions.editor
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            StyleManager.ApplicationTheme = new Windows8Theme();
            InitializeComponent();
            this.Loaded += MainWindowLoaded;
        }

        void MainWindowLoaded(object sender, System.Windows.RoutedEventArgs e)
        {
            var vm = new MainViewModel();
            this.DataContext = vm;
            vm.Load();
        }
    }
}