using System.Linq;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using Bespoke.Cycling.Windows.RideOrganizerModule.Views;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels
{
    public partial class RidersViewModel : StationViewModelBase<Registration>
    {
        private void AddGroup()
        {

            var riders = new ObjectCollection<Registration>();
            var categories = (from c in this.Ride.RideCategoryCollection
                              select c.Category).ToObjectCollection();
            if (categories.Count == 0)
                categories.Add("?");

            var groupWindow = new AddGroupRidersWindow
                {
                DataContext = this,
                Categories = categories,
                RegistrationCollection = riders
            };

            groupWindow.Closed += (s, ea) =>
            {
                if (groupWindow.DialogResult ?? false)
                {
                    this.NewGroupsRegistration(riders);
                }
            };
            groupWindow.Show();
        }


        private void AddIndividualRegistration()
        {
            var rider = new Registration
                {
                RideId = this.Ride.RideId,
                PaymentStatus = "None",
                Status = "New"
            };
            
            var rvm = new RiderRegistrationViewModel(this.Ride, rider);
            var riderWindow = new AddRiderWindow { DataContext = rvm};
            riderWindow.Closed += (s, ea) =>
            {
                if (riderWindow.DialogResult ?? false)
                {
                    this.NewRegistration(rider);
                }
            };
            riderWindow.Show();
        }


        private void Edit(Registration reg)
        {
            var rvm = new RiderRegistrationViewModel(this.Ride, reg);
            var riderWindow = new AddRiderWindow { DataContext = rvm};
            riderWindow.Closed += (s, ea) =>
            {
                if (riderWindow.DialogResult ?? false)
                {
                    this.UpdateRegistration(reg);
                }
            };
            riderWindow.Show();
        }
    }
}
