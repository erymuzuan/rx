using System.Windows;
using Telerik.Windows.Controls;

namespace observable.generator
{
    public partial class MainWindow
    {

        public MainWindow()
        {
            StyleManager.ApplicationTheme = new Windows8Theme();
            InitializeComponent();
            this.Loaded += MainWindowLoaded;
        }

        void MainWindowLoaded(object sender, RoutedEventArgs e)
        {
            var vm = new MainViewModel();
            vm.PropertyChanged += VmPropertyChanged;
            this.DataContext = vm;
            vm.Load();
        }

        void VmPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "ObservableText")
            {
                this.textEditor.Text = this.MainViewModel.ObservableText;
            }
        }

        public MainViewModel MainViewModel
        {
            get { return this.DataContext as MainViewModel; }
        }

        private void CopyButtonClick(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(this.textEditor.Text);
        }
    }
}
