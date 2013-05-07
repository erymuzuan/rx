using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using GalaSoft.MvvmLight.Command;
using System.Linq;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels
{
    public class RegistrationViewModel : StationViewModelBase<Registration>
    {
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand<EmergencyContact> RemoveContactCommand { get; set; }
        public RelayCommand AddContactCommand { get; set; }

        public RegistrationViewModel()
        {
            if (this.IsInDesignMode) return;

            this.SaveCommand = new RelayCommand(Save, () => null != this.Rider
                && null != this.Ride);
            this.RemoveContactCommand = new RelayCommand<EmergencyContact>(RemoveEmergencyContact, c => null != c && null != this.Rider);
            this.AddContactCommand = new RelayCommand(AddEmergencyContact, () => null != this.Rider);
        }


        public async void Load(int registrationId)
        {
            this.IsBusy = true;
            this.Rider = await this.Context.LoadOneAsync<Registration>(r => r.RegistrationId == registrationId);
            if (null != this.Rider)
                this.Ride = await this.Context.LoadOneAsync<Ride>(r => r.RideId == this.Rider.RideId);


            this.IsBusy = false;
            this.SaveCommand.RaiseCanExecuteChanged();

        }
        private async void Save()
        {
            if (this.Rider.HasValidationErrors) return;
            this.IsBusy = true;
            using (var session = this.Context.OpenSession())
            {
                session.Attach(this.Rider);
                await session.SubmitChanges();
            }

            this.IsBusy = false;
        }


        private Registration m_rider;
        private Ride m_ride;

        public Ride Ride
        {
            get { return m_ride; }
            set
            {
                m_ride = value;
                RaisePropertyChanged("Ride");
                this.CategoryCollection.ClearAndAddRange(value.RideCategoryCollection.Select(c => c.Category));
            }
        }
        public Registration Rider
        {
            get { return m_rider; }
            set
            {
                m_rider = value;
                value.PropertyChanged += delegate { this.SaveCommand.RaiseCanExecuteChanged(); };
                RaisePropertyChanged("Rider");
            }
        }


        private readonly ObjectCollection<string> m_categoryCollection = new ObjectCollection<string>();
        public ObjectCollection<string> CategoryCollection
        {
            get { return m_categoryCollection; }
        }
        private void RemoveEmergencyContact(EmergencyContact contact)
        {
            this.Rider.EmergencyContactCollection.Remove(contact);
        }

        private void AddEmergencyContact()
        {
            this.Rider.EmergencyContactCollection.Add(new EmergencyContact());
        }

    }
}
