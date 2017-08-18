using System;
using System.ComponentModel;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Windows;
using Bespoke.Sph.Domain;
using Bespoke.Sph.Mangements;
using Bespoke.Sph.Mangements.Models;
using Bespoke.Sph.Mangements.ViewModels;
using Telerik.Windows.Controls;
using CheckBox = System.Windows.Controls.CheckBox;

namespace deployment.gui
{
    [Export("MainWindow", typeof(Window))]
    public partial class MainWindow : IPartImportsSatisfiedNotification
    {
        [Import]
        public MainViewModel MainViewModel { get; set; }
        [Import]
        public ScriptWindow ScriptWindow { get; set; }

        public MainWindow()
        {
            StyleManager.ApplicationTheme = new Windows8Theme();
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
            this.Closed += MainWindow_Closed;
        }

        private void MainWindow_Closed(object sender, EventArgs e)
        {
            if(this.ScriptWindow.Visibility == Visibility.Hidden)
                this.ScriptWindow.Close();

            Application.Current.Shutdown(0);
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var vm = (MainViewModel)this.DataContext;
            vm.LoadCommand.Execute(null);
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

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e)
        {
            var chb = (CheckBox)sender;
            var ed = (EntityDeployment)chb.DataContext;


            var vm = (MainViewModel)this.DataContext;
            if(ed.IsSelected && !vm.SelectedCollection.Contains(ed))
                vm.SelectedCollection.Add(ed);
            if(!ed.IsSelected && vm.SelectedCollection.Contains(ed))
                vm.SelectedCollection.Remove(ed);
        }

        private void ShowHelp(object sender, RoutedEventArgs e)
        {
            Process.Start(new Uri($@"{ConfigurationManager.ToolsPath}\HelpText.html").ToString());
        }

        private void ShowScriptWindow(object sender, RoutedEventArgs e)
        {
            this.ScriptWindow.ShowDialog();
        }
    }
}
