using System;
using System.Windows;
using Bespoke.Sph.Windows.Infrastructure;
using Bespoke.Sph.Windows.ViewModels.Utils;
using Telerik.Windows.Controls;

namespace Bespoke.Sph.Windows.Views
{
    public partial class SettingAndOptionsWindow
    {
        public SettingAndOptionsWindow()
        {
            InitializeComponent();
            if (this.IsInDesignMode()) return;
            this.Owner = Application.Current.MainWindow;

            this.Loaded += SettingAndOptionsWindowLoaded;
            this.optionTabs.SelectionChanged += OptionTabsSelectionChanged;
        }

        void OptionTabsSelectionChanged(object sender, RadSelectionChangedEventArgs e)
        {
            var tabItem = optionTabs.SelectedItem as RadTabItem;
            if (null == tabItem) return;
            if (null != tabItem.Content) return;

            var metadata = tabItem.Tag as Lazy<object, IOptionPanelMetadata>;
            if (null == metadata) return;
            tabItem.Content = metadata.Value;

        }

        void SettingAndOptionsWindowLoaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as SettingAndOptionsViewModel;
            if (null == vm) return;

            foreach (var p in vm.OptionPanels)
            {
                var tabItem = new RadTabItem { Header = p.Metadata.Caption, Tag = p };
                optionTabs.Items.Add(tabItem);
            }

        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            var vm = ((SettingAndOptionsViewModel)this.DataContext);
            vm.SaveCommand.Execute(null);
            this.Close();
        }
    }
}