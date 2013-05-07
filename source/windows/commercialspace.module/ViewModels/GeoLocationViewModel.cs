using System.ComponentModel.Composition;
using System.Text;
using Bespoke.Cycling.Domain;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System.Linq;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels
{
    [Export]
    public class GeoLocationViewModel : ViewModelBase
    {

        public RelayCommand<string> SearchCommand { get; set; }
        public RelayCommand<GeoLocation> AddNewLocationCommand { get; set; }
        public RelayCommand<Address> FindAddressLocationCommand { get; set; }
        public RelayCommand<GeoLocation> SelectLocationCommand { get; set; }

        public GeoLocationViewModel()
        {
            var addr = new Address();
            //addr..PropertyChanged += delegate
            //                            {
            //                                this.FindAddressLocationCommand.RaiseCanExecuteChanged();
            //                                this.AddNewLocationCommand.RaiseCanExecuteChanged();
            //                            };
            var point = new GeoLocation { Address = addr };
            //point.PropertyChanged += delegate
            //                             {
            //                                 this.SelectLocationCommand.RaiseCanExecuteChanged();
            //                                 this.AddNewLocationCommand.RaiseCanExecuteChanged();
            //                             };
            this.NewItem = point;

            if (IsInDesignMode) return;

           // HtmlPage.RegisterScriptableObject("GeoLocationPage", this);

            this.AddNewLocationCommand = new RelayCommand<GeoLocation>(AddNewLocation, loc => null != loc && loc.GeoLocationId == 0 && !string.IsNullOrWhiteSpace(loc.Name));
            this.SearchCommand = new RelayCommand<string>(SearchCommandExecute, s => !string.IsNullOrWhiteSpace(s));
            this.FindAddressLocationCommand = new RelayCommand<Address>(FindAddress, adr => null != adr && !string.IsNullOrWhiteSpace(adr.State) && !string.IsNullOrWhiteSpace(adr.District));
            this.SelectLocationCommand = new RelayCommand<GeoLocation>(SelectLocation, loc => null != loc &&  !string.IsNullOrWhiteSpace(loc.Name) && loc.GeoLocationId > 0);
        }

        private void SelectLocation(GeoLocation point)
        {
        }

        private void FindAddress(Address adr)
        {
            this.IsBusy = true;

            var st = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(adr.Street))
                st.AppendFormat("{0},", adr.Street.Replace(" ", "+"));
            if (!string.IsNullOrWhiteSpace(adr.District))
                st.AppendFormat("{0},", adr.District.Replace(" ", "+"));
            if (!string.IsNullOrWhiteSpace(adr.State))
                st.AppendFormat("{0}", adr.State.Replace(" ", "+"));
            //m_htmlWindow.geoCodeGoogle(st.ToString());

            //wc.DownloadStringAsync(new Uri("http://maps.googleapis.com/maps/api/geocode/output?json&region=my&sensor=false&address" + st));
        }

        //[ScriptableMember]
        public void FindAddressCompleted(bool status, double lat, double lng)
        {
            // MessageBox.Show(e.Result);
            this.IsBusy = false;

        }

        private async void AddNewLocation(GeoLocation point)
        {
            this.IsBusy = true;
            var ctx = new CyclingDataContext();
            using (var session = ctx.OpenSession())
            {
                session.Attach(point);
                await session.SubmitChanges();

            }

            this.SelectLocationCommand.RaiseCanExecuteChanged();
            this.AddNewLocationCommand.RaiseCanExecuteChanged();
            this.IsBusy = false;

        }

        private async void SearchCommandExecute(string keyword)
        {
            this.IsBusy = true;
            var ctx = new CyclingDataContext();
            var query = ctx.GeoLocations.Where(g => g.Name.Contains(keyword) || g.Keywords.Contains(keyword));
            var lo = await ctx.LoadAsync(query);

            this.IsBusy = false;
            this.SearchResultCollection.ClearAndAddRange(lo.ItemCollection);

        }


        private readonly ObjectCollection<GeoLocation> m_searchResulrCollection = new ObjectCollection<GeoLocation>();
        private bool m_isBusy;
        private GeoLocation m_selectedLocation;
        private GeoLocation m_newItem;

        public GeoLocation NewItem
        {
            get { return m_newItem; }
            set
            {
                m_newItem = value;
                RaisePropertyChanged("NewItem");
            }
        }

        public GeoLocation SelectedLocation
        {
            get { return m_selectedLocation; }
            set
            {
                m_selectedLocation = value;
                RaisePropertyChanged("SelectedLocation");
                this.SelectLocationCommand.RaiseCanExecuteChanged();
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

        public ObjectCollection<GeoLocation> SearchResultCollection
        {
            get { return m_searchResulrCollection; }
        }
    }
}