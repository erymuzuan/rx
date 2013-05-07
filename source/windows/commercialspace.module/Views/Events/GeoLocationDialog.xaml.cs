using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;
using Microsoft.Maps.MapControl.WPF;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    public partial class GeoLocationDialog
    {
        public GeoLocationDialog()
        {
            InitializeComponent();
            if (DesignerProperties.GetIsInDesignMode(this)) return;
            this.Loaded += DialogLoaded;

        }

        private bool m_createNewFlag;



        private void DialogLoaded(object sender, RoutedEventArgs e)
        {
            var vm = this.DataContext as GeoLocationViewModel;
            if (null == vm) return;
            vm.PropertyChanged += VmPropertyChanged;
        }

        void VmPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            locationPushpin.Visibility = Visibility.Collapsed;
            if (m_createNewFlag) return;

            if (e.PropertyName != "SelectedLocation") return;
            var vm = this.DataContext as GeoLocationViewModel;
            if (null == vm) return;
            if (null == vm.SelectedLocation) return;
            if (null == vm.SelectedLocation.Lat) return;
            if (null == vm.SelectedLocation.Lng) return;

            locationPushpin.Visibility = Visibility.Visible;
            locationPushpin.Location = new Location(vm.SelectedLocation.Lat, vm.SelectedLocation.Lng);
            locationPushpin.Content = "X";
            map.Center = locationPushpin.Location;
            map.ZoomLevel = 12;

        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            if (m_createNewFlag)
            {
                var vm = this.DataContext as GeoLocationViewModel;
                if (null == vm) return;
                // set the last location of the pushpi
                vm.NewItem.Lat = dragablePushpin.Location.Latitude;
                vm.NewItem.Lng = dragablePushpin.Location.Longitude;
            }

            this.DialogResult = true;
            this.Close();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void FindAddressButtonClick(object sender, RoutedEventArgs e)
        {
            dragablePushpin.Location = map.Center;
            dragablePushpin.Visibility = Visibility.Visible;
            dragablePushpin.Content = "X";
        }

        private void ChangeToAddModeButtonClick(object sender, RoutedEventArgs e)
        {
            m_createNewFlag = true;
            var vm = this.DataContext as GeoLocationViewModel;
            if (null == vm) return;
            if (!vm.SearchResultCollection.Contains(vm.NewItem))
                vm.SearchResultCollection.Add(vm.NewItem);
            if (null == vm.SelectedLocation)
                vm.SelectedLocation = vm.NewItem;
        }

        private void DraggablePushpinMouseButonUp(object sender, MouseButtonEventArgs e)
        {
            if (!m_createNewFlag) return;
            var vm = this.DataContext as GeoLocationViewModel;
            if (null == vm) return;
            vm.NewItem.Lat = dragablePushpin.Location.Latitude;
            vm.NewItem.Lng = dragablePushpin.Location.Longitude;
            vm.AddNewLocationCommand.RaiseCanExecuteChanged();
        }

        private void CancelAddNewButtonClick(object sender, RoutedEventArgs e)
        {
            m_createNewFlag = false;
        }
    }
}

