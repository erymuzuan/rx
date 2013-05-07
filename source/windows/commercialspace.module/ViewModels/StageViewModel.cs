using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using Bespoke.Cycling.Windows.RideOrganizerModule.Views;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Linq;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels
{
    public class StageViewModel : StationViewModelBase<Ride>
    {
        public RelayCommand<StageViewModel> AddCategoryCommad { get; set; }
        public RelayCommand<StageCategory> RemoveCategoryCommad { get; set; }
        public RelayCommand<GeoLocation> SelectLocationCommand { get; set; }
        public RelayCommand<string> SearchRouteCommand { get; set; }

        public StageViewModel(Stage stage)
        {
            this.SelectedItem = stage;
            if (null == stage.StartLocation) stage.StartLocation = new GeoLocation();
            if (null == stage.EndLocation) stage.EndLocation = new GeoLocation();
            this.AddCategoryCommad = new RelayCommand<StageViewModel>(
                vm => vm.SelectedItem.StageCategoryCollection.Add(new StageCategory
                {
                    Bil = vm.SelectedItem.StageCategoryCollection.Count + 1,
                    CutOff = vm.SelectedItem.CutOffTime,
                    Distance = vm.SelectedItem.Distance
                }), vm => null != vm && null != vm.SelectedItem && null != vm.SelectedItem.StageCategoryCollection);

            this.RemoveCategoryCommad = new RelayCommand<StageCategory>(c => this.SelectedItem.StageCategoryCollection.Remove(c),
                c => null != c && null != this.SelectedItem);

            this.SelectLocationCommand = new RelayCommand<GeoLocation>(SelectLocation);
            this.SearchRouteCommand = new RelayCommand<string>(SearchRoute, t => !string.IsNullOrWhiteSpace(t));
        }

        private async void SearchRoute(string obj)
        {
            var ctx = new CyclingDataContext();
            var query = ctx.Routes.Where(r => r.Name.Contains(obj));
            var lo = await ctx.LoadAsync(query);
            this.RouteSearchResultCollection.ClearAndAddRange(lo.ItemCollection);
        }


        private void SelectLocation(GeoLocation location)
        {
            var cm = new GeoLocationViewModel();
            var ad = new GeoLocationDialog { DataContext = cm };
            ad.Closed += delegate
            {
                if (ad.DialogResult ?? false)
                {
                    var loc = cm.SelectedLocation;
                    location.Name = loc.Name;
                    location.Lat = loc.Lat;
                    location.Lng = loc.Lng;
                    location.Type = loc.Type;
                    location.Address = loc.Address.Clone();
                }
            };
            ad.Show();

        }

        private Stage m_selectedItem;
        private bool m_isBusy;
        private Route m_selectedRoute;

        public Route SelectedRoute
        {
            get { return m_selectedRoute; }
            set
            {
                m_selectedRoute = value;
                RaisePropertyChanged("SelectedRoute");
                if (null != value)
                {
                    this.SelectedItem.RouteName = value.FriendlyName;
                    this.SelectedItem.Distance = value.Distance;
                    this.SelectedItem.DifficultyString = value.Difficulty;
                    //this.SelectedItem.Profile = value.Profile;
                   // this.SelectedItem.TrafficCondition = value.Traffic;
                   this.SelectedItem.RoadCondition = value.Condition;
                }
            }
        }

        public bool IsBusy
        {
            get { return m_isBusy; }
            set
            {
                m_isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }
        private readonly ObjectCollection<Route> m_routeSearchResultCollection = new ObjectCollection<Route>();

        public ObjectCollection<Route> RouteSearchResultCollection
        {
            get { return m_routeSearchResultCollection; }
        }
        public Stage SelectedItem
        {
            get { return m_selectedItem; }
            set
            {
                m_selectedItem = value;
                RaisePropertyChanged("SelectedItem");
            }
        }
    }
}
