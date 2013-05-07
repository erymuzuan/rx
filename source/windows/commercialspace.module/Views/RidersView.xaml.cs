using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Bespoke.Cycling.Domain;
using System.ComponentModel;
using Bespoke.Cycling.Windows.RideOrganizerModule.Converters;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    [Export]
    public partial class RidersView : IPartImportsSatisfiedNotification
    {
        public RidersView()
        {
            InitializeComponent();
        }

        [Import]
        public RidersViewModel ViewModel { get; set; }

        public static readonly DependencyProperty EventIdProperty =
            DependencyProperty.Register("EventId", typeof(int), typeof(RidersView), new PropertyMetadata(0));

        public int EventId
        {
            get { return (int)GetValue(EventIdProperty); }
            set { SetValue(EventIdProperty, value); }
        }


        //// Executes when the user navigates to this page.
        //protected override void OnNavigatedTo(NavigationEventArgs e)
        //{
        //    EventId = PresentationHelper.ParseIdFromUri(e.Uri);
        //    if (null == this.ViewModel) return;
        //    this.DataContext = this.ViewModel;
        //    this.ViewModel.Load(this.EventId);
        //}

        public void OnImportsSatisfied()
        {
            if (EventId > 0 && null != this.ViewModel && this.ViewModel.Ride == null)
            {
                this.DataContext = this.ViewModel;
                this.ViewModel.Load(this.EventId);
                //this.Title = this.ViewModel.Ride.Title;
            }
        }


        private void EditRiderClicked(object sender, RoutedEventArgs e)
        {
            var reg = ((Button) sender).Tag as Registration;
            if (null == reg) return;

            var rvm = new RiderRegistrationViewModel(this.ViewModel.Ride, reg);
            var riderWindow = new AddRiderWindow { DataContext = rvm };
            riderWindow.Closed += (s, ea) =>
            {
                if (riderWindow.DialogResult ?? false)
                {
                    this.ViewModel.UpdateRegistration(reg);
                }
            };
            riderWindow.Show();
        }
    }
}
