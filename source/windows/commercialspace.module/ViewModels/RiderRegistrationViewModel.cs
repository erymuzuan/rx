using System.ComponentModel.Composition;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using GalaSoft.MvvmLight.Command;
using System.Linq;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels
{
    [Export]
    public class RiderRegistrationViewModel : StationViewModelBase<Registration>
    {
        public RelayCommand CancelCommand { get; set; }
        public RelayCommand OkCommand { get; set; }
        public RelayCommand AddEmergencyContactCommand { get; set; }
        public RelayCommand<EmergencyContact> RemoveEmergencyContactCommand { get; set; }
        public RiderRegistrationViewModel(Ride ride,Registration registration)
        {
            if (this.IsInDesignMode) return;
            this.RideData = ride;
            this.Registration = registration;
            this.Registration.RideId = ride.RideId;

            this.EventCategoryCollection.ClearAndAddRange(ride.RideCategoryCollection.Select(c => c.Category) );

            this.OkCommand = new RelayCommand(Save, ()=> null != this.Registration);
            this.AddEmergencyContactCommand = new RelayCommand(
                () => this.Registration.EmergencyContactCollection.Add(new EmergencyContact()), 
                () => null != this.Registration);

            this.RemoveEmergencyContactCommand = new RelayCommand<EmergencyContact>(
                ec => this.Registration.EmergencyContactCollection.Remove(ec),
                ec => null != ec && null != this.Registration);


        }

        private void Save()
        {
            
        }


        private Ride m_rideData;
        private Registration m_registration;

        public Registration Registration
        {
            get { return m_registration; }
            set
            {
                m_registration = value;
                RaisePropertyChanged("Registration");
            }
        }

        public Ride RideData
        {
            get { return m_rideData; }
            set
            {
                m_rideData = value;
                RaisePropertyChanged("RideData");
            }
        }
        private readonly ObjectCollection<string > m_eventCategoryCollection = new ObjectCollection<string >();
        public ObjectCollection<string > EventCategoryCollection
        {
            get { return m_eventCategoryCollection; }
        }

    }
}
