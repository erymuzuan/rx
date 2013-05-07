using System.ComponentModel.Composition;
using System.Diagnostics;
using Bespoke.Station.Windows.ViewModels.Dashboard;
using Telerik.Windows.Controls;

namespace Bespoke.Station.Windows.Views.Dashboards
{
    [Export(typeof(RadTileViewItem))]
    public partial class DashboardStatisticsView
    {
        public DashboardStatisticsView()
        {
            InitializeComponent();
        }

        
    }
}
