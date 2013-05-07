using System.ComponentModel.Composition;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Bespoke.Cycling.Windows.RideOrganizerModule.ViewModels;
using Bespoke.Cycling.Windows.RideOrganizerModule.Views.Events;

namespace Bespoke.Cycling.Windows.RideOrganizerModule.Views
{
    [Export("RideViewContract", typeof(UserControl))]
    [RideViewMetadata(Caption = "Volunteers", Order = 2)]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public partial class RidePermissionView
    {
        public RidePermissionView()
        {
            InitializeComponent();

        }

        [Import]
        public SiteMemberPermissionViewModel ViewModel
        {
            get { return this.DataContext as SiteMemberPermissionViewModel; }
            set { this.DataContext = value; }
        }


        private async void AddVolunteerButtonClick(object sender, RoutedEventArgs e)
        {
            var child = new SiteMemberSearchDialog { DataContext = this.ViewModel };
            child.ShowDialog();
            if (child.DialogResult ?? false)
            {
                this.ViewModel.IsBusy = true;
                await Task.Delay(750);
                await this.ViewModel.LoadingAsync();
            }
        }





    }
}
