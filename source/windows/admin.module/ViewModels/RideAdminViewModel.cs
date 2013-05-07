using System;
using System.ComponentModel.Composition;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using GalaSoft.MvvmLight.Command;

namespace Bespoke.Cycling.Windows.AdminModule.ViewModels
{
    [Export]
    public class RideAdminViewModel : StationViewModelBase<Ride>
    {
        public RelayCommand<Ride> SaveCommand { get; set; }
        private readonly CyclingDataContext m_context;

        public RideAdminViewModel()
        {
            m_context = new CyclingDataContext();
            this.Ride = new Ride
            {
                Title = "Please insert title",
                EndDate = DateTime.Today.AddMonths(1),
                StartDate = DateTime.Today,
                CreatedDate = DateTime.Now

            };
            this.SaveCommand = new RelayCommand<Ride>(SaveCommandExecute, r => r != null &&
            !string.IsNullOrEmpty(r.Title));
        }

        private async void SaveCommandExecute(Ride ride)
        {
            this.IsBusy = true;
            ride.CreatedDate = DateTime.Now;
            using (var session = m_context.OpenSession())
            {
                session.Attach(ride);
                await session.SubmitChanges();
            }

            this.IsBusy = false;
        }

        public void Load()
        {
            if (null != this.Ride) return;
            this.Ride = new Ride
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today.AddMonths(3),
            };

            this.Ride.PropertyChanged += (sender, args) => this.SaveCommand.RaiseCanExecuteChanged();
        }


        private Ride m_ride;
        public Ride Ride
        {
            get { return m_ride; }
            set
            {
                m_ride = value;
                RaisePropertyChanged("Ride");
            }
        }

    }
}