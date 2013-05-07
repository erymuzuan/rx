using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Windows;
using Bespoke.Cycling.Domain;
using Bespoke.Cycling.Windows.Infrastructure;
using Bespoke.Cycling.Windows.RideOrganizerModule.Contants;
using Bespoke.Cycling.Windows.RideOrganizerModule.Views;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Win32;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels
{
    [Export]
    public partial class RideViewModel : StationViewModelBase<Ride>
    {
        public RelayCommand CreateNewRideCommand { get; set; }
        public RelayCommand AddCommand { get; set; }
        public RelayCommand<Ride> SaveCommand{ get; set; }
        public RelayCommand<Ride> DeleteCommand{ get; set; }
        public RelayCommand CancelCommand{ get; set; }
        private Ride m_ride;
        private ObjectCollection<Ride> m_rideCollectionSource = new ObjectCollection<Ride>();
        private ObjectCollection<Ride> m_newRideCollectionSource = new ObjectCollection<Ride>();

        public RelayCommand<RideCategory> RemoveCategoryCommand { get; set; }

        public RelayCommand AddFaqSectionCommand { get; set; }
        public RelayCommand<Faq> RemoveFaqSectionCommand { get; set; }

        public RelayCommand<Faq> AddFaqItemCommand { get; set; }
        public RelayCommand<FaqItem> RemoveFaqItemSectionCommand { get; set; }

        //itenerary
        public RelayCommand AddItinerarySectionCommand { get; set; }
        public RelayCommand<Itinerary> RemoveItinerarySectionCommand { get; set; }

        public RelayCommand<Itinerary> AddItineraryItemCommand { get; set; }
        public RelayCommand<ItineraryItem> RemoveItineraryItemCommand { get; set; }

        public RelayCommand AddDocumentCommand { get; set; }
        public RelayCommand<Document> RemoveDocumentCommand { get; set; }

        public RelayCommand AddLogoSmallCommand { get; set; }
        public RelayCommand AddLogoLargeCommand { get; set; }

        public RelayCommand AddRuleCommand { get; set; }
        public RelayCommand<Rule> RemoveRuleCommand { get; set; }

        public RelayCommand AddSponsorCommand { get; set; }
        public RelayCommand<Sponsor> RemoveSponsorCommand { get; set; }

        public RelayCommand<GeoLocation> SelectLocationCommand { get; set; }

        public event EventHandler NavigateHome;

        public RelayCommand<Ride> ApproveCommand { get; set; }
        public RelayCommand<Ride> PublishCommand { get; set; }
        public RelayCommand<Ride> DisapproveCommand { get; set; }
        public RelayCommand<Ride> DepublishCommand { get; set; }

        public RideViewModel()
        {
            this.CreateNewRideCommand = new RelayCommand(() =>
                this.Ride = new Ride
                        {
                            StartDate = DateTime.Today,
                            EndDate = DateTime.Today,

                            StartLocation = new GeoLocation { Name = "Pick one" },
                            EndLocation = new GeoLocation { Name = "Pick One" },
                            RegistrationInfo = new RegistrationInfo
                                                    {
                                                        OpeningDate = DateTime.Today,
                                                        ClosingDate = DateTime.Today.AddMonths(6)
                                                    }

                        });
            this.RemoveCategoryCommand = new RelayCommand<RideCategory>(RemoveCategory, c => null != c);

            this.AddFaqSectionCommand = new RelayCommand(() => this.Ride.FaqCollection.Add(new Faq { Section = "<Section name>" }));
            this.RemoveFaqSectionCommand = new RelayCommand<Faq>(f => this.Ride.FaqCollection.Remove(f), f => null != f && null != this.Ride);
            this.AddFaqItemCommand = new RelayCommand<Faq>(s => s.FaqItemCollection.Add(new FaqItem { Question = "<Question>", Answer = "<Answer>" }), s => null != s);
            this.RemoveFaqItemSectionCommand = new RelayCommand<FaqItem>(f => this.SelectedFaqSection.FaqItemCollection.Remove(f), f => null != f && null != this.SelectedFaqSection);

            this.AddItinerarySectionCommand = new RelayCommand(() => this.Ride.ItineraryCollection.Add(new Itinerary { Title = "<Section Name>", Date = DateTime.Today }));
            this.RemoveItinerarySectionCommand = new RelayCommand<Itinerary>(f => this.Ride.ItineraryCollection.Remove(f), f => null != f && null != this.Ride);
            this.AddItineraryItemCommand = new RelayCommand<Itinerary>(AddItineraryItem, s => null != s);
            this.RemoveItineraryItemCommand = new RelayCommand<ItineraryItem>(f => this.SelectedItinerarySection.ItineraryItemCollection.Remove(f), f => null != f && null != this.SelectedItinerarySection);


            this.SelectLocationCommand = new RelayCommand<GeoLocation>(SelectLocation, g => null != g);

            this.AddLogoLargeCommand = new RelayCommand(AddLargeLogo);
            this.AddLogoSmallCommand = new RelayCommand(AddSmallLogo);
            this.AddDocumentCommand = new RelayCommand(AddDocument);
            this.RemoveDocumentCommand = new RelayCommand<Document>(f => this.Ride.DocumentCollection.Remove(f), f => null != f && null != this.Ride);

            this.AddSponsorCommand = new RelayCommand(() => this.Ride.SponsorCollection.Add(new Sponsor { Bil = this.Ride.SponsorCollection.Count + 1 }));
            this.RemoveSponsorCommand = new RelayCommand<Sponsor>(f => this.Ride.SponsorCollection.Remove(f), f => null != f && null != this.Ride);

            this.AddRuleCommand = new RelayCommand(()=> this.Ride.RuleCollection.Add(new Rule { Bil = this.Ride.RuleCollection.Count + 1 }));
            this.RemoveRuleCommand = new RelayCommand<Rule>(f => this.Ride.RuleCollection.Remove(f), f => null != f && null != this.Ride);


            this.AddCommand = new RelayCommand(this.Add,
                () => null != this.Ride
               && !string.IsNullOrWhiteSpace(this.Ride.Title)
               && !string.IsNullOrWhiteSpace(this.Ride.FriendlyName)
               );

            this.PublishCommand = new RelayCommand<Ride>(Publish, r => null != r && r.IsApproved && !r.IsPublished);
            this.DepublishCommand = new RelayCommand<Ride>(Depublish, r => null != r && r.IsApproved && r.IsPublished);

            this.ApproveCommand = new RelayCommand<Ride>(Approve, r => null != r);
            this.DisapproveCommand = new RelayCommand<Ride>(Disapprove, r => null != r);

            this.SaveCommand = new RelayCommand<Ride>(Save, CanSave);
            this.DeleteCommand = new RelayCommand<Ride>(Delete, CanDelete);

            Messenger.Default.Register<Ride>(this,"Open", Open);

        }

        protected override void Open(Ride item)
        {
            this.Ride = item;
            this.ChangeView(ViewNames.EVENT_DETAIL_VIEW);
        }


        private bool CanDelete(Ride arg)
        {
            if (null == arg) return false;
            if (string.IsNullOrWhiteSpace(arg.Title)) return false;
            return true;
        }

        private bool CanSave(Ride arg)
        {
            if (null == arg) return false;
            if (string.IsNullOrWhiteSpace(arg.Title)) return false;
            return true;
        }

        private void AddItineraryItem(Itinerary s)
        {
            s.ItineraryItemCollection.Add(new ItineraryItem { Time = DateTime.Today.AddHours(7) });
        }

        private async void AddSmallLogo()
        {
            var ofd = new OpenFileDialog { Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG" };
            if (ofd.ShowDialog() ?? false)
            {
                var store = ObjectBuilder.GetObject<IBinaryStore>();
                var fileName = ofd.FileName;
                var storeId = Guid.NewGuid().ToString();
                byte[]  content = File.ReadAllBytes(fileName);
                
                if ( content.Length > 0)
                {
                    await store.AddAsync(content, Path.GetExtension(fileName), storeId);
                    this.Ride.LogoSmallStoreId = storeId;
                }
            }
        }

        private async void AddLargeLogo()
        {
            var ofd = new OpenFileDialog { Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG" };
            if (ofd.ShowDialog() ?? false)
            {

                var store = ObjectBuilder.GetObject<IBinaryStore>();
                var fileName = ofd.FileName;
                var storeId = Guid.NewGuid().ToString();
                byte[] content = File.ReadAllBytes(fileName);

                if (content.Length > 0)
                {
                    await store.AddAsync(content, Path.GetExtension(fileName), storeId);
                    this.Ride.LogoLargeStoreId = storeId;
                }
            }
        }

        private async void Approve(Ride ride)
        {
            this.IsBusy = true;
            var ctx = new CyclingDataContext();
            using (var session = ctx.OpenSession())
            {
                ride.IsApproved = true;
                session.Attach(ride);
                await session.SubmitChanges();
            }
            this.IsBusy = false;
        }

        private async void Disapprove(Ride ride)
        {
            this.IsBusy = true;
            var ctx = new CyclingDataContext();
            using (var session = ctx.OpenSession())
            {
                ride.IsApproved = false;
                session.Attach(ride);
                await session.SubmitChanges();
            }
            this.IsBusy = false;

        }

        private async void Depublish(Ride ride)
        {
            this.IsBusy = true;
            ride.IsPublished = false;
            using (var session = this.Context.OpenSession())
            {
                session.Attach(ride);
                await session.SubmitChanges();

            }
            this.PublishCommand.RaiseCanExecuteChanged();
            this.DepublishCommand.RaiseCanExecuteChanged();
            this.IsBusy = false;
        }

        private async void Publish(Ride ride)
        {
            this.IsBusy = true;
            ride.IsPublished = true;
            using (var session = this.Context.OpenSession())
            {
                session.Attach(ride);
                await session.SubmitChanges();

            }
            this.PublishCommand.RaiseCanExecuteChanged();
            this.DepublishCommand.RaiseCanExecuteChanged();
            this.IsBusy = false;


        }


        private void SelectLocation(GeoLocation location)
        {

            var cm = new GeoLocationViewModel();
            var ad = new GeoLocationDialog { DataContext = cm };
            ad.Closed += delegate
                             {
                                 if (ad.DialogResult ?? false)
                                 {
                                     var loc = cm.SelectedLocation;
                                     location.Name = loc.Name;
                                     location.Lat = loc.Lat;
                                     location.Lng = loc.Lng;
                                     location.Type = loc.Type;
                                     location.Address = loc.Address.Clone();
                                     location.GeoLocationId = loc.GeoLocationId;
                                     if (loc.GeoLocationId == 0) throw new InvalidOperationException("Geolocation ID is 0");

                                     // assign the loc
                                     this.Ride.StartLocationId = this.Ride.StartLocation.GeoLocationId;
                                     this.Ride.EndLocationId = this.Ride.EndLocation.GeoLocationId;
                                 }
                             };
            ad.Show();

        }

        private async void AddDocument()
        {
            var ofd = new OpenFileDialog();
            if (ofd.ShowDialog() ?? false)
            {
                var fileName = ofd.FileName;
                var doc = new Document
                {
                    Bil = this.Ride.DocumentCollection.Count + 1,
                    StoreId = Guid.NewGuid().ToString(),
                    Title = Path.GetFileNameWithoutExtension(fileName),
                    Extension = Path.GetExtension(fileName),
                    Summary = "<Your summary>",
                    Group = "<File Group>"
                };
                this.Ride.DocumentCollection.Add(doc);

                var file = new BinaryStore
                    {
                        StoreId = doc.StoreId,
                        Extension = Path.GetExtension(fileName),
                        Content = File.ReadAllBytes(fileName)
                    };

                if (null != file.Content && file.Content.Length > 0)
                {
                    var store = ObjectBuilder.GetObject<IBinaryStore>();
                    this.IsBusyUploadDocument = true;
                    await store.AddAsync(file);

                    this.IsBusyUploadDocument = false;
                }
            }
        }




        private void RemoveCategory(RideCategory cat)
        {
            this.Ride.RideCategoryCollection.Remove(cat);
        }

        public async void LoadById(int id)
        {
            if (id < 1) throw new ArgumentException(message: "Ride id cannot be 0", paramName: "id");
            this.IsBusy = true;
            this.Ride = await this.Context.LoadOneAsync<Ride>(r => r.RideId == id);
            this.IsBusy = false;

        }

        public async void LoadRides(string owner)
        {
            this.IsBusy = true;
            if (string.IsNullOrEmpty(owner))
            {
                var noOwnerRideQuery = this.Context.Rides.Where(r => r.IsApproved && r.Owner == null);
                var noOwnerRidesLo = await this.Context.LoadAsync(noOwnerRideQuery);
                this.RideCollectionSource.ClearAndAddRange(noOwnerRidesLo.ItemCollection);
            }
            else
            {
                var query = this.Context.Rides.Where(r => r.IsApproved && r.Owner == owner);
                var lo = await this.Context.LoadAsync(query);
                this.RideCollectionSource.ClearAndAddRange(lo.ItemCollection);
            }

            this.IsBusy = false;
        }

        [Import]
        public LoginInfoViewModel LoginInfoViewModel { get; set; }
        public async void Add()
        {
            this.IsBusy = true;
            var rd = this.Ride;
            rd.CreatedDate = DateTime.Now;
            rd.Owner = this.LoginInfoViewModel.UserName;

            if (null == Ride)
                throw new InvalidOperationException(@"RideEvent cannot be null");


            if (!rd.Validate()) return;

            using (var session = this.Context.OpenSession())
            {
                session.Attach(rd);
                await session.SubmitChanges();
            }

            this.IsBusy = false;
            if (NavigateHome != null) NavigateHome(this, new EventArgs());


        }

        private async void Save(Ride rd)
        {
            this.IsBusy = true;
            using (var session = this.Context.OpenSession())
            {
                session.Attach(rd);
                await session.SubmitChanges();
            }
            this.Alert( "Successfully update event.");
            this.IsBusy = false;

        }

        public void Delete(Ride ride)
        {
            var rd = string.Format("Are you sure to delete '{0}' item?", ride.Title);
            if (MessageBox.Show(rd, "Delete Event", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel)
                return;

            //this.IsBusy = true;
            //var repos = ObjectBuilder.GetObject<IRideRepository>();
            //repos.Delete(ride);
            //repos.SaveChanges(t => this.IsBusy = false);
        }

        public void Cancel()
        {
            if (null != NavigateHome)
                NavigateHome(this, new EventArgs());
        }


        public Ride Ride
        {
            get { return m_ride; }
            set
            {
                m_ride = value;
                RaisePropertyChanged("Ride");
            }
        }

        public ObjectCollection<Ride> RideCollectionSource
        {
            get { return m_rideCollectionSource; }
            set
            {
                m_rideCollectionSource = value;
            }
        }

        public ObjectCollection<Ride> NewRideCollectionSource
        {
            get { return m_newRideCollectionSource; }
            set
            {
                m_newRideCollectionSource = value;
            }
        }

        public async void LoadNewRideEvents()
        {
            var ctx = new CyclingDataContext();
            var query = ctx.Rides.Where(r => !r.IsApproved);
            var lo = await ctx.LoadAsync(query);
            this.NewRideCollectionSource.ClearAndAddRange(lo.ItemCollection);
        }
    }
}
