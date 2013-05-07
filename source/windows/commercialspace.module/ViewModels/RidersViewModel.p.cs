using Bespoke.Cycling.Domain;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels
{
    public partial class RidersViewModel
    {


        private Ride m_ride;
        private ObjectCollection<Registration> m_registrationCollectionSource = new ObjectCollection<Registration>();

   
        public Ride Ride
        {
            get { return m_ride; }
            set
            {
                m_ride = value;
                RaisePropertyChanged("Ride");
            }
        }

        private bool m_isBusy;
        public bool IsBusy
        {
            get { return m_isBusy; }
            set
            {
                m_isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public ObjectCollection<Registration> RegistrationCollectionSource
        {
            get { return m_registrationCollectionSource; }
            set
            {
                m_registrationCollectionSource = value;
            }
        }
    }
}
