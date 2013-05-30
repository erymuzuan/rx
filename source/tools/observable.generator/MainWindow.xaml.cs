using System.Windows;

namespace observable.generator
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = new MainViewModel();
            this.DataContext = vm;
            vm.Load();
        }
    }
}
