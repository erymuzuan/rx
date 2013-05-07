using System;
using System.ComponentModel.Composition;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using Bespoke.Cycling.Windows.RideOrganizerModule.Contants;
using GalaSoft.MvvmLight.Command;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels
{
    [Export]
    public class RideListViewModel : StationViewModelBase<Ride>
    {
        public RelayCommand<string> SearchCommad { get; set; }
        public RelayCommand<Ride> EditRideCommand { get; set; }
        public RelayCommand<Ride> DeleteRideCommand { get; set; }
        public RelayCommand<Ride> OpenRegistrationCommand { get; set; }

        public RideListViewModel()
        {
            this.SearchCommad = new RelayCommand<string>(Search, s => !string.IsNullOrWhiteSpace(s));
            this.OpenRegistrationCommand = new RelayCommand<Ride>(OpenRegistration);
        }

        private void OpenRegistration(Ride item)
        {
            ChangeView(ViewNames.EVENT_REGISTRATION_LIST_VIEW);
            Messenger.Default.Send(item, "Open");
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
        protected override void Open(Ride item)
        {
            ChangeView(ViewNames.EVENT_DETAIL_VIEW);
            Messenger.Default.Send(item,"Open");
        }

        private readonly ObjectCollection<Ride> m_currentRideCollection = new ObjectCollection<Ride>();

        public ObjectCollection<Ride> CurrentRideCollection
        {
            get { return m_currentRideCollection; }
        }


        protected async override void OnViewReady()
        {
            this.ShowBusy("Loading all upcoming events...");
            var query = this.Context.Rides
                            .Where(r => r.StartDate >= DateTime.Today.AddMonths(-1))
                            .Where(r => r.IsApproved == true)
                            ;

            var lo =await this.Context.LoadAsync(query);
            this.CurrentRideCollection.AddRange(lo.ItemCollection);

            this.HideBusy();
        }
        
    }
}