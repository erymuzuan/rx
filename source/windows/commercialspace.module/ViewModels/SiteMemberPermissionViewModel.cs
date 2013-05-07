using System.ComponentModel.Composition;
using System.Threading.Tasks;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using GalaSoft.MvvmLight.Command;
using System.Linq;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels
{
    [Export]
    public class SiteMemberPermissionViewModel : StationViewModelBase<SiteMember>
    {
        public RelayCommand<string> SearchMemberCommand { get; set; }
        public RelayCommand SaveCommand { get; set; }
        public RelayCommand<SiteMember> AddMemberCommand { get; set; }
        public RelayCommand<RidePermission> RemoveMemberCommand { get; set; }


        public SiteMemberPermissionViewModel()
        {
            if (IsInDesignMode) return;

            this.SaveCommand = new RelayCommand(Save, () => null != this.SelectedRide);
            this.SearchMemberCommand = new RelayCommand<string>(SearchSiteMember, s => !string.IsNullOrWhiteSpace(s));
            this.AddMemberCommand = new RelayCommand<SiteMember>(AddMember, m => null != m);
            this.RemoveMemberCommand = new RelayCommand<RidePermission>(RemoveMember, p => null != p);

        }


        private async void RemoveMember(RidePermission perm)
        {
            if (!this.Confirm("Are you sure you want to remove " + perm.SiteMemberName + " from the volunteer list?"))
                return;


            this.ShowBusy("Removing {0}..Please wait.", perm.SiteMemberName);
            using (var session = this.Context.OpenSession())
            {
                session.Delete(perm);
                await session.SubmitChanges();
            }

            this.RidePermissionCollection.Remove(perm);
            this.HideBusy();
        }

        private async void AddMember(SiteMember member)
        {
            this.ShowBusy("Saving new permissions..");
            var perm = new RidePermission
            {
                Permission = "organizer",
                SiteMemberId = member.SiteMemberId,
                SiteMemberName = member.UserName,
                RideId = this.SelectedRide.RideId,
                SiteMemberEmail = member.Email
            };
            using (var session = this.Context.OpenSession())
            {
                session.Attach(perm);
                await session.SubmitChanges();
            }
            this.HideBusy();

        }

        private async void SearchSiteMember(string term)
        {
            this.ShowBusy("Searching for {0}..Please wait.", term);
            var query = this.Context.SiteMembers.Where(m => m.UserName.Contains(term) || m.Email.Contains(term));
            var lo = await this.Context.LoadAsync(query);

            this.SearchResultCollection.ClearAndAddRange(lo.ItemCollection);
            this.HideBusy();

        }

        private RideViewModel m_rideViewModel;

        [Import]
        public RideViewModel RideViewModel
        {
            get { return m_rideViewModel; }
            set
            {
                m_rideViewModel = value;
                value.PropertyChanged += RideValuePropertyChanged;
                this.SelectedRide = value.Ride;
            }
        }

        async void RideValuePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "Ride" && null != m_rideViewModel.Ride)
            {
                this.SelectedRide = m_rideViewModel.Ride;
                await this.LoadingAsync();

            }
        }

        private void Save()
        {

        }


        private Ride m_selectedRide;
        private readonly ObjectCollection<SiteMember> m_searchResultCollection = new ObjectCollection<SiteMember>();
        private readonly ObjectCollection<RidePermission> m_ridePermissionCollection = new ObjectCollection<RidePermission>();

        public ObjectCollection<RidePermission> RidePermissionCollection
        {
            get { return m_ridePermissionCollection; }
        }
        public ObjectCollection<SiteMember> SearchResultCollection
        {
            get { return m_searchResultCollection; }
        }

        public Ride SelectedRide
        {
            get { return m_selectedRide; }
            set
            {
                m_selectedRide = value;
                RaisePropertyChanged("SelectedRide");
                this.SaveCommand.RaiseCanExecuteChanged();
            }
        }

        public async Task LoadingAsync()
        {
            this.ShowBusy("Loading members..");
            var query = this.Context.RidePermissions.Where(p => p.RideId == this.SelectedRide.RideId);
            var lo = await this.Context.LoadAsync(query);
            this.RidePermissionCollection.ClearAndAddRange(lo.ItemCollection);

            var page = 1;
            while (lo.HasNextPage)
            {
                lo = await this.Context.LoadAsync(query, page: ++page);
                this.RidePermissionCollection.AddRange(lo.ItemCollection);
            }
            this.HideBusy();


        }
    }
}
