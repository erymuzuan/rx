using System;
using System.ComponentModel.Composition;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using GalaSoft.MvvmLight.Command;
using System.Linq;

namespace Bespoke.Cycling.Windows.AdminModule.ViewModels
{
    [Export]
    public class RideListViewModel : StationViewModelBase<Ride>
    {
        public RelayCommand<string> SearchCommad { get; set; }
        public RelayCommand<Ride> EditRideCommand { get; set; }
        public RelayCommand<Ride> DeleteRideCommand { get; set; }

        public RideListViewModel()
        {
            this.ApproveCommand = new RelayCommand<Ride>(Approve);
            this.SearchCommad = new RelayCommand<string>(Search, s => !string.IsNullOrWhiteSpace(s));
        }

        private async void Approve(Ride ride)
        {
            this.ShowBusy("Saving ..");

            using (var session = this.Context.OpenSession())
            {
                ride.IsApproved = true;
                session.Attach(ride);
                await session.SubmitChanges();
            }

            this.Alert("Has been approved");
            this.HideBusy();
        }

        private async void Search(string text)
        {
            this.ShowBusy("Searching for {0}. Please wait..");
            var query = this.Context.Rides
                            .Where(r => r.Title.Contains(text)
                            || r.Organizer.Name.Contains(text)
                            );
            var lo = await this.Context.LoadAsync(query);
            this.CurrentRideCollection.ClearAndAddRange(lo.ItemCollection);

            this.HideBusy();
        }

        private readonly ObjectCollection<Ride> m_currentRideCollection = new ObjectCollection<Ride>();

        public ObjectCollection<Ride> CurrentRideCollection
        {
            get { return m_currentRideCollection; }
        }

        public RelayCommand<Ride> ApproveCommand { get; set; }

        protected async override void OnViewReady()
        {
            this.ShowBusy("Loading all upcoming events...");
            var query = this.Context.Rides
                            .Where(r => r.StartDate >= DateTime.Today.AddMonths(-1));

            var lo =await this.Context.LoadAsync(query);
            this.CurrentRideCollection.AddRange(lo.ItemCollection);

            this.HideBusy();
        }
        
    }
}