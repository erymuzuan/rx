using System.ComponentModel.Composition;
using System.Windows;
using Bespoke.Sph.Mangements.ViewModels;
using Telerik.Windows.Controls;

namespace Bespoke.Sph.Mangements
{
    [Export]
    public partial class ScriptWindow
    {
        public ScriptWindow()
        {
            StyleManager.ApplicationTheme = new Windows8Theme();
            InitializeComponent();
            this.Loaded += ScriptWindow_Loaded;
            this.Closing += Window_Closing;
        }

        [Import]
        public MigrationScriptViewModel ViewModel { get; set; }

        private void ScriptWindow_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            this.DataContext = this.ViewModel;
            this.ViewModel.Load().Wait();
            this.ViewModel.PropertyChanged += ViewModel_PropertyChanged;
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(MigrationScriptViewModel.Script))
            {
                this.textEditor.Text = this.ViewModel.Script;
            }
        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Visibility = Visibility.Hidden;
        }

        private void OkClick(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Script = textEditor.Text;
            this.ViewModel.SaveCommand.Execute(null);
            this.Visibility = Visibility.Hidden;
        }
    }
}
