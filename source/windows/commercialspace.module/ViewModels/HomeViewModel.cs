using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using GalaSoft.MvvmLight.Command;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels
{
    public class HomeViewModel : StationViewModelBase<Ride>
    {
        public RelayCommand CreateNewRideCommand { get; set; }

        public HomeViewModel()
        {
            this.CreateNewRideCommand = new RelayCommand(Create);

        }

        private void Create()
        {
            throw new System.NotImplementedException();
        }


        private readonly ObjectCollection<Ride> m_rideCollection = new ObjectCollection<Ride>();

        public ObjectCollection<Ride> RideCollection
        {
            get { return m_rideCollection; }
        }
    }
}
