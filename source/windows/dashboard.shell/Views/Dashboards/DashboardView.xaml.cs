using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Controls;
using Bespoke.Cycling.Windows.Infrastructure;
using Bespoke.Station.Windows.Helpers;
using Bespoke.Station.Windows.Infrastructure;
using Bespoke.Station.Windows.ViewModels;
using Telerik.Windows.Controls;

namespace Bespoke.Station.Windows.Views
{
    [Export("ViewContract", typeof(UserControl))]
    [ViewMetadata(Group = ViewGroup.Home, Name = ViewNames.DASHBOARD_VIEW, Caption = "Papan Tugas", Order = 1, IsHome = true, Image = "/Images/home.png")]
    public partial class DashboardView :IPartImportsSatisfiedNotification
    {
        public DashboardView()
        {
            InitializeComponent();
            
            this.dashboardItemTileView.TileStateChanged += dashboardItemTileView_TileStateChanged;
        }

        void dashboardItemTileView_TileStateChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            var item = e.OriginalSource as RadTileViewItem;
            if (item == null) return;
            RadFluidContentControl fluid = item.ChildrenOfType<RadFluidContentControl>().FirstOrDefault();
            if (fluid == null) return;
            switch (item.TileState)
            {
                case TileViewItemState.Maximized:
                    fluid.State = FluidContentControlState.Large;
                    break;
                case TileViewItemState.Minimized:
                case TileViewItemState.Restored:
                    fluid.State = FluidContentControlState.Normal;
                    break;
            }
        }


        [Import]
        public DashboardViewModel ViewModel
        {
            get { return this.DataContext as DashboardViewModel; }
            set{this.DataContext = value;}
        }

        [ImportMany(typeof(RadTileViewItem))]
        public RadTileViewItem[] TileItems { set; get; }

        public void OnImportsSatisfied()
        {
            foreach (var item in this.TileItems.OrderBy(v => v.Tag))
            {
                this.dashboardItemTileView.Items.Add(item);
            }
        }
    }
}
