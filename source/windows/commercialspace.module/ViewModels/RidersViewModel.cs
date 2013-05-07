using System;
using System.ComponentModel.Composition;
using Bespoke.Cycling.Domain;
using GalaSoft.MvvmLight.Command;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels
{
    [Export]
    public partial class RidersViewModel
    {
        public RelayCommand AddIndividualRegistrationCommand { get; set; }
        public RelayCommand AddGroupCommand { get; set; }
        public RelayCommand<Registration> EditCommand { get; set; }

        public RidersViewModel()
        {
            if (this.IsInDesignMode) return;

            this.AddIndividualRegistrationCommand = new RelayCommand(AddIndividualRegistration);
            this.AddGroupCommand = new RelayCommand(AddGroup);
            this.EditCommand = new RelayCommand<Registration>(Edit);
        }


        public void Load(int id)
        {
            if (id < 1) throw new ArgumentException("Event id must be > 0");

            this.IsBusy = true;
            //var repos = ObjectBuilder.GetObject<IRideRepository>();
            //repos.Load(id, r =>
            //{
            //    this.Ride = r;
            //    this.IsBusy = false;
            //});

            //
            LoadRegistrations(id);
        }

        private void LoadRegistrations(int rideId)
        {
            //var repos = ObjectBuilder.GetObject<IRegistrationRepository>();
            //repos.GetRegistrations(items => RegistrationCollectionSource.ClearAndAddRange(items.ToObjectCollection().FindAll(r => r.RideId == rideId)));
        }



        private void NewRegistration(Registration register)
        {
            this.IsBusy = true;

            //register.IsBusyLoading = true;
            //register.LoadingDone += delegate
            //{
            //    if (this.IsBusy)
            //        new NotifyWindow { Message = "Successfully save registration.", Title = "Registration of Rider" }.Show();
            //    this.IsBusy = false;
            //};

            //register.AddNew(string.Empty, errors =>
            //{
            //    if (errors.Count() > 0)
            //    {
            //        var list = new StringBuilder();
            //        errors.ToObjectCollection().ForEach(e => list.AppendFormat("- {0}", e));
            //        new NotifyWindow { Message = list.ToString(), Title = "Error Notification" }.Show();
            //        IsBusy = false;
            //        return;
            //    }

            //    register.Save(t =>
            //                      {
            //                          this.IsBusy = false;
            //                          if (!t)
            //                              new NotifyWindow { Message = "Fail to save registration.", Title = "Error Notification" }.Show();
            //                          else
            //                          {
            //                              this.RegistrationCollectionSource.AddRange(register);
            //                          }
            //                      });
            //});

        }



        public void NewGroupsRegistration(ObjectCollection<Registration> riders)
        {
            this.IsBusy = true;
            //var repos = ObjectBuilder.GetObject<IRegistrationRepository>();
            //riders.ForEach(repos.Add);
            this.IsBusy = false;
            //new NotifyWindow { Message = "Successfully save registration.", Title = "Registration of Riders" }.Show();
        }

        internal void UpdateRegistration(Registration reg)
        {
            this.IsBusy = true;
            //save
        }
    }
}
