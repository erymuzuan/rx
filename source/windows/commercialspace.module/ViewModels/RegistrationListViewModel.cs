using System.ComponentModel.Composition;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using Bespoke.Cycling.Windows.RideOrganizerModule.Contants;
using Bespoke.Cycling.Windows.RideOrganizerModule.Views;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Linq;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels
{
    [Export]
    public class RegistrationListViewModel : StationViewModelBase<Ride>
    {

        public RegistrationListViewModel()
        {
            this.AddCommand = new RelayCommand(Add);
            Messenger.Default.Register<Ride>(this, "Open", Open);
        }

        private void Add()
        {
            var vm = new RegistrationViewModel {Ride = this.Ride, Rider = new Registration {RideId = this.Ride.RideId}};
            var window = new RegistrationDetailsWindow(vm);
            window.ShowDialog();
        }

        protected async override void Open(Ride item)
        {
            this.ShowBusy("Loading ...");
            this.Ride = item;
            this.RegistrationCollection.Clear();
            var query = this.Context.Registrations.Where(r => r.RideId == item.RideId);
            var lo = await this.Context.LoadAsync(query);
            this.RegistrationCollection.AddRange(lo.ItemCollection);
            var page = 1;
            while (lo.HasNextPage)
            {
                lo = await this.Context.LoadAsync(query, page++);
                this.RegistrationCollection.AddRange(lo.ItemCollection);
            }

            this.HideBusy();

        }

        private Ride m_ride;
        private readonly ObjectCollection<Registration> m_registrationCollection = new ObjectCollection<Registration>();

        public ObjectCollection<Registration> RegistrationCollection
        {
            get { return m_registrationCollection; }
        }
        public Ride Ride
        {
            get { return m_ride; }
            set
            {
                m_ride = value;
                RaisePropertyChanged("Ride");
            }
        }

        public RelayCommand AddCommand { get; private set; }

        public void SaveRegistration(Registration reg)
        {

        }


        public void ViewRegistrationDetails(int registrationId)
        {
        }

        public void DeleteRegistration(int registrationId)
        {

        }

        public void ViewPayment(int registrationId)
        {
            MakePayment(registrationId);
        }

        public void MakePayment(int registrationId)
        {
        }

    }
}
